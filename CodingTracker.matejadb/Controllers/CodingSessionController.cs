using CodingTracker.matejadb.Config;
using CodingTracker.matejadb.Database;
using CodingTracker.matejadb.Models;
using Spectre.Console;

namespace CodingTracker.matejadb.Controllers; 
internal class CodingSessionController : BaseController, IBaseController {
    DatabaseManager _databaseManager = new();

    public void ViewSessions() {
        var table = new Table();

        table.AddColumn("[darkorange]Id[/]");
        table.AddColumn("[darkorange]Start Time[/]");
        table.AddColumn("[darkorange]End Time[/]");
        table.AddColumn("[darkorange]Duration[/]");

        var sessions = _databaseManager.GetAllSessions();

        foreach(var session in sessions) {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime,
                session.EndTime,
                session.Duration);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press any Key to continue.");
        Console.ReadKey();
    }

    public void AddSession() {
        var startTime = AnsiConsole.Ask<string>($"Enter the [darkorange]Start Time[/] of your session ({AppSettings.DateFormat}):");
        var endTime = AnsiConsole.Ask<string>($"Enter the [darkorange]End Time[/] of your session ({AppSettings.DateFormat}):");
        // temporary manual duration input
        var duration = AnsiConsole.Ask<string>("Enter the [darkorange]Duration[/] of your session:");

        _databaseManager.AddSession(startTime, endTime, duration);
        AnsiConsole.MarkupLine($"[green]Session successfully added![/]");

        AnsiConsole.MarkupLine("Press any Key to continue.");
        Console.ReadKey();
    }

    public void DeleteSession() {
        var sessions = _databaseManager.GetAllSessions();

        if(sessions.Count == 0) {
            AnsiConsole.MarkupLine("[red]No sessions available to delete[/]");
            Console.ReadKey();
            return;
        }

        var sessionToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<CodingSession>()
            .Title("Select a [red]session[/] to delete.")
            .UseConverter(s => $"{s.Id} {s.StartTime} {s.EndTime} {s.Duration}")
            .AddChoices(sessions));

        if(ConfirmDeletion(sessionToDelete)) {
            _databaseManager.DeleteSession(sessionToDelete.Id);
            AnsiConsole.MarkupLine("[red]Book deleted successfully.[/]");
        }

        AnsiConsole.MarkupLine("Press any Key to continue.");
        Console.ReadKey();
    }

    public void UpdateSession() { }
}
