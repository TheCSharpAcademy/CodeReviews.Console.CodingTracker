using CodingTracker.matejadb.Controllers;
using static CodingTracker.matejadb.UI.Enums.Menu;
using Spectre.Console;

namespace CodingTracker.matejadb.UI;

internal class UserInterface {
    private readonly CodingSessionController _codingSessionControler = new CodingSessionController();

    internal void MainMenu() {
        while (true) {
            Console.Clear();

            var actionChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuAction>()
                .Title("Please select an option.")
                .AddChoices(Enum.GetValues<MenuAction>()));

            switch (actionChoice) {
                case MenuAction.ViewSessions:
                    ViewSessions();
                    break;
                case MenuAction.AddSession:
                    AddSession();
                    break;
                case MenuAction.DeleteSession:
                    DeleteSession();
                    break;
                case MenuAction.UpdateSession:
                    break;
            }
        }
    }

    private void ViewSessions() {
        _codingSessionControler.ViewSessions();
    }

    private void AddSession() {
        _codingSessionControler.AddSession();
    }

    private void DeleteSession() {
        _codingSessionControler.DeleteSession();
    }
}
