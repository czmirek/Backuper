namespace Backuper.Library.Services
{
    using System;
    using System.Globalization;
    using System.IO;
    using Backuper.Code.DateTimeResolution;
    using Backuper.Library.Configuration;

    /// <summary>
    /// Naming service that names backup files in the format
    /// {ID}.{Date}.bak where {ID} is the ID of the backup target
    /// and {Date} is a date and time of the back up in a format
    /// specified in <see cref="DateTimeFormat"/>.
    /// </summary>
    public class DefaultNamingService : IBackupNamingService
    {
        /// <summary>
        /// Filename part containing the date and time. 
        /// Should be datetime sortable and should contain allowed
        /// characters.
        /// </summary>
        public const string DateTimeFormat = "yyyy_MM_dd_HH_mm_ss_ffff";

        /// <summary>
        /// Extension of the backup filename.
        /// </summary>
        public const string BackupFileExtension = "bak";

        /// <summary>
        /// Date and time service
        /// </summary>
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNamingService"/> class.
        /// </summary>
        /// <param name="dateTimeProvider">DateTime service</param>
        public DefaultNamingService(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Reads a date time from the backup file name.
        /// </summary>
        /// <param name="fileName">Filename of the backup.</param>
        /// <returns>Backup reference with date.</returns>
        public BackupFileWithDate ReadDateTime(string fileName)
        {
            Validate.NotNull(fileName, nameof(fileName));

            string[] parts = fileName.Split('.');
            string dateTimePart = parts[1];
            DateTime parsedDateTime = DateTime.ParseExact(dateTimePart, DateTimeFormat, CultureInfo.InvariantCulture);
            return new BackupFileWithDate(fileName, parsedDateTime);
        }

        /// <summary>
        /// Renames the downloaded target file .
        /// </summary>
        /// <param name="fileName">Local file reference.</param>
        /// <param name="target">Backup target configuration</param>
        /// <returns>Renamed local backup file reference with date.</returns>
        public BackupFileWithDate RenameFile(BackupFile fileName, IBackupTarget target)
        {
            Validate.NotNull(fileName, nameof(fileName));
            Validate.NotNull(target, nameof(target));

            DateTime backupDateTime = this.dateTimeProvider.Now;
            string dateTimePart = backupDateTime.ToString(DateTimeFormat);
            string newFileFullName = $"{target.ID}.{dateTimePart}.{BackupFileExtension}";
            string newFileFullPath = Path.Combine(fileName.Directory, newFileFullName);
            File.Move(fileName.FullPath, newFileFullPath);

            return new BackupFileWithDate(newFileFullPath, backupDateTime);
        }

        /// <summary>
        /// Returns whether a specified file is a valid backup file.
        /// </summary>
        /// <param name="fileName">Filename of the backup.</param>
        /// <param name="id">Id of the backup target.</param>
        /// <returns>True if it's a valid backup.</returns>
        public bool IsNamedBackup(string fileName, string id)
        {
            string[] parts = fileName.Split('.');
            return parts[0] == id && parts[parts.Length - 1] == BackupFileExtension;
        }
    }
}