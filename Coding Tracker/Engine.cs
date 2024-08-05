using Spectre.Console;

namespace CodingTracker;

public class TableVisualizationEngine
{
    static internal void DisplaySessionsTable(List<CodingSession> tableData)
    {
        Console.WriteLine();

        var table = new Table();
        table.Border = TableBorder.Horizontal;

        table.AddColumn("[yellow]Id[/]");
        table.AddColumn("[darkorange]Date[/]");
        table.AddColumn("[red]Language[/]");
        table.AddColumn("[blue]Start Time[/]");
        table.AddColumn("[blue]End Time[/]");
        table.AddColumn("[green]Duration[/]");

        foreach (CodingSession session in tableData)
        {
            table.AddRow($"[white]{session.Id}[/]", $"[white]{session.Date}[/]", $"[white]{session.Language}[/]", $"[white]{session.StartTime}[/]", $"[white]{session.EndTime}[/]", $"[white]{session.Duration}[/]");
        }

        AnsiConsole.Write(table);
    }

    static internal void DisplayGoalsTable(List<CodingGoal> tableData)
    {
        Console.WriteLine();

        var table = new Table();
        table.Border = TableBorder.Horizontal;

        table.AddColumn("[yellow]Id[/]");
        table.AddColumn("[red]Language[/]");
        table.AddColumn("[blue]Percentage Done[/]");
        table.AddColumn("[orangered1]Hours Left[/]");
        table.AddColumn("[green]Total Hours Goal[/]");

        foreach (CodingGoal goal in tableData)
        {
            table.AddRow($"[white]{goal.Id}[/]", $"[white]{goal.Language}[/]", $"[white]{goal.Percentage}%[/]", $"[white]{goal.HoursLeft}[/]", $"[white]{goal.TotalHours}[/]");
        }

        AnsiConsole.Write(table);
    }
}