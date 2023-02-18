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

            var videoIds = uow.Videos.ReadAllIds().ToList();
            for (var i = 0; i < videoIds.Count; i++)
            {
                var videoNum = i + 1;
                if (videoNum % 50 == 0)
                {
                    logger.LogActivityMessage($"Deleting video {videoNum} of {videoIds.Count}");
                }

                var video = uow.Videos.ReadById(videoIds[i]);
                if (video != null)
                {
                    uow.Videos.Delete(video);
                }
            }
            //uow.ExecuteSql("Truncate table Videos");
            uow.Complete();

            logger.LogActivityCompleted(nameof(TruncateVideosAgent));
        }
    }
}
