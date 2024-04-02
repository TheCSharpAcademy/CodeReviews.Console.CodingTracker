using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class UserInput
{
    public SessionController SessionController { get; set; }

    public UserInput(SessionController sessionController)
    {
        SessionController = sessionController;
        sessionController.UserInput = this;
    }

    static InputValidation validation = new();
    static RandomSeed randomSeed = new();
    internal void MainMenu()
    {
        bool endApplication = false;
        do
        {
            var crudActions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the operation with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "View Sessions", "Enter New Sessions", "Reports",
                "Update Session", "Delete Sessions", "Exit Application",
                "Random Seed Database"
            }));

            switch (crudActions)
            {
                case "View Sessions":
                    SessionController.ViewAllSessions();
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case "Enter New Sessions":
                    SessionTrackingType();
                    break;
                case "Reports":
                    SessionController.WeeklyCodingSessions();
                    break;
                case "Update Session":
                    SessionController.UpdateSession();
                    AnsiConsole.Clear();
                    break;
                case "Delete Sessions":
                    SessionController.DeleteSession();
                    AnsiConsole.Clear();
                    break;
                case "Exit Application":
                    endApplication = true;
                    Environment.Exit(0);
                    break;
                case "Random Seed Database":
                    randomSeed.GenerateRandomData();
                    break;
            }
        } while (!endApplication);
    }
    internal void SessionTrackingType()
    {
        var trackingType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the operation with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Stopwatch Session", "Manual Session Entry", "Back to Main Menu"
            }));

        switch (trackingType)
        {
            case "Stopwatch Session":
                SessionController.SessionStopwatch();
                break;
            case "Manual Session Entry":
                SessionController.AddNewManualSession();
                break;
            case "Back to Main Menu":
                break;
        }
    }
}
