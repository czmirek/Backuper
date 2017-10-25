namespace Backuper.Library
{
    using System.IO;

    /// <summary>
    /// Reference to the backup file.
    /// </summary>
    public class BackupFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFile"/> class.
        /// </summary>
        /// <param name="fullPath">Full path to the file.</param>
        public BackupFile(string fullPath)
        {
            Validate.NotNull(fullPath, nameof(fullPath));

            this.FullPath = fullPath;
            this.Directory = Path.GetDirectoryName(fullPath);
            this.FileName = Path.GetFileName(fullPath);
            this.FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
            this.Extension = Path.GetExtension(fullPath);
        }

        /// <summary>
        /// Gets a full path to the file
        /// </summary>
        public string FullPath { get; private set; }

        /// <summary>
        /// Gets a directory of the file
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// Gets the file name only
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the file name without the extension
        /// </summary>
        public string FileNameWithoutExtension { get; private set; }

        /// <summary>
        /// Gets the extension of the file
        /// </summary>
        public string Extension { get; private set; }
    }
}