namespace Backuper.Library.Configuration
{
    /// <summary>
    /// Interface for a configuration of a file that
    /// must be backed up.
    /// </summary>
    public interface IBackupTarget
    {
        /// <summary>
        /// Gets an ID used for naming purposes of the backup file. This should be unique.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Gets a human readable name of the backup target
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a description of the backup target
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a number of backups that must be kept in the backup location
        /// </summary>
        int Keep { get; }

        /// <summary>
        /// Gets the type of the target (FTP, HTTP, Azure Storage, etc.)
        /// </summary>
        BackupTarget Target { get; }

        /// <summary>
        /// Gets the type of the target (FTP, HTTP, Azure Storage, etc.)
        /// </summary>
        BackupLocation Location { get; }
    }
}