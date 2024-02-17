using codingTracker.Ibrahim.Models;
using ConsoleTableExt;

namespace codingTracker.Ibrahim.UI
{
    public class TableVisualizationEngine
    {
        public static void ShowTable<T>(List<T> history) where T : class
        {
            ConsoleTableBuilder
                .From(history)
                .WithTitle("Coding History")
                .ExportAndWriteLine();
        }
    }
}
