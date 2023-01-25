using TatterFitness.Dal.Interfaces.Persistance;

namespace TatterFitness.Backup.Agents
{
    internal class TruncateVideosAgent
    {
        private readonly IUnitOfWork uow;

        public TruncateVideosAgent(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void Execute()
        {
            uow.ExecuteSql("Truncate table Videos");
            uow.Complete();
        }
    }
}
