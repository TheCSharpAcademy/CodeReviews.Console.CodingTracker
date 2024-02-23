using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal class TableVisualisation
{
    internal static void ShowCodingTable(List<Coding> tableData, string title)
    {
        var table = new Table();

        table.Title = new TableTitle(title, "bold");

        table.AddColumns($"[{Color.Olive}]Id[/]", $"[{Color.Olive}]Start[/]", $"[{Color.Olive}]End[/]", $"[{Color.Olive}]Duration[/]").Centered();

        foreach (var c in tableData)
        {
            table.AddRow(c.Id.ToString(), $"{c.StartTime.ToString("dd-MM-yy H:mm")}", $"{c.EndTime.ToString("dd-MM-yy H:mm")}", $"{FormatDuration(c.Duration)}").Centered();
        }

        AnsiConsole.Write(table);
    }

    internal static void ShowGoalsTable(List<Goal> tableData, string title)
    {
        var table = new Table();

        table.Title = new TableTitle(title, "bold");

        table.AddColumns($"[{Color.Olive}]Id[/]", $"[{Color.Olive}]Start[/]", $"[{Color.Olive}]End[/]", $"[{Color.Olive}]Amount[/]").Centered();

        foreach (var g in tableData)
        {
            table.AddRow(g.Id.ToString(), $"{g.StartDate.ToString("dd-MM-yy")}", $"{g.EndDate.ToString("dd-MM-yy")}", $"{g.Amount} hours").Centered();
        }

        AnsiConsole.Write(table);
    }

    private static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays > 1)
        {
            return string.Format("{0:%d} days, {0:%h} hours, {0:%m} minutes", duration);
        }
        else
        {
            return string.Format("{0:%h} hours, {0:%m} minutes", duration);
        }
    }
}
