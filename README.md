# Backuper
Simple idempotent console app for creating backups.
---

Backuper downloads a file from a configured "target" and then uploads it to a configured "location".
Downloading is done via "Downloaders" and uploading via "Uploaders".

Currently, there is only one downloader and one uploader:
- **FtpFileDownloader**: downloads a specified file from a secured FTP server
- **AzureStorageBlobUploader**: uploads a file as a backup to specified Azure Storage Blob container

Configuration is made in `backup-config.json`.

Backuper supports maintaining a number of backups in any Uploader, see the `Keep` parameter.
Downloaded files are renamed to a format containing a backup date and time. Backuper works idempotently
and automatically removes old backups.

Backuper uses [NLog](http://nlog-project.org/) to automatically log its progress to files. 
Fatal errors are emailed. See `NLog.config`.