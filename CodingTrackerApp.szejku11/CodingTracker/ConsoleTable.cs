using ConsoleTableExt;

namespace CodingTracker
{
    internal static class ConsoleTable
    {
        internal static void DisplayData(List<List<object>> data)
        {
            ConsoleTableBuilder
                .From(data)
                .WithTitle("Coding Tracker ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                .WithColumn("Id", "Start Time", "End Time", "Duration")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }
    }
}
