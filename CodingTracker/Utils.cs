namespace CodingTracker;

public static class Utils
{
    
    public static TimeSpan Duration(DateTime start, DateTime end)
    {
        if (start.CompareTo(end) > 0) throw new Exception("Start must be earlier than end");
        
        return (end - start);
    }

    public static DateTime GetFirstDayMonth(DateTime time)
    {
        return new DateTime(time.Date.Year, time.Date.Month, 1);
    }

    public static DateTime PreviousMonth(DateTime time)
    {
        return time.AddMonths(-1);
    }
    
    public static DateTime NextMonth(DateTime time)
    {
        return time.AddMonths(1);
    }
}