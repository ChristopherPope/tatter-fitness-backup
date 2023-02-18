using Microsoft.Extensions.Options;
using TatterFitness.Backup.Logger;
using TatterFitness.Backup.Utils;

namespace TatterFitness.Backup.Agents
{
    internal class MoveDbBackupToOneDriveAgent
    {
        private readonly TatterFitConfiguration config;
        private readonly IBackupLogger logger;

        public MoveDbBackupToOneDriveAgent(IOptions<TatterFitConfiguration> options, IBackupLogger logger)
        {
            this.logger = logger;
            config = options.Value;
        }

        public void Execute(FileInfo dbBackupFileInfo)
        {
            logger.LogActivityStart(nameof(MoveDbBackupToOneDriveAgent));

            File.Move(dbBackupFileInfo.FullName, Path.Combine(config.OneDriveDbBackupDirectory, dbBackupFileInfo.Name), true);

            logger.LogActivityCompleted(nameof(MoveDbBackupToOneDriveAgent));
        }
    }
}
