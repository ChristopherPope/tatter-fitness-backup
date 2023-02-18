using Microsoft.Extensions.Configuration;
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
        private readonly TatterFitConfiguration tatterFitConfig;
        private readonly IConfiguration config;

        public BackupDirector(ExportVideosAgent exportVidsAgent,
            TruncateVideosAgent truncateVidsAgent,
            DatabaseBackupAgent backupDbAgent,
            MoveDbBackupToOneDriveAgent moveToOneDriveAgent,
            ImportVideosAgent importVidsAgent,
            IBackupLogger logger,
            IConfiguration configer,
            IOptions<TatterFitConfiguration> options)
        {
            this.exportVidsAgent = exportVidsAgent;
            this.truncateVidsAgent = truncateVidsAgent;
            this.backupDbAgent = backupDbAgent;
            this.moveToOneDriveAgent = moveToOneDriveAgent;
            this.importVidsAgent = importVidsAgent;
            this.logger = logger;
            tatterFitConfig = options.Value;
            this.config = configer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogActivityStart(nameof(BackupDirector));
                LogConfig();
                CreateConfigDirectories();

                exportVidsAgent.Execute();
                truncateVidsAgent.Execute();
                var dbBackupFileInfo = backupDbAgent.Execute();
                moveToOneDriveAgent.Execute(dbBackupFileInfo);
                importVidsAgent.Execute();

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

        private void CreateConfigDirectories()
        {
#if DEBUG
            Directory.CreateDirectory(config.ExportedVideosDirectory);
            Directory.CreateDirectory(config.OneDriveDbBackupDirectory);
#endif
        }

        private void LogConfig()
        {
            logger.LogActivityMessage($@"
ExportedVideosDirectory.....{tatterFitConfig.ExportedVideosDirectory}
OneDriveDbBackupDirectory...{tatterFitConfig.OneDriveDbBackupDirectory}
DBName......................{tatterFitConfig.DbName}
DBCS........................{config.GetConnectionString("TatterFitnessDb")}
");

            logger.LogActivityMessage("If this is correct, press ENTER to continue...");
            _ = Console.ReadLine();
        }
    }
}
