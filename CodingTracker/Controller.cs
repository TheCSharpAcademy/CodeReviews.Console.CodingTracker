namespace CodingTracker;

public static class Controller
{
    private static DateTime _monthToShow = GetFirstDayMonth(DateTime.Now);
    
    public static async Task Start()
    {
        bool endApp = false;

        UserInterface.Title();
        do
        {
            var sessions = await DataBaseInterface.ViewCodingSessions(_monthToShow, NextMonth(_monthToShow));
            var sessionsPerDay = AmountCodingPerDay(sessions);
            
            TableVisualisationEngine.ShowCalendarWithCaption(sessionsPerDay);
            
        } while (!endApp);
    }

    public static int Duration(DateTime start, DateTime end)
    {
        if (start.CompareTo(end) >= 0) throw new Exception("Start must be earlier than end");
        
        return (end - start).Seconds;
    }

    public static DateTime GetFirstDayMonth(DateTime time)
    {
        return new DateTime(time.Date.Year, time.Date.Month, 1);
    }

    public static DateTime NextMonth(DateTime time)
    {
        return time.AddMonths(1);
    }
    
    /// Aggregates coding sessions by calendar day, returning one session per day with summed durations
    /// We take this postulate: Sessions that happens past midnight are attributed entirely to their start date
    public static List<CodingSession> AmountCodingPerDay(List<CodingSession> codingSessions)
    {
        return codingSessions
            .GroupBy(codingSession => codingSession.StartTime.Date) // Only take the day of the startTime
            .Select(codingSession =>
            new CodingSession(
                codingSession.Key,
                codingSession.Key.AddDays(1),
                TimeSpan.FromTicks(codingSession.Sum(s => s.Duration.Ticks))))
            .ToList();
    }
    
}