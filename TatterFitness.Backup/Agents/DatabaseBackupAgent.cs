using Microsoft.SqlServer.Management.Smo;
using TatterFitness.Backup.Logger;

namespace TatterFitness.Backup.Agents
{
    internal class DatabaseBackupAgent
    {
        private readonly IBackupLogger logger;

        public DatabaseBackupAgent(IBackupLogger logger)
        {
            this.logger = logger;
        }

        public FileInfo Execute()
        {
            logger.LogActivityStart(nameof(DatabaseBackupAgent));
            var dbName = "TATTER-FITNESS";
            var server = new Server();
            var db = default(Database);
            db = server.Databases[dbName];
            var backup = new Microsoft.SqlServer.Management.Smo.Backup();

            backup.Action = BackupActionType.Database;
            backup.BackupSetDescription = "Full backup of TATTER-FITNESS";
            backup.Database = dbName;
            backup.Incremental = false;
            backup.LogTruncation = BackupTruncateLogType.Truncate;

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
