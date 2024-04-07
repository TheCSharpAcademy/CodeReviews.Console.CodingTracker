using Spectre.Console;

namespace coding_tracker;

public class TableVisualisationEngine
{
    InputValidation validation = new();

    internal static void ShowTable<T>(List<T> tableData, bool showCaption = true) where T : class
    {
        DisplayData(tableData, showCaption);
    }

    internal static void ReportDisplay(List<CodingSession> reportData, bool showCaption = true)
    {
        DisplayData(reportData, showCaption);
    }

    private static void DisplayData<T>(List<T> data, bool showCaption = true) where T : class
    {
        var table = new Table()
            .Border(TableBorder.Double)
            .Title("[teal]All Coding Sessions[/]");

        if (showCaption)
        {
            table.Caption("[red]Press any key to return to Main Menu[/]");
        }
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
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Current Goal").Color(Color.Teal));

        var table = new Table()
            .Border(TableBorder.DoubleEdge);
        table.AddColumn("Days Remaining");
        table.AddColumn("Hours Per Day");

        if (hoursPerDay == 0)
        {
            table.AddRow(daysRemaining.ToString(), "Less than 1 hour per day");
        }
        else
        {
            table.AddRow(daysRemaining.ToString(), hoursPerDay.ToString());
        }
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
