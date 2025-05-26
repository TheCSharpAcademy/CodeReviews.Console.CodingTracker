using System.Diagnostics;
using Spectre.Console;

namespace CodingTracker.Golvi1124.Services;

internal class StopwatchService
{
    internal (DateTime StartTime, DateTime EndTime, TimeSpan Duration) RunStopwatch()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[green]Press [bold]Enter[/] to start the stopwatch...[/]");
        Console.ReadLine();

        var startTime = DateTime.Now;
        var stopwatch = Stopwatch.StartNew();

        AnsiConsole.MarkupLine("[yellow]Stopwatch started! Press [bold]Enter[/] again to stop.[/]\n");

        string displayText = "";
        AnsiConsole.Live(new Panel(displayText).Expand())
            .Start(ctx =>
            {
                while (!Console.KeyAvailable)
                {
                    var elapsed = stopwatch.Elapsed;
                    displayText = $"[blue]Elapsed time:[/] {elapsed:hh\\:mm\\:ss}";
                    ctx.UpdateTarget(new Panel(displayText).Expand());
                    Thread.Sleep(1000);
                }
            });

        Console.ReadLine(); // Consume the enter key
        stopwatch.Stop();
        var endTime = DateTime.Now;
        var duration = stopwatch.Elapsed;

        AnsiConsole.MarkupLine($"\n[bold green]Stopped! Total time: {duration} (seconds will be rounded to minutes)[/]");
        return (startTime, endTime, duration);
    }
}