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

## Configuration

See `Backuper.Console/backuper-config.json`

```javascript
// ----------------------------------------
// Configuration file for Backuper.
// ----------------------------------------
{
  // List of backup "targets" e.g. what should saved as a backup
  "Targets": [
    {
      // ID of target, this should be a string with characters
      // supported as a file name or as an Azure Storage Blob
      "ID": "MyFile",

      // Human readable target, only for logging purposes
      "Name": "My project SQLite database",

      // Human readable description of the target, only for logging and orientation purposes
      "Description": "Database of MyProject",

      // Type of the target. Only FtpFile is supported right now.
      "Type": "FtpFile",

      // Type of the backup location e.g. where is the backup target going to be saved.
      "Location": {

        // ID of the backup location (see below)
        "ID": "AzureStorageBlob",

        // How many backup files should be kept
        "Keep": 5
      },

      // FTP configuration (will be changed to "TargetConfiguration" in future)
      "SourceConfiguration": {

        // FTP server URL
        "Url": "ftp://myproject.net",

        // FTP UserName
        "UserName": "ftp-username",

        // FTP Password
        "Password": "ftp-password",

        // Full path of the file
        "FullPath": "/MyProjectSQLite.db"
      }
    }
  ],

  // Where should we backup. Configurations for possible backup destinations.
  // Structure of this configuration will change in future.
  "Locations": [

    // Azure Storage Blob
    {
      // ID of backup destination
      "ID": "AzureStorageBlob",

      // Human readable description for orientation
      "Description": "My azure storage account",

      // Azure Storage connection configuration
      "Configuration": {

        // Account name
        "AccountName": "azureaccountname",

        // Account key
        "AccountKey": "azureaccountkey",

        // Azure Blob storage container name
        "ContainerName": "containername"
      }
    }
  ]
}
```
