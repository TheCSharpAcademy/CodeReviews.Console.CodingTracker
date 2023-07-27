namespace CodeTracker;

internal class CodingSession
{
    static DateTime startTime;
    static DateTime endTime;
    static TimeSpan timeCoded;

    internal static string StartCodingSession()
    {
        string dateInput;
        Console.WriteLine(DateTime.Now.ToString());
        dateInput = DateTime.Now.ToString();
        startTime = DateTime.Now;

        return dateInput;
    }

    internal static string EndCodingSession()
    {
        Console.WriteLine("Press enter to use the current time as the end time");
        string dateInput;
        Console.ReadLine();

        Console.WriteLine(DateTime.Now.ToString());
        endTime = DateTime.Now;
        CalculateDuration();
        dateInput = DateTime.Now.ToString();

        return dateInput;
    }

    static void CalculateDuration()
    {
        timeCoded = endTime - startTime;
        Console.WriteLine($"Time Coded {(int)timeCoded.TotalMinutes}.");
    }

    public class CodeingTime
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan TimeCoded { get; set; }
    }
}
