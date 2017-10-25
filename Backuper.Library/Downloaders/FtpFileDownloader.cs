namespace Backuper.Library.Downloaders
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Backuper.Library.Configuration.BackupSourceTypes;
    using FluentFTP;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Downloads a given file from an FTP server using secure connection 
    /// but always accepting the server certificate.
    /// </summary>
    internal class FtpFileDownloader : IDownloader
    {
        /// <summary>
        /// Target configuration
        /// </summary>
        private readonly FtpFile ftpConfig;

        /// <summary>
        /// Logger interface
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileDownloader"/> class.
        /// </summary>
        /// <param name="ftpConfig">FTP configuration</param>
        /// <param name="logger">Logger interfae</param>
        public FtpFileDownloader(FtpFile ftpConfig, ILogger logger)
        {
            this.ftpConfig = ftpConfig;
            this.logger = logger;
        }

        /// <summary>
        /// Downloads the FTP file.
        /// </summary>
        /// <returns>Downloaded file reference in <see cref="BackupFile"/>.</returns>
        public async Task<BackupFile> DownloadAsync()
        {
            this.logger.LogTrace("Creating FTP client...");

            FtpClient ftpClient = new FtpClient(this.ftpConfig.FtpUrl);
            ftpClient.Credentials = new NetworkCredential(this.ftpConfig.FtpUserName, this.ftpConfig.FtpPassword);
            ftpClient.EncryptionMode = FtpEncryptionMode.Explicit;
            ftpClient.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
            ftpClient.ValidateCertificate += this.FtpClient_ValidateCertificate;
            ftpClient.DataConnectionType = FtpDataConnectionType.PASV;

            this.logger.LogTrace($"Opening connection to {ftpConfig.FtpUrl}");
            await ftpClient.ConnectAsync();
            
            string tempFile = Path.GetTempFileName();
            this.logger.LogTrace($"Downloading {Path.GetFileName(ftpConfig.FtpFileFullPath)} to {tempFile}");

            await ftpClient.DownloadFileAsync(tempFile, this.ftpConfig.FtpFileFullPath, true);
            this.logger.LogTrace("File successfully downloaded");

            return new BackupFile(tempFile);
        }

        /// <summary>
        /// Always validates an SSL certificate
        /// </summary>
        /// <param name="control">FTP client instance</param>
        /// <param name="e">Certificate validation arguments</param>
        private void FtpClient_ValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            e.Accept = true;
        }
    }
}