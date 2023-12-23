using ConsoleTableExt;

internal class TableVisulation
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Coding")
            .WithColumn("Id", "Start Date","End Date", "Hours coding")
            .ExportAndWriteLine();
        Console.WriteLine("\n\n");
    }
}