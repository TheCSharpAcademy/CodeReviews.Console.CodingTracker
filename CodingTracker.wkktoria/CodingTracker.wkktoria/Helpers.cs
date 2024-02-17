namespace CodingTracker.wkktoria;

public static class Helpers
{
    public static string PareDateToDbFormat(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static double CalculateDuration(DateTime startTime, DateTime endTime)
    {
        return Math.Round((endTime - startTime).TotalHours, 4);
    }
}