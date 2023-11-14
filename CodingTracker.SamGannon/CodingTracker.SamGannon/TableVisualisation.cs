using ConsoleTableExt;

namespace CodingTracker.SamGannon;

internal class TableVisualisation
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        Console.Clear();
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("-")
            .ExportAndWriteLine();
        Console.Write("\n\n");
    }
}