using ConsoleTableExt;

namespace CodingTracker.jwhitt3r
{
    /// <summary>
    /// Visualises the data returned from the database onto the console
    /// </summary>
    internal class TableVisualisation
    {
        /// <summary>
        /// ShowTable builds a SQL-Like output via the ConsoleTableBuilder package
        /// </summary>
        /// <typeparam name="T">A generic type is used to allow for a wide array of information</typeparam>
        /// <param name="tableData">tableData is the data retrieved from the SQL Database</param>
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            Console.WriteLine("\n\n");
            ConsoleTableBuilder.From(tableData).WithTitle("Coding").ExportAndWriteLine();
            Console.WriteLine("\n\n");
        }
    }
}