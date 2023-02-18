using TatterFitness.Backup.Logger;
using TatterFitness.Dal.Interfaces.Persistance;

namespace TatterFitness.Backup.Agents
{
    internal class TruncateVideosAgent
    {
        private readonly IUnitOfWork uow;
        private readonly IBackupLogger logger;

        public TruncateVideosAgent(IUnitOfWork uow, IBackupLogger logger)
        {
            this.logger = logger;
            this.uow = uow;
        }

        public void Execute()
        {
            logger.LogActivityStart(nameof(TruncateVideosAgent));

            //uow.ExecuteSql("Truncate table Videos");
            //uow.Complete();

            logger.LogActivityCompleted(nameof(TruncateVideosAgent));
        }
    }
}
