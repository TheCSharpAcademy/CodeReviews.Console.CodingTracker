using CodingTracker.matejadb.Database;
using CodingTracker.matejadb.Models;
using Spectre.Console;
using CodingTracker.matejadb.Utils;

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

        foreach (var session in sessions) {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime,
                session.EndTime,
                session.Duration);
        }

        AnsiConsole.Write(table);
        DisplayMessage("Press any Key to continue");
        Console.ReadKey();
    }

    public void AddSession() {
        var startTime = UserInput.GetDateTimeFromUser("Start Time");
        var endTime = UserInput.GetDateTimeFromUser("End Time");

        var duration = CalculateSessionDuration.SessionDuration(startTime, endTime);

        _databaseManager.InsertNewSession(startTime, endTime, duration);
        DisplayMessage("Session successfully added.", "green");

        DisplayMessage("Press any Key to continue");
        Console.ReadKey();
    }

    public void DeleteSession() {
        var sessions = _databaseManager.GetAllSessions();

        if (sessions.Count == 0) {
            DisplayMessage("No session available to delete.", "red");
            Console.ReadKey();
            return;
        }

        var sessionToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<CodingSession>()
            .Title("Select a [red]session[/] to delete.")
            .UseConverter(s => $"{s.Id} {s.StartTime} {s.EndTime} {s.Duration}")
            .AddChoices(sessions));

        if (ConfirmDeletion(sessionToDelete)) {
            _databaseManager.DeleteExistingSession(sessionToDelete.Id);
            DisplayMessage("Book deleted successfully.", "red");
        }
        DisplayMessage("Press any Key to continue");
        Console.ReadKey();
    }

    public void UpdateSession() {
        var sessions = _databaseManager.GetAllSessions();

        if (sessions.Count == 0) {
            DisplayMessage("No sessions available to update.", "yellow");
            Console.ReadKey();
            return;
        }

        var sessionToUpdate = AnsiConsole.Prompt(
           new SelectionPrompt<CodingSession>()
           .Title("Select a [yellow]session[/] to update.")
           .UseConverter(s => $"{s.Id} {s.StartTime} {s.EndTime} {s.Duration}")
           .AddChoices(sessions));

        var startTime = UserInput.GetDateTimeFromUser("Start Time");
        var endTime = UserInput.GetDateTimeFromUser("End Time");
        var duration = CalculateSessionDuration.SessionDuration(startTime, endTime);

        if (ConfirmUpdate(sessionToUpdate)) {
            _databaseManager.UpdateExistingSession(sessionToUpdate.Id, startTime, endTime, duration);
            DisplayMessage("Session updated successfully.", "yellow");
        } else {
            DisplayMessage("Session update cancelled.", "red");
        }

        DisplayMessage("Press any Key to continue");
        Console.ReadKey();

    }
}
