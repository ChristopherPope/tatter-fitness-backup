using Microsoft.Extensions.Hosting;
using TatterFitness.Backup.Agents;

namespace TatterFitness.Backup
{
    internal class BackupDirector : IHostedService
    {
        private readonly ExportVideosAgent exportVidsAgent;
        private readonly TruncateVideosAgent truncateVidsAgent;
        private readonly DatabaseBackupAgent backupDbAgent;
        private readonly MoveDbBackupToOneDriveAgent moveToOneDriveAgent;
        private readonly ImportVideosAgent importVidsAgent;

        public BackupDirector(ExportVideosAgent exportVidsAgent, 
            TruncateVideosAgent truncateVidsAgent, 
            DatabaseBackupAgent backupDbAgent, 
            MoveDbBackupToOneDriveAgent moveToOneDriveAgent,
            ImportVideosAgent importVidsAgent)
        {
            this.exportVidsAgent = exportVidsAgent;
            this.truncateVidsAgent = truncateVidsAgent;
            this.backupDbAgent = backupDbAgent; 
            this.moveToOneDriveAgent = moveToOneDriveAgent;
            this.importVidsAgent = importVidsAgent;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Backup begins...");
                exportVidsAgent.Execute();
                truncateVidsAgent.Execute();
                var dbBackupFileInfo = backupDbAgent.Execute();
                moveToOneDriveAgent.Execute(dbBackupFileInfo);
                importVidsAgent.Execute();

                Console.WriteLine("Backup successfull.");
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
    }
}
