using CodingTracker.Chad1082.Models;
using ConsoleTableExt;

namespace CodingTracker.Chad1082.Data
{
    internal static class TableVisualisationEngine
    {
        internal static void ShowTable(List<CodingSession> codingSessions)
        {
            ConsoleTableBuilder
                .From(codingSessions)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithTitle("Coding Sessions ", ConsoleColor.White, ConsoleColor.DarkGray)
                .ExportAndWriteLine();
        }

        internal static void ShowTable(CodingSession codingSession)
        {
            List<CodingSession> tmpList = new();
            tmpList.Add(codingSession);

            ConsoleTableBuilder
                .From(tmpList)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithTitle("Coding Sessions ", ConsoleColor.White, ConsoleColor.DarkGray)
                .ExportAndWriteLine();
        }
        internal static void DisplayAllSessions()
        {
            Console.WriteLine("The following sessions have been logged:\n");

            List<CodingSession> codingSessions = new();
            codingSessions = CodingController.GetSessions();

            ShowTable(codingSessions);
        }
    }
}
