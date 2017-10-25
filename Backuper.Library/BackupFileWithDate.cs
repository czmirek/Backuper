namespace Backuper.Library
{
    using System;

    /// <summary>
    /// Backup file reference with date
    /// from the naming service.
    /// </summary>
    public sealed class BackupFileWithDate : BackupFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFileWithDate"/> class.
        /// </summary>
        /// <param name="fileName">File name or full path</param>
        /// <param name="backupDate">DateTime of the backup.</param>
        public BackupFileWithDate(string fileName, DateTime backupDate) : base(fileName)
        {
            this.BackupDate = backupDate;
        }

        /// <summary>
        /// Gets the date and time of the backup.
        /// </summary>
        public DateTime BackupDate { get; private set; }
    }
}
