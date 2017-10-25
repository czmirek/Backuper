namespace Backuper.Library.Configuration
{
    /// <summary>
    /// Enum containing supported backup targets or sources that must be
    /// backed up in specified destination (or <see cref="BackupLocation"/>
    /// </summary>
    public enum BackupTarget
    {
        /// <summary>
        /// File on an FTP server
        /// </summary>
        FtpFile
    }
}