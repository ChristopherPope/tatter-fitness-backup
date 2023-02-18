namespace TatterFitness.Backup.Logger
{
    internal class BackupLogger : IBackupLogger
    {
        //const string SepLine = "==============================";

        public void LogActivityMessage(string message)
        {
            Console.WriteLine($"    {message}");
        }

        public void LogActivityCompleted(string activityName)
        {
            const string SepLine = "---------------------------------";
            Console.WriteLine($@"
{SepLine}
--- {activityName} has completed.
{SepLine}


");
        }

        public void LogActivityStart(string activityName)
        {
            const string SepLine = "+++++++++++++++++++++++++++++++++";
            Console.WriteLine($@"
{SepLine}
+++ {activityName} begins...
{SepLine}");
        }
    }
}
