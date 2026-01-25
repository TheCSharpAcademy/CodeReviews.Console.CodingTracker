using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Utils;

internal static class Helper
{
    internal static bool ValidateTime(DateTime start, DateTime end)
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
