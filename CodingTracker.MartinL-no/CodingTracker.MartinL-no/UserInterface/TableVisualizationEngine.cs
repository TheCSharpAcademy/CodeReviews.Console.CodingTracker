using ConsoleTableExt;

using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.UserInterface;

internal class TableVisualizationEngine
{
    private static string _dateTimeFormat = "d/M/yyyy H:mm";
    private static string _durationFormat = "h'h 'm'm'";

    public static void ShowTable(List<CodingSession> sessions)
    {
        var tableData = FormatTableDate(sessions);

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Start Time", "End Time", "Duration")
            .ExportAndWriteLine();        
    }

    public static void ShowTable(List<CodingGoal> goals)
    {
        var tableData = FormatTableDate(goals);

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Start Time", "Deadline", "Goal (Hours)", "Time Completed", "Time per day to meet goal")
            .ExportAndWriteLine();
    }

    public static void ShowTable(List<CodingStatistic> statistics)
    {
        var tableData = FormatTableDate(statistics);

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Period", "Total", "Average per session")
            .ExportAndWriteLine();
    }

    private static List<List<object>> FormatTableDate(List<CodingStatistic> statistics)
    {
        return statistics.Select(p => new List<object>
                    {
                        GetPeriodFormat(p),
                        p.Total.ToString(_durationFormat),
                        p.Average.ToString(_durationFormat)
                    }).ToList();
    }

    private static string GetPeriodFormat(CodingStatistic p)
    {
        switch (p.PeriodType)
        {
            case PeriodType.Day:
                return p.StartDate.ToString("d/M/yyyy");
            case PeriodType.Week:
                var weekNumber = System.Globalization.ISOWeek.GetWeekOfYear(p.StartDate);
                return $"Week {weekNumber}, {p.StartDate.Year}";
            case PeriodType.Month:
                return p.StartDate.ToString("M/yyyy");
            case PeriodType.Year:
                return p.StartDate.Year.ToString();
            default:
                return null;
        }
    }

    private static List<List<object>> FormatTableDate(List<CodingSession> sessions)
    {
        return sessions.Select(s => new List<object>
                    {
                        s.StartTime.ToString(_dateTimeFormat),
                        s.EndTime.ToString(_dateTimeFormat),
                        s.Duration.ToString(_durationFormat)
                    }).ToList();
    }

    private static List<List<object>> FormatTableDate(List<CodingGoal> goals)
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
}
