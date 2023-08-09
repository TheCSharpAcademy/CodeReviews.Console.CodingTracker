using ConsoleTableExt;

using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.UserInterface;

internal class TableVisualizationEngine
{
    public static void ShowTable(List<CodingSession> sessions)
    {
        var tableData = FormatTableDate(sessions);

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Start Time", "End Time", "Duration")
            .ExportAndWriteLine();        
    }

    private static List<List<object>> FormatTableDate(List<CodingSession> sessions)
    {
        return sessions.Select(s => new List<object> { s.StartTime.ToString("d/M/yyyy H:m"), s.EndTime.ToString("d/M/yyyy H:m"), s.Duration.ToString("h'h 'm'm'") }).ToList();
    }
}
