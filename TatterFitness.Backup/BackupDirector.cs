using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TatterFitness.Backup.Agents;
using TatterFitness.Backup.Logger;
using TatterFitness.Backup.Utils;

namespace TatterFitness.Backup
{
    internal class BackupDirector : IHostedService
    {
        private readonly ExportVideosAgent exportVidsAgent;
        private readonly TruncateVideosAgent truncateVidsAgent;
        private readonly DatabaseBackupAgent backupDbAgent;
        private readonly MoveDbBackupToOneDriveAgent moveToOneDriveAgent;
        private readonly ImportVideosAgent importVidsAgent;
        private readonly IBackupLogger logger;
        private readonly TatterFitConfiguration config;

        public BackupDirector(ExportVideosAgent exportVidsAgent,
            TruncateVideosAgent truncateVidsAgent,
            DatabaseBackupAgent backupDbAgent,
            MoveDbBackupToOneDriveAgent moveToOneDriveAgent,
            ImportVideosAgent importVidsAgent,
            IBackupLogger logger,
            IOptions<TatterFitConfiguration> options)
        {
            this.exportVidsAgent = exportVidsAgent;
            this.truncateVidsAgent = truncateVidsAgent;
            this.backupDbAgent = backupDbAgent;
            this.moveToOneDriveAgent = moveToOneDriveAgent;
            this.importVidsAgent = importVidsAgent;
            this.logger = logger;
            config = options.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogActivityStart(nameof(BackupDirector));

                //exportVidsAgent.Execute();
                //truncateVidsAgent.Execute();
                //var dbBackupFileInfo = backupDbAgent.Execute();
                //moveToOneDriveAgent.Execute(dbBackupFileInfo);
                //importVidsAgent.Execute();

                logger.LogActivityCompleted(nameof(BackupDirector));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to perform backup - {ex.Message}");
            }

            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Press ENTER to exit...");
            Console.ReadLine();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void LogConfig()
        {
            logger.LogActivityMessage($@"
ExportedVideosDirectory.....{config.ExportedVideosDirectory}
OneDriveDbBackupDirectory...{config.OneDriveDbBackupDirectory}
DBName......................{config.DbName}
");
        }
    }
}
