namespace TatterFitness.Backup.Logger
{
    internal interface IBackupLogger
    {
        void LogActivityStart(string agentName);
        void LogActivityMessage(string message);
        void LogActivityCompleted(string agentName);
    }
}
