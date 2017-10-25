namespace Backuper.Library.Downloaders
{
    using System;
    using Backuper.Library.Configuration;
    using Backuper.Library.Configuration.BackupSourceTypes;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Factory for resolving the type of a downloader
    /// </summary>
    public class DownloaderFactory
    {
        /// <summary>
        /// Logger interface
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloaderFactory"/> class.
        /// </summary>
        /// <param name="logger">Logger interface</param>
        public DownloaderFactory(ILogger logger)
        {
            Validate.NotNull(logger, nameof(logger));

            this.logger = logger;
        }

        /// <summary>
        /// Creates an instance of a specific downloader depending
        /// on the target configuration
        /// </summary>
        /// <param name="targetConfig">Target configuration</param>
        /// <returns>Downloader instance</returns>
        public IDownloader Factory(IBackupTarget targetConfig)
        {
            Validate.NotNull(targetConfig, nameof(targetConfig));

            switch (targetConfig.Target)
            {
                case BackupTarget.FtpFile:
                    return new FtpFileDownloader(targetConfig as FtpFile, this.logger);
                default:
                    throw new NotImplementedException($"Unknown source type {targetConfig.Target}");
            }
        }
    }
}