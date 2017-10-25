namespace Backuper.Library.Uploaders
{
    using System.Threading.Tasks;
    using Backuper.Library.Configuration;

    /// <summary>
    /// Service for uploading a backup file to a specified location.
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Uploads a backup file to a specified location.
        /// </summary>
        /// <param name="file">Local file.</param>
        /// <returns>Async task</returns>
        Task UploadAsync(BackupFileWithDate file);

        /// <summary>
        /// Cleans old backups as specified in the target configuration.
        /// </summary>
        /// <param name="target">Target configuration.</param>
        /// <returns>Async task</returns>
        Task CleanOldBackupsAsync(IBackupTarget target);
    }
}