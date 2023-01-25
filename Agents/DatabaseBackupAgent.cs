using Microsoft.SqlServer.Management.Smo;

namespace TatterFitness.Backup.Agents
{
    internal class DatabaseBackupAgent
    {

        public FileInfo Execute()
        {
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

            return new FileInfo(Path.Combine(server.BackupDirectory, backupFileName));
        }
    }
}
