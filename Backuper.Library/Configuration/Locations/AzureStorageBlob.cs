namespace Backuper.Library.Configuration.Locations
{
    using Backuper.Library.Configuration;

    /// <summary>
    /// Configuration for Azure Storage Blob with the access key
    /// for specific container.
    /// </summary>
    internal class AzureStorageBlob : IBackupLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageBlob" /> class.
        /// </summary>
        /// <param name="accountName">Account name</param>
        /// <param name="accountKey">Account key</param>
        /// <param name="containerName">Container name</param>
        public AzureStorageBlob(string accountName, string accountKey, string containerName)
        {
            Validate.NotNull(accountName, nameof(accountName));
            Validate.NotNull(accountKey, nameof(accountKey));
            Validate.NotNull(containerName, nameof(containerName));

            this.AccountName = accountName;
            this.AccountKey = accountKey;
            this.ContainerName = containerName;
        }

        /// <summary>
        /// Gets an account name
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// Gets an account key
        /// </summary>
        public string AccountKey { get; private set; }

        /// <summary>
        /// Backup type
        /// </summary>
        public BackupLocation Location => BackupLocation.AzureStorageBlob;

        /// <summary>
        /// Gets a container, where backups are stored
        /// </summary>
        public string ContainerName { get; private set; }
    }
}
