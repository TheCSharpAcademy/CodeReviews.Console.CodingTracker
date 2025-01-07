using Spectre.Console;
using System.Diagnostics;

internal class SessionStopwatch
{
    internal static void Start()
    {
        Console.Clear();
        AnsiConsole.Markup("Press any key to start code session: ");
        Console.ReadKey(true);
        Console.Clear();

        AnsiConsole.MarkupLine("[green]Coding session started![/]");
        AnsiConsole.MarkupLine(
            $"Date: [yellow]{DateTime.Today.ToString(Config.DateFormat)}[/] " +
            $"Start Time: [yellow]{DateTime.Now.ToString(Config.TimeFormat)}[/]\n");

        Thread.Sleep(200);
        var startTime = DateTime.Now;
        TimeSpan duration = SetSessionDurationByKeyPress();

        while ($"{duration:hh\\:mm}" == "00:00")
        {
            AnsiConsole.MarkupLine("\n[red]Current session duration is less than one minute![/]");
            AnsiConsole.MarkupLine("Can not add a new session record with that result.\n");

            var startAgain = DisplayInfoHelpers.GetYesNoAnswer("Do you want to start session again?");
            if (!startAgain)
            {
                Console.Clear();
                return;
            }
            duration = SetSessionDurationByKeyPress();
        }

        RecordCreate.InsertNewRecordIntoDb(startTime, startTime + duration, duration);
    }

    internal static TimeSpan SetSessionDurationByKeyPress()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        AnsiConsole.MarkupLine("[yellow]Press any key to end coding session.[/]");

        while (true)
        {
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true);
                break;
            }
            Console.Write($"\rTime elapsed: {stopwatch.Elapsed:hh\\:mm\\:ss}");
        }
        stopwatch.Stop();

        AnsiConsole.MarkupLine($"\nSession duration: [green]{stopwatch.Elapsed:hh\\:mm}[/]");
        return stopwatch.Elapsed;
    }
}
