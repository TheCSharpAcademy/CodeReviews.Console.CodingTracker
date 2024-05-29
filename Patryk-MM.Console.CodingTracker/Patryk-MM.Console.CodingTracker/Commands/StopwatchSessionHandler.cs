using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using Patryk_MM.Console.CodingTracker.Utilities;
using Spectre.Console;
using System;
using System.Diagnostics;

namespace Patryk_MM.Console.CodingTracker.Commands {
    public class StopwatchSessionHandler {
        private readonly TrackerService _trackerService;

        public StopwatchSessionHandler(TrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public void Handle() {
            // Initialize session
            CodingSession session = new CodingSession();

            AnsiConsole.MarkupLine("[green]Press any key to start tracking the session, or press Esc to cancel...[/]");

            // Wait for user input
            ConsoleKeyInfo keyInfo = System.Console.ReadKey(intercept: true);

            // Check if the pressed key is Esc
            if (keyInfo.Key == ConsoleKey.Escape) {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                return; // Cancel operation
            }

            // Start stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();
            DateTime startDate = DateTime.Now.Truncate(TimeSpan.FromSeconds(1));

            AnsiConsole.MarkupLine("[green]Session started. Press any key to end the session, or press Esc to cancel...[/]");

            // Wait for user input
            keyInfo = System.Console.ReadKey(intercept: true);

            // Stop stopwatch
            stopwatch.Stop();
            DateTime endDate = DateTime.Now.Truncate(TimeSpan.FromSeconds(1));

            // Check if the pressed key is Esc
            if (keyInfo.Key == ConsoleKey.Escape) {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                return; // Cancel operation
            }

            // Set session start and end dates
            session.StartDate = startDate;
            session.EndDate = endDate;

            // Display session details
            AnsiConsole.WriteLine($"{session}");

            // Save the session
            _trackerService.CreateSession(session);
            AnsiConsole.MarkupLine("[green]Session saved successfully![/]");
        }

    }
}
