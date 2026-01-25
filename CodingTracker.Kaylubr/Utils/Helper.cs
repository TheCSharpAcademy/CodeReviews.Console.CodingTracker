using System.Globalization;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Utils;

internal static class Helper
{
    internal static int GetID(string mode)
    {
        AnsiConsole.WriteLine();
        return AnsiConsole.Ask<int>($"\nEnter the [green]ID[/] of the row to be [bold]{mode}[/]: ");
    }

    internal static (string StartTime, string EndTime) GetStartAndEndTime()
    {
        DateTime start;
        DateTime end;

        do
        {
            AnsiConsole.WriteLine();
            start = GetTime("start");
            end = GetTime("end");
        } while (!ValidateTime(start, end));

        string startTime = start.ToString();
        string endTime = end.ToString();

        return (startTime, endTime);
    }

    static DateTime GetTime(string message)
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

    internal static string GetDuration(string start, string end)
    {
        DateTime startTime = DateTime.Parse(start);
        DateTime endTime = DateTime.Parse(end);

        return (endTime - startTime).ToString(@"hh\:mm\:ss");
    }

    static bool ValidateTime(DateTime start, DateTime end)
    {
        if (start > end)
        {
            AnsiConsole.MarkupLine("[red]Starting time shouldn't be greater than the end time.[/]");
            return false;
        }

        return true;
    }

    internal static bool RenderCodingSessionInTable(List<CodingSession> codingSessions)
    {
        AnsiConsole.Clear();

        if (codingSessions.Count >= 1)
        {
            var table = new Table()
                .Title("[green bold]Session Records[/]")
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

            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No coding sessions to display. Add a session to see it here.[/]");
            return false;
        }

    }

    internal static bool Confirmation(string message)
    {
        AnsiConsole.WriteLine();

        string? choice;
        do
        {
            choice = AnsiConsole.Ask<string>($"{message} [bold green]Y[/] or [bold red]N[/]:").Trim().ToUpper();
        } while (choice != "Y" && choice != "N");

        if (choice == "N")
            return false;

        return true;
    }

    internal static void Pause()
    {
        AnsiConsole.Write("\nPress any key to continue..");
        Console.ReadKey();
    }

    internal static void Pause(string message, bool success)
    {
        if (success)
        {
            AnsiConsole.MarkupLine($"\n[green]{message}[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"\n[red]{message}[/]");
        }

        AnsiConsole.Write("\nPress any key to continue..");
        Console.ReadKey();

        AnsiConsole.Clear();
    }
}
