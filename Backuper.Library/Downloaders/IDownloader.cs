namespace Backuper.Library.Downloaders
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface representing a downloader that is able to download
    /// the backup target that shall be backed up in a configured location.
    /// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// Downloads the target file.
        /// </summary>
        /// <returns>Downloaded file reference in <see cref="BackupFile"/>.</returns>
        Task<BackupFile> DownloadAsync();
    }
}