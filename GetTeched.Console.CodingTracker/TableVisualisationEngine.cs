using Spectre.Console;

namespace coding_tracker;

public class TableVisualisationEngine
{
    InputValidation validation = new();

    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        DisplayData(tableData);
    }

    internal static void ReportDisplay(List<CodingSession> reportData)
    {
        DisplayData(reportData);
    }

    private static void DisplayData<T>(List<T> data) where T : class
    {
        var table = new Table()
            .Border(TableBorder.Double)
            .Title("[teal]All Coding Sessions[/]");
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            table.AddColumn(new TableColumn($"[yellow]{property.Name}[/]"));
        }

        foreach (var item in data)
        {
            var row = new List<string>();
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                switch (property.Name)
                {
                    case "Date":
                        value = InputValidation.DateTimeParse(value.ToString(), true, false);
                        
                        break;
                    case "StartTime":
                        value = InputValidation.DateTimeParse(value.ToString(), false, true);
                        break;
                    case "EndTime":
                        value = InputValidation.DateTimeParse(value.ToString(), false, true);
                        break;
                    case "Duration":
                        value = InputValidation.SecondsConversion(value.ToString());
                        break;
                    case "Completed":
                        if (Convert.ToInt32(value) == 0)
                        {
                            value = "[red]FAILED[/]";
                        }
                        else value = "[green]PASSED[/]";
                        break;
                }
                row.Add(value?.ToString() ?? string.Empty);
            }
            table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
    }
    //internal void TableGeneration(List<string> columnHeaders, List<string> rowData)
    //{
    //    var table = new Table()
    //        .Border(TableBorder.DoubleEdge)
    //        .Title("[teal]All Coding Sessions[/]");
    //    string[] columns = columnHeaders.ToArray();
    //    string[] rows = rowData.ToArray();

    //    for(int i=0; i<columns.Length; i++)
    //    {
    //        table.AddColumn($"[yellow]{columns[i]}[/]");
    //    }
    //    for(int i = 0; i < rows.Length; i += 5)
    //    {
    //        table.AddRow(rows[i], rows[i + 1], rows[i + 2], rows[i + 3], rows[i + 4]);
    //    }

    //    AnsiConsole.Write(table);
    //}

    internal void StopwatchTable()
    {
        var table = new Table()
            .Border(TableBorder.DoubleEdge)
            .Caption("[red]Press F to stop[/]");
        table.AddColumn("[green]Stopwatch Elapsed Time[/]");
        table.AddColumn("             ");
        AnsiConsole.Write(table);
    }

    internal void TotalTable(string[] value)
    {
        var table = new Table()
            .Border(TableBorder.DoubleEdge);
        table.AddColumn("[green]Total Duration[/]");
        table.AddColumn(value[0]);
        table.AddColumn("[green]Average Duration[/]");
        table.AddColumn(value[1]);
        AnsiConsole.Write(table);
    }

    internal void WeekGenerator(List<string> rowData)
    {
        var table = new Table()
            .Border(TableBorder.DoubleEdge)
            .ShowFooters();
        string[] rows = rowData.ToArray();

        table.AddColumn("WeekNumber");
        table.AddColumn("Duration");

        for (int i = 0; i < rowData.Count - 1; i += 2)
        {
            table.AddRow(rowData[i], rowData[i + 1]);
        }
        AnsiConsole.Write(table);
    }

    internal void GoalBreakDown(int hoursRemaining, int hoursCompleted, int daysRemaining, int hoursPerDay)
    {
        var table = new Table()
            .Border(TableBorder.DoubleEdge);
        table.AddColumn("Days Remaining");
        table.AddColumn("Hours Per Day");
        table.AddRow(daysRemaining.ToString(), hoursPerDay.ToString());
        AnsiConsole.Write(table);
        AnsiConsole.Write("\n");
        AnsiConsole.Write(new BreakdownChart()
            .Width(60)
            .AddItem("Hours Completed", hoursCompleted, Color.Green)
            .AddItem("Hours to Reach Goal", hoursRemaining, Color.Red));

        AnsiConsole.Write(new Markup("\n[red]Press any key to return to Main Menu[/]"));
        Console.ReadLine();
        AnsiConsole.Clear();

    }
}
