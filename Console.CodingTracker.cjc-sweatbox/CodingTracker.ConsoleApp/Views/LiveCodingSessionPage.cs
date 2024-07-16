using CodingTracker.Models;
using Spectre.Console;
using System.Diagnostics;

namespace CodingTracker.ConsoleApp.Views;

/// <summary>
/// Page which allows users to start and track live CodingSession.
/// </summary>
internal class LiveCodingSessionPage : BasePage
{
    #region Constants

    private const string PageTitle = "Live Coding Session";

    #endregion
    #region Methods: Internal

    internal static CodingSession Show()
    {
        Stopwatch stopwatch = new Stopwatch();

        AnsiConsole.Clear();
        WriteHeader(PageTitle);
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Press any [blue]key[/] to start the session...");
        Console.ReadKey();

        // Start timer.
        DateTime start = DateTime.Now;
        stopwatch.Start();

        AnsiConsole.Clear();
        WriteHeader(PageTitle);
        var stopwatchDisplayRow = Console.CursorTop;
        AnsiConsole.MarkupLine(@$"Current coding session duration: [olive]{stopwatch.Elapsed:hh\:mm\:ss}[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Press any [blue]key[/] to stop the session...");

        // Stop the cursor flashing every update.
        Console.CursorVisible = false;
        
        var lastUpdate = stopwatch.Elapsed;
        while (!Console.KeyAvailable)
        {
            // Only update every 500 milliseconds.
            if (stopwatch.Elapsed > lastUpdate.Add(TimeSpan.FromMilliseconds(500)))
            {
                lastUpdate = stopwatch.Elapsed;
                Console.SetCursorPosition(0, stopwatchDisplayRow);
                AnsiConsole.MarkupLine(@$"Current coding session duration: [olive]{stopwatch.Elapsed:hh\:mm\:ss}[/]");
            }
        }

        // Read the key press (Stops messing up the next Console.ReadKey() press).
        Console.ReadKey();

        // Stop timer.
        stopwatch.Stop();
        DateTime end = DateTime.Now;

        return new CodingSession(start, end);
    }

    #endregion
}
