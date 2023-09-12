namespace CodingTracker;

class Logger
{
    static readonly string logFileName = "CodingTracker.log";

    public static void Error(Exception ex)
    {
        var timestamp = DateTime.Now.ToString("u");
        using StreamWriter sw = File.AppendText(logFileName);
        sw.WriteLine($"{timestamp} ERROR {ex.Message}");
        sw.WriteLine(ex.StackTrace);
    }
}