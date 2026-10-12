using Spectre.Console;

namespace CodingTracker;

public record ColorLevel(double MaxHours, Color Color);

public static class TableVisualisationEngine
{
    private static readonly ColorLevel[] ColorLevels =
    [
        new(1, Color.Grey),
        new(3, Color.IndianRed),
        new(6, Color.Red),
        new(9, Color.Red3),
        new(double.MaxValue, Color.DarkRed)
    ];
    
    public static void ShowCalendarWithCaption(List<CodingSession> sessions)
    {
        var caption = GetCaptionForCalendar();
        var alignedCaption = Align.Left(caption, VerticalAlignment.Middle).Height(10);
        
        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow(GetCalendar(sessions), alignedCaption);
        AnsiConsole.Live(grid).Start(ctx => ctx.Refresh());
    }

    private static Calendar GetCalendar(List<CodingSession> sessions)
    {
        if (sessions.Count == 0) return new Calendar(Controller.MonthToShow.Year, Controller.MonthToShow.Month);

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
    
    private static Grid GetCaptionForCalendar()
    {
        var captionTable = new Grid()
            .AddColumn()
            .AddColumn();

        double previousMax = 0;

        foreach (var colorLevel in ColorLevels)
        {
             string rangeString = $"{previousMax}h " + 
                                 (colorLevel.MaxHours > 24 ? $"or more" : $"to {colorLevel.MaxHours}h");
             string coloredShape = $"[{colorLevel.Color}]██[/]";
            captionTable.AddRow(coloredShape, rangeString);
            
            previousMax = colorLevel.MaxHours;
        }

        return captionTable;
    }
}