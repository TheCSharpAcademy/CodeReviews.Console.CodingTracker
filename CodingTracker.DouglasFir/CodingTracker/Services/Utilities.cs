using CodingTracker.Models;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace CodingTracker.Services;

public static class Utilities
{
    public static string SplitCamelCase(string input)
    {
        return Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
    }

    public static void PrintNewLines(int numOfNewLines)
    {
        for (int i = 0; i < numOfNewLines; i++) { Console.WriteLine(); }
    }

    public static void DisplayExceptionErrorMessage(string message, string exception) 
    {
        PrintNewLines(1);
        string errorMessage = $"[red]{message}[/]\n{exception}";
        AnsiConsole.MarkupLine(errorMessage);
        PrintNewLines(1);
    }

    public static void DisplaySuccessMessage(string message)
    {
        PrintNewLines(1);
        string successMessage = $"[chartreuse1]{message}[/]";
        AnsiConsole.MarkupLine(successMessage);
        PrintNewLines(1);
    }

    public static void DisplayWarningMessage(string message)
    {
        PrintNewLines(1);
        string warningMessage = $"[lightgoldenrod2_2]{message}[/]";
        AnsiConsole.MarkupLine(warningMessage);
        PrintNewLines(1);
    }

    public static void DisplayCancellationMessage(string message)
    {
        PrintNewLines(1);
        string successMessage = $"[blueviolet]{message}[/]";
        AnsiConsole.MarkupLine(successMessage);
        PrintNewLines(1);
    }

    public static void DisplayCurrentQuerySelections(List<(CodingSessionModel.EditableProperties column, SortDirection? direction, int? rank)> orderByProperties, Color[] colors)
    {
        var table = new Table();
        table.AddColumn("[bold]Column[/]");
        table.AddColumn("[bold]Direction[/]");
        table.AddColumn("[bold]Rank[/]");
        table.Border(TableBorder.Rounded);

        int colorIndex = 0;
        foreach (var (column, direction, rank) in orderByProperties)
        {
            string columnName = $"[{colors[colorIndex++]}]{Utilities.SplitCamelCase(column.ToString())}[/]";
            table.AddRow(columnName, direction?.ToString() ?? "Not Set", rank?.ToString() ?? "Not Set");
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
        PrintNewLines(2);
    }

    public static int GetDaysMultiplier(TimePeriod period)
    {
        switch (period)
        {
            case TimePeriod.Days:
                return 1;
            case TimePeriod.Weeks:
                return 7;
            case TimePeriod.Years:
                return 365;
            default:
                throw new ArgumentOutOfRangeException(nameof(period), "Unsupported time period.");
        }
    }
}
