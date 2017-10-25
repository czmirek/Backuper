namespace Backuper.Library.Configuration
{
    /// <summary>
    /// Interface for a backup location where
    /// backup files are saved.
    /// </summary>
    public interface IBackupLocation
    {
        /// <summary>
        /// Gets the type of location
        /// </summary>
        BackupLocation Location { get; }
    }
}
