
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal static class TableVisualisationEngine
{
    internal static void DisplayAllSessions(IEnumerable<CodingSession>? sessions)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumns(["Id", "Start Time", "End Time", "Duration (HH:mm:ss)"]);
        foreach (var session in sessions)
        {
            table.AddRow(session.Id.ToString(), session.StartTime.ToString(), session.EndTime.ToString(), session.Duration.ToString());
        }
        AnsiConsole.Write(table);
    }
}