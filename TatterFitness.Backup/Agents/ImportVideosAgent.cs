using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using TatterFitness.Backup.Utils;
using TatterFitness.Bll.Interfaces.Services;
using TatterFitness.Dal.Entities;

namespace TatterFitness.Backup.Agents
{
    internal class ImportVideosAgent
    {
        private readonly IVideoService videoSvc;
        private readonly TatterFitConfiguration config;
        static string[] months = {
            "xxx",
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"
        };

        public ImportVideosAgent(IVideoService videoSvc, IOptions<TatterFitConfiguration> options)
        {
            this.videoSvc = videoSvc;
            config = options.Value;
        }

        public void Execute()
        {
            var videoFilePaths = Directory.GetFiles(config.ExportedVideosDirectory);
            var videoNum = 1;
            foreach (var videoFilePath in videoFilePaths)
            {
                Console.WriteLine($"Importing video {videoNum++} of {videoFilePaths.Length}...");
                var workoutExerciseId = GetWorkoutExerciseId(videoFilePath);
                var videoData = File.ReadAllBytes(videoFilePath);
                var video = new VideoEntity
                {
                    WorkoutExerciseId = workoutExerciseId,
                    VideoData = videoData,
                    Hash = MD5.Create().ComputeHash(videoData)
                };

                if (videoSvc.DoesVideoExist(video.Hash))
                {
                    continue;
                }

                videoSvc.Create(video);
            }
        }

        private int GetWorkoutExerciseId(string videoFilePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(videoFilePath);
            var idx = fileName.IndexOf('-');
            var weIdText = fileName.Substring(0, idx);

            return int.Parse(weIdText);
        }

        private DateTime ParseCreatedText(string createdText)
        {
            var monText = createdText.Substring(4, 3);
            var month = Array.IndexOf(months, monText);

            var dayText = createdText.Substring(8, 2);
            var day = int.Parse(dayText);

            var yearText = createdText.Substring(20, 4);
            var year = int.Parse(yearText);

            return new DateTime(year, month, day);
        }

    }
}
