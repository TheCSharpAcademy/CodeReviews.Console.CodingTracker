using CodingTracker.selnoom.Data;
using Spectre.Console;
using System.Diagnostics;

namespace CodingTracker.selnoom.Helpers;

internal static class StopWatch
{
    internal static void StartStopWatch(CodingHoursRepository repository)
    {
        AnsiConsole.Clear();
        Stopwatch stopwatch = Stopwatch.StartNew();
        DateTime startTime = DateTime.Now;

        AnsiConsole.Live(new Markup($"Elapsed time: {stopwatch.Elapsed:hh\\:mm\\:ss}"))
        .Start(ctx =>
        {
            while (!Console.KeyAvailable)
            {
                ctx.UpdateTarget(new Markup($"Elapsed time: {stopwatch.Elapsed:hh\\:mm\\:ss}"));
                Thread.Sleep(500); // update every half second
            }
        });
        Console.ReadLine();

        DateTime endTime = DateTime.Now;
        stopwatch.Stop();

        string formattedStartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
        string formattedEndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss");

        repository.CreateRecord(formattedStartTime, formattedEndTime);

        AnsiConsole.MarkupLine("[bold green]Entry was successfully created! Press enter to continue[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }
}
