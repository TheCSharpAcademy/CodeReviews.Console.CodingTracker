using CyrillParfen.CodingTracker.Model;
using Spectre.Console;

namespace CyrillParfen.CodingTracker.Helpers;

internal class UserPrompts
{
    internal static void PrintRecords(List<CodingSession> sessions)
    {
        Table table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Grey)
            .Title("[magenta]Coding Sessions[/]");

        table.AddColumn("Id");
        table.AddColumn("Session started");
        table.AddColumn("Session ended");
        table.AddColumn(new TableColumn("Duration").RightAligned());

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime.ToString(),
                session.EndTime.ToString(),
                session.Duration.ToString(@"hh\:mm\:ss")
                );
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine($"[dim]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    internal static void PrintRecords(CodingSession? session)
    {
        if (session == null)
        {
            Console.WriteLine("Record not found.");
            return;
        }

        PrintRecords(new List<CodingSession> { session });
    }

    internal static bool IsSessionMissing(CodingSession codingSession, int id)
    {
        if (codingSession is null)
        {
            AnsiConsole.MarkupLine($"[red]No record found with ID:{id}[/]");
            AnsiConsole.MarkupLine($"[blink]Press any key to continue...[/]");
            Console.ReadKey();
            return true;
        }

        return false;
    }

    internal static bool ActionConfirmation(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<bool>(message)
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "yes" : "no"));
    }
}
