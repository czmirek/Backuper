namespace Backuper.Library
{
    using System.IO;
    using System.Threading.Tasks;
    using Backuper.Library.Configuration;
    using Backuper.Library.Downloaders;
    using Backuper.Library.Services;
    using Backuper.Library.Uploaders;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Entry point for the backup task.
    /// </summary>
    public class BackuperTask
    {
        /// <summary>
        /// Logging service
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Backup configuration
        /// </summary>
        private readonly BackuperConfiguration config;

        /// <summary>
        /// Factory for a downloader (FTP, HTTP, Azure Storage, etc...)
        /// </summary>
        private readonly DownloaderFactory downloaderFactory;

        /// <summary>
        /// Factory for an uploader (FTP, HTTP, Azure Storage, etc...)
        /// </summary>
        private readonly UploaderFactory uploaderFactory;

        /// <summary>
        /// Service for handling the name of the backup file
        /// </summary>
        private readonly IBackupNamingService namingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackuperTask" /> class.
        /// </summary>
        /// <param name="logger">Logging interface</param>
        /// <param name="backuperConfig">Backup configuration</param>
        /// <param name="downloaderFactory">Factory for a downloader</param>
        /// <param name="uploaderFactory">Factory for an uploader</param>
        /// <param name="namingService">Service for handling the name of the backup file</param>
        public BackuperTask(
            ILogger logger, 
            BackuperConfiguration backuperConfig, 
            DownloaderFactory downloaderFactory,
            UploaderFactory uploaderFactory,
            IBackupNamingService namingService)
        {
            this.logger = logger;
            this.config = backuperConfig;
            this.downloaderFactory = downloaderFactory;
            this.uploaderFactory = uploaderFactory;
            this.namingService = namingService;
        }

        /// <summary>
        /// Returns the task for running the backup.
        /// </summary>
        /// <returns>Backup task</returns>
        public async Task RunAsync()
        {
            // Enumerate backup targets
            foreach (IBackupTarget target in this.config.Targets)
            {
                this.logger.LogTrace($"Processing backup {target.Name}");
                this.logger.LogTrace($"Name: {target.Name}");
                this.logger.LogTrace($"Description: {target.Description}");
                this.logger.LogTrace($"Target: {target.Target}");
                this.logger.LogTrace($"Location: {target.Location}");
                
                IDownloader downloader = this.downloaderFactory.Factory(target);
                IUploader uploader = this.uploaderFactory.Factory(target.Location);

                this.logger.LogInformation($"Removing old backup files in {target.Name}");
                await uploader.CleanOldBackupsAsync(target);

                // Download file
                this.logger.LogInformation($"Downloading target file from {target.Target}");
                BackupFile file = await downloader.DownloadAsync();

                // Rename the file using the naming service
                this.logger.LogTrace($"Renaming {file.FileName}");
                BackupFileWithDate renamedFile = this.namingService.RenameFile(file, target);

                // Upload the file to the configured backup location
                this.logger.LogInformation($"Uploading {file.FileName} to {target.Location}");
                await uploader.UploadAsync(renamedFile);

                // Remove downloaded temporary file
                this.logger.LogTrace($"Deleting local file {file.FullPath}");
                File.Delete(renamedFile.FullPath);

                this.logger.LogInformation("Backup complete");
            }
        }
    }
}