using Spectre.Console;

namespace CodingTracker.Models;

internal static class TableVisualisationEngine
{
    internal static void PrintSessions(List<CodingSession> sessions)
    {
        Console.Clear();
        var table = new Table();

        table.Title("Coding Sessions");

        table.AddColumn("ID");
        table.AddColumn("Date");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");
        table.AddColumn("Description");
        foreach (var session in sessions)
        {
            table.AddRow($"{session.Id}", $"{session.Date}", $"{session.StartTime}", $"{session.EndTime}", $"{session.DurationFormatted}", $"{session.Description}");
        }

        AnsiConsole.Write(table);
    }

}
