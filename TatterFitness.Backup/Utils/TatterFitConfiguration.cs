namespace TatterFitness.Backup.Utils
{
    internal class TatterFitConfiguration
    {
        public const string TatterFitConfigKey = "TatterFitConfig";

        public string ExportedVideosDirectory { get; set; } = string.Empty;
        public string OneDriveDbBackupDirectory { get; set; } = string.Empty;
    }
}



