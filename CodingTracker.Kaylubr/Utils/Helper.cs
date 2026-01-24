using System.Globalization;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Utils;

internal static class Helper
{
    internal static DateTime GetTime(string message)
    {
        while (true)
        {
            var time = AnsiConsole.Ask<string>($"Enter [green]{message}[/] session time in the format (dd-MM-yy HH-mm): ");

            if (DateTime.TryParseExact(time, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                return DateTime.ParseExact(time, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
            }

            AnsiConsole.MarkupLine("[red]Invalid Format or Invalid Date & Time[/]");
        }

    }

    internal static string GetDuration(DateTime start, DateTime end)
    {
        return (end - start).ToString(@"hh\:mm\:ss");
    }

    internal static bool ValidateTime(DateTime start, DateTime end)
    {
        if (start > end)
        {
            AnsiConsole.MarkupLine("[red]Starting time shouldn't be greater than the end time.[/]");
            return false;
        }

        return true;
    }

    internal static void RenderCodingSessionInTable(List<CodingSession> codingSessions)
    {
        var table = new Table()
            .Title("\n[green bold]Session Records[/]")
            .Border(TableBorder.Heavy);

        table.AddColumn("ID");
        table.AddColumn("Starting time");
        table.AddColumn("End time");
        table.AddColumn("Duration");

        foreach (var session in codingSessions)
        {
            table.AddRow(session.Id.ToString(), session.StartTime.ToString(), session.EndTime.ToString(), session.Duration);
        }

        AnsiConsole.Write(table);

        AnsiConsole.WriteLine("\nPress any key to exit..");
        Console.ReadKey();
    }

    internal static bool Confirmation()
    {
        string? choice;

        do
        {
            choice = AnsiConsole.Ask<string>(@"Do you want to perform the operation [green]y[/] or [red]n[/]:").Trim().ToLower();
        } while (choice != "y" && choice != "n");

        if (choice == "n")
            return false;

        return true;
    }

    internal static void PrintSuccessOperation()
    {
        AnsiConsole.MarkupLine("\n[green]Successful operation![/]");

        AnsiConsole.WriteLine("\nPress any key to continue..");
        Console.ReadKey();
    }
}
