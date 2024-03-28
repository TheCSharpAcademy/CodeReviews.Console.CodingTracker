using Spectre.Console;

namespace CodingTracker;

public class UserInput
{
    public CodingController CodingController { get; set; }

    public UserInput(CodingController codingController)
    {
        CodingController = codingController;
        codingController.UserInput = this;
    }

    internal void MainMenu()
    {
        bool appIsRunning = true;
        do
        {
            var command = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU - Please select an option:")
                .PageSize(10)
                .AddChoices(
                    "View All Records",
                    "Generate a Report",
                    "Use a Timer",
                    "Insert a Record",
                    "Delete a Record",
                    "Update a Record",
                    "Close Application"
                ));

            switch (command)
            {
                case "Close Application":
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("Goodbye!");
                    Thread.Sleep(1000);
                    appIsRunning = false;
                    Environment.Exit(0);
                    break;
                case "View All Records":
                    CodingController.Get();
                    break;
                case "Generate a Report":
                    CodingController.GenerateReport();
                    break;
                case "Use a Timer":
                    AnsiConsole.Clear();
                    TimerMenu();
                    break;
                case "Insert a Record":
                    CodingController.Insert();
                    break;
                case "Delete a Record":
                    CodingController.Delete();
                    break;
                case "Update a Record":
                    CodingController.Update();
                    break;
                default:
                    AnsiConsole.MarkupLine("Invalid input.");
                    break;
            }
        } while (appIsRunning);
    }

    internal void TimerMenu()
    {
        bool timerIsRunning = true;
        do
        {
            var timer = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("TIMER MENU - Please select an option:")
                .PageSize(10)
                .AddChoices(
                    "Start/Pause/Resume Timer",
                    "Stop Timer",
                    "Main Menu"
                ));

            switch (timer)
            {
                case "Start/Pause/Resume Timer":
                    CodingController.TimerStartPauseResume();
                    break;
                case "Stop Timer":
                    CodingController.TimerStop();
                    break;
                case "Main Menu":
                    timerIsRunning = false;
                    AnsiConsole.Clear();
                    MainMenu();
                    break;
                default:
                    AnsiConsole.MarkupLine("Invalid input.");
                    break;
            }
        } while (timerIsRunning);
    }
}
