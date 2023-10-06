using ConsoleTableExt;

namespace CodingTracker.Services;

public class TableVisualizationEngine
{
    public static void ShowTable(List<List<object?>?>? tableData, List<string?>? columnHeaders, string? title)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle(title)
            .WithColumn(columnHeaders)
            .ExportAndWrite();
    }
}
