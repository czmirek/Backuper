namespace Backuper.Library.Uploaders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Backuper.Library.Configuration;
    using Backuper.Library.Configuration.Locations;
    using Backuper.Library.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Uploader to the azure storage blob.
    /// </summary>
    internal class AzureStorageBlobUploader : IUploader
    {
        /// <summary>
        /// Configuration of Azure Storage
        /// </summary>
        private readonly AzureStorageBlob azureConfig;

        /// <summary>
        /// Backup filename naming service
        /// </summary>
        private readonly IBackupNamingService namingService;

        /// <summary>
        /// Logging service
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageBlobUploader"/> class.
        /// </summary>
        /// <param name="azureConfig">Azure Storage configuration</param>
        /// <param name="namingService">Backup file naming service</param>
        /// <param name="logger">Logging service</param>
        public AzureStorageBlobUploader(AzureStorageBlob azureConfig, IBackupNamingService namingService, ILogger logger)
        {
            this.azureConfig = azureConfig;
            this.namingService = namingService;
            this.logger = logger;
        }

        /// <summary>
        /// Cleans old backups in the Azure Storage Blob container
        /// </summary>
        /// <param name="target">Target configuration.</param>
        /// <returns>Async task</returns>
        public async Task CleanOldBackupsAsync(IBackupTarget target)
        {
            CloudBlobContainer container = this.GetContainer();

            // List all blobs in container
            List<IListBlobItem> blobs = await this.ListBlobsAsync(container);

            // List old backups that can be removed
            IEnumerable<CloudBlockBlob> blobsToRemove =
                blobs.OfType<CloudBlockBlob>()
                     .Where(x => this.namingService.IsNamedBackup(x.Name, target.ID))
                     .Select(blob => (blob, fileDate: this.namingService.ReadDateTime(blob.Name)))
                     .OrderByDescending(x => x.fileDate.BackupDate)
                     .Skip(target.Keep - 1)
                     .Select(x => x.blob);

            if (blobsToRemove.Any())
            {
                this.logger.LogTrace($"{blobsToRemove.Count()} old backups found");
            }
            else
            {
                this.logger.LogTrace("No old backup found");
            }

            // Remove old backups in list
            foreach (CloudBlockBlob blobToRemove in blobsToRemove)
            {
                this.logger.LogTrace($"Removing old backup: {blobToRemove.Name}");
                await blobToRemove.DeleteAsync();
                this.logger.LogTrace($"Removed");
            }
        }

        /// <summary>
        /// Uploads a specified file to Azure Storage Blob
        /// </summary>
        /// <param name="file">Local file</param>
        /// <returns>Async task</returns>
        public async Task UploadAsync(BackupFileWithDate file)
        {
            CloudBlobContainer container = this.GetContainer();
            CloudBlockBlob blobReference = container.GetBlockBlobReference(file.FileName);
            using (var fileStream = System.IO.File.OpenRead(file.FullPath))
            {
                this.logger.LogTrace($"Uploading file to Azure Storage blob {file.FileName}");
                await blobReference.UploadFromStreamAsync(fileStream);
                this.logger.LogTrace("Uploaded");
            }
        }

        /// <summary>
        /// Creates an <see cref="CloudBlobContainer"/> from the <see cref="BackupLocation.AzureStorageBlob"/> configuration.
        /// </summary>
        /// <returns>Container instance</returns>
        private CloudBlobContainer GetContainer()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                storageCredentials: new StorageCredentials(
                    accountName: this.azureConfig.AccountName,
                    keyValue: this.azureConfig.AccountKey),
                useHttps: true);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(this.azureConfig.ContainerName);
        }

        /// <summary>
        /// Lists all blobs in specified container.
        /// </summary>
        /// <param name="container">Container to list all blobs from.</param>
        /// <returns>List of blobs</returns>
        private async Task<List<IListBlobItem>> ListBlobsAsync(CloudBlobContainer container)
        {
            BlobContinuationToken continuationToken = null;
            List<IListBlobItem> results = new List<IListBlobItem>();
            do
            {
                this.logger.LogTrace("Downloading blob list from Azure Storage...");

                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);

                this.logger.LogTrace("Blob list downloaded");
            }
            while (continuationToken != null);
            return results;
        }
    }
}