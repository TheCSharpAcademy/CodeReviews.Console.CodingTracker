using ConsoleTableExt;
using frockett.CodingTracker.Library;
using Spectre.Console;
using System.Diagnostics;
using System.Transactions;

namespace frockett.CodingTracker;

internal class DisplayService
{
    
    public void PrintSessionList(List<CodingSession> sessions)
    {
        // This is from experimenting with converting the list contents to strings first
        /*
        List<CodingSessionStrings> toPrint = new List<CodingSessionStrings>();

        foreach (CodingSession s in sessions)
        {
            toPrint.Add( 
                new CodingSessionStrings
                {
                    PrintStartTime = s.StartTime.ToString(),
                    PrintEndTime = s.EndTime.ToString(),  
                    PrintDuration = s.Duration.TotalHours.ToString(),
                }
                );
        }

        var tableBuilder = ConsoleTableBuilder.From(toPrint);
        */

        var tableBuilder = ConsoleTableBuilder.From(sessions);

        tableBuilder.ExportAndWriteLine();

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

    /*
    async void PrintStopwatch(Stopwatch stopwatch)
    {
        var table = new Table();
        table.AddColumn(new TableColumn("[yellow]Time[/]").Centered());

        while (true)
        {
            AnsiConsole.Clear();
            table.Rows.Clear();
            table.AddRow(stopwatch.Elapsed.ToString("hh\\:mm\\:ss"));
            AnsiConsole.Write(table);
            //await Task.Delay(1000);
            Thread.Sleep(100);
        }
    }
    */
}
