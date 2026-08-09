using CyrillParfen.CodingTracker.Model;
using Spectre.Console;
using System.Diagnostics;

namespace CyrillParfen.CodingTracker.Services;

internal class CodingStopwatch
{
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private CodingSession _codingSession;

    internal void Start()
    {
        _codingSession = new CodingSession() { StartTime = DateTime.Now };
        _stopwatch.Start();

        AnsiConsole.MarkupLine($"[green]Stopwatch started at {_codingSession.StartTime:dd.MM.yy HH:mm:ss}[/]");
    }

    internal CodingSession Stop()
    {
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("Elapsed Time");
        table.AddRow("00:00:00");

        AnsiConsole.Live(table).Start(ctx =>
        {
            while(!Console.KeyAvailable)
            {
                table.Rows.Update(0, 0, new Markup(_stopwatch.Elapsed.ToString(@"hh\:mm\:ss")));
                ctx.Refresh();
            }
        });

        _stopwatch.Stop();
        _codingSession.EndTime = DateTime.Now;
        return _codingSession;
    }

    internal void ShowStopwatchResult(CodingSession codingSession)
    {
        AnsiConsole.Clear();

        var content = new Rows(
            new Markup($"[bold]Start:[/] {codingSession.StartTime:dd.MM.yy HH:mm}"),
            new Markup($"[bold]End:[/] {codingSession.EndTime:dd.MM.yy HH:mm}"),
            new Markup($"[bold green]Duration:[/] {codingSession.Duration.Hours}:" +
                $"{codingSession.Duration.Minutes:D2}:" +
                $"{codingSession.Duration.Seconds:D2}"));

        var panel = new Panel(content)
            .Header("[yellow]Session finished![/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Green);

        AnsiConsole.Write(panel);
    }
}
