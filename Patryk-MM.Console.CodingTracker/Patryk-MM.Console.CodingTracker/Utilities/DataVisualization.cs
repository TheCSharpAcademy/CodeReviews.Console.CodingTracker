using Patryk_MM.Console.CodingTracker.Models;
using Spectre.Console;
using System.Collections.Generic;
using System.Linq;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    /// <summary>
    /// Provides utility methods for data visualization.
    /// </summary>
    public static class DataVisualization {
        /// <summary>
        /// Prints a table of coding sessions.
        /// </summary>
        /// <param name="sessions">The list of coding sessions to print.</param>
        public static void PrintSessions(List<CodingSession> sessions) {
            if (!sessions.Any()) {
                AnsiConsole.MarkupLine("[red]No sessions found.[/]");
                return;
            }

            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Start date");
            table.AddColumn("End date");
            table.AddColumn("Duration");

            for (int i = 0; i < sessions.Count; i++) {
                table.AddRow((i + 1).ToString(), sessions[i].StartDate.ToString(), sessions[i].EndDate.ToString(), sessions[i].Duration.ToString());
            }

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Prints the logo for the Coding Tracker application.
        /// </summary>
        public static void PrintLogo() {
            AnsiConsole.Write(
            new FigletText("Coding Tracker")
            .Centered()
            .Color(Color.Green));
        }
    }
}
