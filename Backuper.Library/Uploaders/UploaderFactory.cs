namespace Backuper.Library.Uploaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Backuper.Library.Configuration;
    using Backuper.Library.Configuration.Locations;
    using Backuper.Library.Services;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Factory for initializing an uploader instance.
    /// </summary>
    public sealed class UploaderFactory
    {
        /// <summary>
        /// Configuration for all <see cref="BackupLocation"/> types.
        /// </summary>
        private readonly IDictionary<BackupLocation, IBackupLocation> backupConfig;

        /// <summary>
        /// Service for naming the backup file.
        /// </summary>
        private readonly IBackupNamingService namingService;

        /// <summary>
        /// Logging service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploaderFactory"/> class.
        /// </summary>
        /// <param name="backuperConfig">Backuper configuration</param>
        /// <param name="namingService">Backup file naming service</param>
        /// <param name="logger">Logging service</param>
        public UploaderFactory(
            BackuperConfiguration backuperConfig, 
            IBackupNamingService namingService,
            ILogger logger)
        {
            Validate.NotNull(backuperConfig, nameof(backuperConfig));
            Validate.NotNull(namingService, nameof(namingService));
            Validate.NotNull(logger, nameof(logger));

            this.backupConfig = backuperConfig.Locations.ToDictionary(x => x.Location, x => x);
            this.namingService = namingService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new <see cref="IUploader"/> for a given <see cref="BackupLocation"/>.
        /// </summary>
        /// <param name="backupType">Backup location type</param>
        /// <returns>Instance of a specific uploader.</returns>
        public IUploader Factory(BackupLocation backupType)
        {
            switch (backupType)
            {
                case BackupLocation.AzureStorageBlob:
                    return new AzureStorageBlobUploader(this.backupConfig[backupType] as AzureStorageBlob, this.namingService, this.logger);
                default:
                    throw new NotImplementedException($"Unknown backup type {backupType}");
            }
        }
    }
}
