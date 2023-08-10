using ConsoleTableExt;

using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.UserInterface;

internal static class TableVisualizationEngine
{
    private static string _dateTimeFormat = "yyyy-MM-dd HH:mm";
    private static string _durationFormat = "h'h 'm'm'";

    public static void ShowTable(List<CodingSession> sessions)
    {
        var tableData = FormatTableData(sessions);
        BuildTable(tableData, new string[] { "Id", "Start Time", "End Time", "Duration" });
    }

    public static void ShowTable(List<CodingGoal> goals)
    {
        var tableData = FormatTableData(goals);
        BuildTable(tableData, new string[] { "Start Time", "Deadline", "Goal (Hours)", "Time Completed", "Time per day to meet goal" });
    }

    public static void ShowTable(List<CodingStatistic> statistics)
    {
        var tableData = FormatTableData(statistics);
        BuildTable(tableData, new string[] { "Period", "Total", "Average per session" });
    }

    private static void BuildTable(List<List<object>> tableData, params string[] columnNames)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithColumn(columnNames)
            .ExportAndWriteLine();
    }

    private static List<List<object>> FormatTableData(List<CodingSession> sessions)
    {
        return sessions.Select(s => new List<object>
            {
                s.Id,
                s.StartTime.ToString(_dateTimeFormat),
                s.EndTime.ToString(_dateTimeFormat),
                s.Duration.ToString(_durationFormat)
            }).ToList();
    }

    private static List<List<object>> FormatTableData(List<CodingGoal> goals)
    {
        return goals.Select(g => new List<object>
            {
                g.StartTime.ToString(_dateTimeFormat),
                g.EndTime.ToString(_dateTimeFormat),
                g.Hours,
                g.TimeCompleted.ToString(_durationFormat),
                g.HoursPerDayToComplete.ToString(_durationFormat)
            }).ToList();
    }
    
    private static List<List<object>> FormatTableData(List<CodingStatistic> statistics)
    {
        return statistics.Select(p => new List<object>
            {
                GetPeriodFormat(p),
                p.Total.ToString(_durationFormat),
                p.Average.ToString(_durationFormat)
            }).ToList();
    }

    private static string GetPeriodFormat(CodingStatistic statistic)
    {
        switch (statistic.PeriodType)
        {
            case PeriodType.Day:
                return statistic.StartDate.ToString("dd-MM-yyyy");
            case PeriodType.Week:
                var weekNumber = System.Globalization.ISOWeek.GetWeekOfYear(statistic.StartDate);
                return $"Week {weekNumber}, {statistic.StartDate.Year}";
            case PeriodType.Month:
                return statistic.StartDate.ToString("MM-yyyy");
            case PeriodType.Year:
                return statistic.StartDate.Year.ToString();
            default:
                return null;
        }
    }
}
