using CodingTracker.wkktoria.Models;
using ConsoleTableExt;

namespace CodingTracker.wkktoria;

public static class TableVisualisationEngine
{
    public static void PrintAllRecordTable(List<CodingSession> tableData)
    {
        foreach (var td in tableData)
        {
            td.StartTime = DateTime.Parse(td.StartTime).ToString("dd MMM yyyy HH:mm:ss");
            td.EndTime = DateTime.Parse(td.EndTime).ToString("dd MMM yyyy HH:mm:ss");
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Coding Sessions")
            .WithColumn("Id", "Start Time", "End Time", "Duration (in hours)")
            .ExportAndWriteLine();
    }

    public static void PrintOneRecordTable(CodingSession record)
    {
        var tableData = new List<CodingSession> { record };
        PrintAllRecordTable(tableData);
    }

    public static void PrintReportTable(List<ReportData> tableData)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Report")
            .WithColumn("Total Time (in hours)", "Average Time (in hours)")
            .ExportAndWriteLine();
    }
}