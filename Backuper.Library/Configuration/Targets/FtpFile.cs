namespace Backuper.Library.Configuration.BackupSourceTypes
{
    /// <summary>
    /// Configuration for downloading a specific file.
    /// </summary>
    internal class FtpFile : IBackupTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFile" /> class.
        /// </summary>
        /// <param name="id">ID of the target (for backup naming purposes)</param>
        /// <param name="name">Name of the target</param>
        /// <param name="description">Description of the target</param>
        /// <param name="backup">Backup location</param>
        /// <param name="keep">How many backups must be kept</param>
        /// <param name="ftpUrl">FTP URL with the target file</param>
        /// <param name="ftpUserName">FTP username</param>
        /// <param name="ftpPassword">FTP password</param>
        /// <param name="ftpFilePath">Full path for the file to backup</param>
        public FtpFile(string id, string name, string description, BackupLocation backup, int keep, string ftpUrl, string ftpUserName, string ftpPassword, string ftpFilePath)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Location = backup;
            this.Keep = keep;
            this.FtpUrl = ftpUrl;
            this.FtpUserName = ftpUserName;
            this.FtpPassword = ftpPassword;
            this.FtpFileFullPath = ftpFilePath;
        }

        /// <summary>
        /// Gets a ID used for backup naming purposes
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Gets a configuration name for logging purposes
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a description for documentation purposes
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets a type of this target (FtpFile)
        /// </summary>
        public BackupTarget Target { get => BackupTarget.FtpFile; } 

        /// <summary>
        /// Gets a location, where this target should be backed up
        /// </summary>
        public BackupLocation Location { get; }

        /// <summary>
        /// Gets a maximum number of backups that will be kept in specified location
        /// </summary>
        public int Keep { get; private set; }

        /// <summary>
        /// Gets a URL for the FTP target
        /// </summary>
        public string FtpUrl { get; private set; }

        /// <summary>
        /// Gets a FTP username
        /// </summary>
        public string FtpUserName { get; private set; }

        /// <summary>
        /// Gets a FTP password
        /// </summary>
        public string FtpPassword { get; private set; }

        /// <summary>
        /// Gets a full path to the target file after FTP connection
        /// </summary>
        public string FtpFileFullPath { get; private set; }
    }
}