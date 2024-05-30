using System.Text;
using CodingTracker.Controllers;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Views;

public static class TableVisualisationEngine
{
    public static void GenerateFullReport(bool showStats)
    {
        var tableData = CrudManager.GetAllSessions();

        var table = GenerateTable("Summary Report");

        foreach (var row in tableData)
            table.AddRow($"{row.Id}", $"{row.StartTime}", $"{row.EndTime}", $"{row.Duration}");

        AnsiConsole.Write(table);

        if (showStats) ProduceStats(tableData);
    }

    public static void GenerateSummaryReport()
    {
        var tableData = CrudManager.GetSummarySessions();

        var table = GenerateTable("Summary Report");

        foreach (var row in tableData)
            table.AddRow($"{row.Id}", $"{row.StartTime}", $"{row.EndTime}", $"{row.Duration}");

        AnsiConsole.Write(table);

        ProduceStats(CrudManager.GetAllSessions());
    }

    public static void GenerateFilteredReport()
    {
        try
        {
            var filterStartDate = UserInput.GetDateInput("start", "report filter");
            var filterEndDate = UserInput.GetDateInput("end", "report filter");

            var tableData = CrudManager.GetFilteredSessions(filterStartDate, filterEndDate);

            var table = GenerateTable("Filtered Report");

            foreach (var row in tableData)
                table.AddRow($"{row.Id}", $"{row.StartTime}", $"{row.EndTime}", $"{row.Duration}");

            AnsiConsole.Write(table);

            ProduceStats(tableData);
        }
        catch (HelpersValidation.InputZero)
        {
            Console.WriteLine("Returning to main menu...");
        }
    }

    private static Table GenerateTable(string reportName)
    {
        var table = new Table();

        table.Title($"\n[bold blue on yellow]-- {reportName} --[/]");
        table.Border(TableBorder.Rounded);

        table.AddColumns("[bold red]ID[/]", "[bold red]Start[/]", "[bold red]End[/]", "[bold red]Duration[/]");

        table.Columns[3].RightAligned();

        table.ShowFooters();
        return table;
    }

    private static void ProduceStats(List<CodingSession> tableData)
    {
        var totalTime = HelpersValidation.TotalTime(tableData);
        var panel = new Panel(new StringBuilder().Append("[bold]No. of coding sessions:[/] ")
            .Append(tableData.Count)
            .Append("\n[bold]Total time spent coding[/]: ")
            .Append((int)totalTime.TotalHours)
            .Append(':')
            .Append(totalTime.Minutes.ToString().PadLeft(2, '0'))
            .Append("\n[bold]Average session time: [/]")
            .Append((int)(totalTime / tableData.Count).TotalHours)
            .Append(':')
            .Append((totalTime / tableData.Count).Minutes.ToString().PadLeft(2, '0'))
            .ToString());

        panel.Header("[bold red]Statistics[/]");
        panel.Border = BoxBorder.Rounded;
        AnsiConsole.Write(panel);
    }
}