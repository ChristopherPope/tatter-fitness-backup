using Microsoft.Extensions.Options;
using TatterFitness.Backup.Utils;

namespace TatterFitness.Backup.Agents
{
    internal class MoveDbBackupToOneDriveAgent
    {
        private readonly TatterFitConfiguration config;

        public MoveDbBackupToOneDriveAgent(IOptions<TatterFitConfiguration> options)
        {
            config = options.Value;
        }

        public void Execute(FileInfo dbBackupFileInfo)
        {
            Console.WriteLine("Moving db backup to One Drive...");
            File.Move(dbBackupFileInfo.FullName, Path.Combine(config.OneDriveDbBackupDirectory, dbBackupFileInfo.Name));
        }
    }
}
