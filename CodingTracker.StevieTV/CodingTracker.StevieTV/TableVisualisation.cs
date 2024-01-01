using ConsoleTableExt;

namespace CodingTracker.StevieTV;

internal class TableVisualisation
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Coding")
            .ExportAndWriteLine();
        Console.WriteLine("\n");
    }
}