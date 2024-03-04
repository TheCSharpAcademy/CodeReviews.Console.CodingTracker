using Spectre.Console;

namespace codingTracker.Fennikko;

public class UserInput
{
    public static void GetUserInput()
    {
        CodingController.DatabaseCreation();
        AnsiConsole.Clear();
        var appRunning = true;
        do
        {
            var functionSelect = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a [blue]function[/].")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Auto Session", "Manual Session", "Get Session History",
                        "Get Session History By Filter","Delete A Session",
                        "Update A Session","Exit"
                    }));
            switch (functionSelect)
            {
                case "Manual Session":
                    CodingController.ManualSession();
                    break;
                case "Get Session History":
                    CodingController.GetAllSessions();
                    break;
                case "Auto Session":
                    CodingController.AutoSession();
                    break;
                case "Get Session History By Filter":
                    CodingController.GetSessionFilterType();
                    break;
                case "Delete A Session":
                    CodingController.DeleteSession();
                    break;
                case "Update A Session":
                    CodingController.UpdateSession();
                    break;
                case "Exit":
                    appRunning = false;
                    Environment.Exit(0);
                    break;
            }
        } while (appRunning);
    }
}