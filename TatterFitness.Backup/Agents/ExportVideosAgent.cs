using Microsoft.Extensions.Options;
using TatterFitness.Backup.Logger;
using TatterFitness.Backup.Utils;
using TatterFitness.Dal.Interfaces.Persistance;

namespace TatterFitness.Backup.Agents
{
    internal class ExportVideosAgent
    {
        private readonly IUnitOfWork uow;
        private readonly TatterFitConfiguration config;
        private readonly IBackupLogger logger;

        public ExportVideosAgent(IUnitOfWork uow, IOptions<TatterFitConfiguration> options, IBackupLogger logger)
        {
            this.logger = logger;
            this.uow = uow;
            config = options.Value;
        }

        public void Execute()
        {
            logger.LogActivityStart(nameof(ExportVideosAgent));

            var videoIds = uow.Videos.ReadAllIds().ToList();
            var nextVideoFileNum = new Dictionary<int, int>();
            for (var videoNum = 1; videoNum <= videoIds.Count; videoNum++)
            {
                if (videoNum % 50 == 0)
                {
                    logger.LogActivityMessage($"Exporting video {videoNum} of {videoIds.Count}...");
                }

                var videoId = videoIds[videoNum - 1];
                var video = uow.Videos.ReadById(videoId);
                if (video == null)
                {
                    throw new Exception($"Unable to read video {videoId}");
                }

                if (!nextVideoFileNum.ContainsKey(video.WorkoutExerciseId))
                {
                    nextVideoFileNum.Add(video.WorkoutExerciseId, 1);
                }

                var videoFileNum = nextVideoFileNum[video.WorkoutExerciseId]++;
                if (!DoesVideoFileExist(video.WorkoutExerciseId, videoFileNum))
                {
                    ExportVideo(video.WorkoutExerciseId, videoFileNum, video.VideoData);
                }
            }

            logger.LogActivityCompleted(nameof(ExportVideosAgent));
        }

        private string MakeVideoPath(int workoutExerciseId, int exerciseVideoNum)
        {
            return Path.Combine(config.ExportedVideosDirectory, $"{workoutExerciseId}-{exerciseVideoNum}.MP4");
        }

        private void ExportVideo(int workoutExerciseId, int exerciseVideoNum, byte[] videoData)
        {
            var videoPath = MakeVideoPath(workoutExerciseId, exerciseVideoNum);
            File.WriteAllBytes(videoPath, videoData);
        }

        private bool DoesVideoFileExist(int workoutExerciseId, int exerciseVideoNum)
        {
            var videoPath = MakeVideoPath(workoutExerciseId, exerciseVideoNum);
            return File.Exists(videoPath);
        }
    }
}
