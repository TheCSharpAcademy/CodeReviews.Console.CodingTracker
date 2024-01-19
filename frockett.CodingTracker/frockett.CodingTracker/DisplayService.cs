using frockett.CodingTracker.Library;
using Spectre.Console;
using System.Diagnostics;

namespace frockett.CodingTracker;

internal class DisplayService
{
    public void PrintSessionList(List<CodingSession> sessions)
    {
        Table table = new Table();
        table.AddColumn("Data ID");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        foreach (CodingSession session in sessions)
        {
            table.AddRow(session.Id.ToString(), session.StartTime.ToString("dd-MM-yyyy HH:mm"), session.EndTime.ToString("dd-MM-yyyy HH:mm"), session.Duration.ToString(@"h\:mm") + " hh:mm");
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("Press enter to continue");
        Console.ReadLine();
    }

    public void PrintMonthlyAverages(List<CodingSession> sessions)
    {
        Table table = new Table();
        table.Title("Monthly Averages");
        table.AddColumn("Month");
        table.AddColumn("Total Time");
        table.AddColumn("Average Per Session");
        table.AddColumn("Average Per Day");

        foreach (var session in sessions)
        {
            table.AddRow(session.Month, session.TotalTime.ToString() + " hours total", session.AverageTime.ToString() + " hours per session", Math.Round((session.TotalTime / 30), 1).ToString() + " hours per day");
        }
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("Press enter to continue");
        Console.ReadLine();
    }

    public Stopwatch DisplayStopwatch(Stopwatch stopwatch)
    {
        bool isRunning = true;
        var table = new Table();
        table.AddColumn(new TableColumn("[yellow]Time[/]").Centered());

        while (isRunning)
        {
            AnsiConsole.Clear();
            table.Rows.Clear();
            table.AddRow(stopwatch.Elapsed.ToString("hh\\:mm\\:ss"));
            AnsiConsole.Write(table);
            AnsiConsole.Markup("\nPress [green]any key[/] to end the session");
            Thread.Sleep(250);
            if (Console.KeyAvailable)
            {
                isRunning = false;
            }
        }
        return stopwatch;
    }
}
