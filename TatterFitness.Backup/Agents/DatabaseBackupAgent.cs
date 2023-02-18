using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;
using TatterFitness.Backup.Logger;
using TatterFitness.Backup.Utils;

namespace TatterFitness.Backup.Agents
{
    internal class DatabaseBackupAgent
    {
        private readonly IBackupLogger logger;
        private readonly TatterFitConfiguration config;

        public DatabaseBackupAgent(IBackupLogger logger,
            IOptions<TatterFitConfiguration> options)
        {
            this.logger = logger;
            config = options.Value;
        }

        public FileInfo Execute()
        {
            logger.LogActivityStart(nameof(DatabaseBackupAgent));
            var server = new Server();
            var db = default(Database);
            db = server.Databases[config.DbName];
            var backup = new Microsoft.SqlServer.Management.Smo.Backup
            {
                Action = BackupActionType.Database,
                BackupSetDescription = $"Full backup of {config.DbName}",
                Database = config.DbName,
                Incremental = false,
                LogTruncation = BackupTruncateLogType.Truncate
            };

            var backupItem = default(BackupDeviceItem);
            var backupFileName = $"TF_{DateTime.Now:yyyy_MM_dd}.bak";
            backupItem = new BackupDeviceItem(backupFileName, DeviceType.File);

            backup.Devices.Add(backupItem);
            backup.SqlBackup(server);
            backup.Devices.Remove(backupItem);

            logger.LogActivityCompleted(nameof(DatabaseBackupAgent));
            return new FileInfo(Path.Combine(server.BackupDirectory, backupFileName));
        }
    }
}
