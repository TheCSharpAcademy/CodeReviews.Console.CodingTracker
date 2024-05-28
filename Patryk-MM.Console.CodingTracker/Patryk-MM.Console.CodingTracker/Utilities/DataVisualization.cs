using Patryk_MM.Console.CodingTracker.Models;
using Spectre.Console;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    public static class DataVisualization {
        public static void PrintSessions(List<CodingSession> sessions) {
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
    }
}
