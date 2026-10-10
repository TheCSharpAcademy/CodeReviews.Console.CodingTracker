using Spectre.Console;

namespace CodingTracker;

public record ColorLevel(double MaxHours, Color Color);

public static class TableVisualisationEngine
{
    public static readonly ColorLevel[] ColorLevels =
    [
        new(1, Color.Grey),
        new(3, Color.IndianRed),
        new(6, Color.Red),
        new(9, Color.Red3),
        new(double.MaxValue, Color.DarkRed)
    ];

    public static Calendar GetCalendar(CodingSession[] sessions)
    {
        if (sessions.Length == 0) throw new Exception("No calendar");

        int year = sessions[0].StartTime.Year;
        int month = sessions[0].StartTime.Month;
        var calendar = new Calendar(year, month);

        foreach (var session in sessions)
        {
            calendar.AddCalendarEvent(year, month, session.StartTime.Day, GetStyle(session.Duration));
        }
        return calendar;
    }
    
    private static Style GetStyle(TimeSpan duration)
    {
        return new Style(ColorLevels.First(level => duration.TotalHours < level.MaxHours).Color);
    }
    
    
}