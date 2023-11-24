using ConsoleTableExt;

namespace CodingTracker.jkjones98
{
    internal class ConsoleTableCreator
    {
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            Console.WriteLine("\n\n");

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Coding")
                .ExportAndWriteLine();
            Console.WriteLine("\n\n");
        }
    }
}