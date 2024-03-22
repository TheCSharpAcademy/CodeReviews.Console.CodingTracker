using CodingModel.HopelessCoding;
using DbHelpers.HopelessCoding;
using Spectre.Console;

namespace Helpers.HopelessCoding;

internal class GeneralHelpers
{
    public static CodingSession CreateCodingSession(int? id)
    {
        var (startTime, endTime) = DatabaseHelpers.GetTimeInput(id?.ToString());
        string sessionDuration = CalculateDuration(DateTime.Parse(startTime), DateTime.Parse(endTime));

        return new CodingSession
        {
            Id = id ?? 0,
            StartTime = startTime,
            EndTime = endTime,
            Duration = sessionDuration
        };
    }

    public static void PrintExecutionResult(int rowsAffected)
    {
        if (rowsAffected > 0)
        {
            AnsiConsole.Write(new Markup($"[green]\nThe record operation was successful.[/]\n"));
        }
        else
        {
            AnsiConsole.Write(new Markup($"[red]\nFailed to perform record operation.[/]\n"));
        }
        Console.WriteLine("----------------------------");
    }

    internal static string CalculateDuration(DateTime startTime, DateTime endTime)
    {
        TimeSpan duration = endTime - startTime;
        return duration.ToString(@"hh\:mm");
    }

    internal static void ShowConsoleTable(List<CodingSession> codingSessions, string tableTitle)
    {
        var table = new Table();
        table.Title = new TableTitle($"[underline yellow1]{tableTitle.ToUpper()}[/]");

        table.Border = TableBorder.MinimalDoubleHead;

        table.AddColumn("[gold1]ID[/]");
        table.AddColumn("[gold1]StartTime[/]");
        table.AddColumn("[gold1]EndTime[/]");
        table.AddColumn("[gold1]Duration[/]");

        for(int i = 0; i < table.Columns.Count(); i++)
        {
            table.Columns[i].Padding(4, 0);
        }

        foreach (var codingSession in codingSessions)
        {
            table.AddRow($"{codingSession.Id}", $"{codingSession.StartTime}", $"{codingSession.EndTime}", $"{codingSession.Duration}");
        }

        AnsiConsole.Write(table);
    }
}

