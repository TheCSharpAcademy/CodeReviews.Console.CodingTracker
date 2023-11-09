using ConsoleTableExt;
using System.Configuration;

namespace CodingTracker.SamGannon
{
    internal class TableVisualisation
    {
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            Console.Clear();
            Console.WriteLine("\n\n");

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Coding")
                .ExportAndWriteLine();
            Console.Write("\n\n");

            Console.WriteLine("press a key");
            Console.ReadLine();
        }
    }
}