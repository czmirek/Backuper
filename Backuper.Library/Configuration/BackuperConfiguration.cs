namespace Backuper.Library.Configuration
{
    using System;
    using System.Collections.Generic;
    using Backuper.Library.Configuration.Locations;
    using BackupSourceTypes;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Configuration of the backup task
    /// </summary>
    public class BackuperConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackuperConfiguration" /> class.
        /// </summary>
        /// <param name="config">Configuration of the backuper</param>
        public BackuperConfiguration(IConfiguration config)
        {
            List<IBackupLocation> locations = new List<IBackupLocation>();
            List<IBackupTarget> targets = new List<IBackupTarget>();

            IEnumerable<IConfigurationSection> locationsConf = config.GetSection("Locations").GetChildren();
            IEnumerable<IConfigurationSection> targetsConf = config.GetSection("Targets").GetChildren();

            // Load and enumerate locations
            foreach (IConfigurationSection locationConf in locationsConf)
            {
                BackupLocation backupLocation = (BackupLocation)Enum.Parse(typeof(BackupLocation), locationConf["ID"]);
                switch (backupLocation)
                {
                    case BackupLocation.AzureStorageBlob:

                        locations.Add(new AzureStorageBlob(
                           accountName: locationConf["Configuration:AccountName"],
                           accountKey: locationConf["Configuration:AccountKey"],
                           containerName: locationConf["Configuration:ContainerName"]));

                        break;
                    default:
                        throw new NotImplementedException($"Backup location {backupLocation} not implemented");
                }
            }

            // Load and enumerate targets
            foreach (IConfigurationSection targetConf in targetsConf)
            {
                BackupTarget targetType = (BackupTarget)Enum.Parse(typeof(BackupTarget), targetConf["Type"]);

                IConfigurationSection locationConf = targetConf.GetSection("Location");
                BackupLocation backupLocation = (BackupLocation)Enum.Parse(typeof(BackupLocation), locationConf["ID"]);
                int keep = int.Parse(locationConf["Keep"]);

                switch (targetType)
                {
                    case BackupTarget.FtpFile:
                        targets.Add(new FtpFile(
                            id: targetConf["ID"],
                            name: targetConf["Name"],
                            description: targetConf["Description"],
                            backup: backupLocation,
                            keep: keep,
                            ftpUrl: targetConf["SourceConfiguration:Url"],
                            ftpUserName: targetConf["SourceConfiguration:UserName"],
                            ftpPassword: targetConf["SourceConfiguration:Password"],
                            ftpFilePath: targetConf["SourceConfiguration:FullPath"]));
                        break;
                    default:
                        throw new NotImplementedException($"Target type {targetType} not implemented");
                }
            }

            this.Locations = locations;
            this.Targets = targets;
        }

        /// <summary>
        /// Gets a list of backup locations (and their configurations) where backups can be stored
        /// </summary>
        public IEnumerable<IBackupLocation> Locations { get; private set; }

        /// <summary>
        /// Gets a list of desired targets that need to be backed up
        /// </summary>
        public IEnumerable<IBackupTarget> Targets { get; private set; }
    }
}
