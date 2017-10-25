namespace Backuper.Library.Services
{
    using Backuper.Library.Configuration;

    /// <summary>
    /// Service for managing the name of the backup file.
    /// </summary>
    public interface IBackupNamingService
    {
        /// <summary>
        /// Renames the downloaded target file .
        /// </summary>
        /// <param name="file">Local file reference.</param>
        /// <param name="target">Backup target configuration</param>
        /// <returns>Renamed local backup file reference with date.</returns>
        BackupFileWithDate RenameFile(BackupFile file, IBackupTarget target);

        /// <summary>
        /// Reads a date time from the backup file name.
        /// </summary>
        /// <param name="fileName">Filename of the backup.</param>
        /// <returns>Backup reference with date.</returns>
        BackupFileWithDate ReadDateTime(string fileName);

        /// <summary>
        /// Returns whether a specified file is a valid backup file.
        /// </summary>
        /// <param name="fileName">Filename of the backup.</param>
        /// <param name="id">Id of the backup target.</param>
        /// <returns>True if it's a valid backup.</returns>
        bool IsNamedBackup(string fileName, string id);
    }
}
