namespace TatterFitness.Backup.Logger
{
    internal class BackupLogger : IBackupLogger
    {
        const string SepLine = "==============================";

        public void LogActivityMessage(string message)
        {
            Console.Write(message);
        }

        public void LogActivityCompleted(string activityName)
        {
            Console.WriteLine($@"
{SepLine}
=== {activityName} has completed.
{SepLine}");
        }

        public void LogActivityStart(string activityName)
        {
            Console.WriteLine($@"
{SepLine}
===
=== {activityName} begins...
===
{SepLine}");
        }
    }
}
