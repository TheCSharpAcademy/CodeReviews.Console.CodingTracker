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
                "View Sessions", "Enter New Sessions",
                "Update Session", "Delete Sessions", "Exit Application","Random Seed Database"
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
                case "Update Session":
                    UpdateSession();
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case "Delete Sessions":
                    DeleteSession();
                    Console.ReadLine();
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
                StopwatchSessionInput();
                break;
            case "Manual Session Entry":
                SessionController.AddNewManualSession();
                break;
            case "Back to Main Menu":
                break;
        }
    }
    internal void StopwatchSessionInput()
    {

        if(!AnsiConsole.Confirm("Start stopwatch now?"))
        {
            AnsiConsole.MarkupLine("Returning to Menu");
        }
        AnsiConsole.Clear();
        SessionController.SessionStopwatch();
    }
    internal string[] ManualSessionInput()
    {
        string[] sessionType = new string[] { "Start", "End" };
        string[] dateTime = new string[3];
        string userinput = "";
        bool validDateTime = false;

        for(int i = 0;i < sessionType.Length;i++)
        {
            while (!validDateTime)
            {
                userinput = AnsiConsole.Ask<string>($"Please enter the {sessionType[i]} date and time of your coding session. Format:[green]DD-MM-YY HH:MM[/]");
                validDateTime = validation.DateTimeValidation(userinput);
            }
            validDateTime = false;
            dateTime[i] = userinput;
        }
        dateTime[2] = Duration(dateTime[0], dateTime[1]);
        return dateTime;
    }

    internal string Duration(string sessionStart, string sessionEnd)
    {
        DateTime startTime = DateTime.ParseExact(sessionStart, "dd-MM-yy HH:mm:ss", new CultureInfo("en-GB"));
        DateTime endTime = DateTime.ParseExact(sessionEnd, "dd-MM-yy HH:mm:ss", new CultureInfo("en-GB"));

        TimeSpan duration = endTime.Subtract(startTime);
        return duration.TotalSeconds.ToString();
    }

    internal void UpdateSession()
    {
        int[] sessionIds = SessionController.GetIds();
        bool entryValid = false;
        int idSelection = 0;


        while (!entryValid)
        {
            SessionController.ViewAllSessions();
            idSelection = AnsiConsole.Ask<int>("Please type the Id number you would like to edit.");
            AnsiConsole.Clear();
            entryValid = validation.SessionIdInRange(sessionIds, idSelection);
            
        }

        SessionController.UpdateSession(idSelection);
        


        //Possible use if under certain number of entries
        //var updateId = AnsiConsole.Prompt(
        //    new SelectionPrompt<string>()
        //    .Title("Please select the operation with the arrow keys")
        //    .PageSize(10)
        //    .MoreChoicesText("[grey](Move up and down to reveal more entries)[/]")
        //    .AddChoices(allData.Reverse()));

        //SessionController.UpdateSession(updateId);
    }

    internal void DeleteSession()
    {
        int[] sessionIds = SessionController.GetIds();
        bool entryValid = false;
        int idSelection = 0;

        while (!entryValid)
        {
            SessionController.ViewAllSessions();
            idSelection = AnsiConsole.Ask<int>("Please type the Id number you would like to delete.");
            AnsiConsole.Clear();
            entryValid = validation.SessionIdInRange(sessionIds,idSelection);
        }
        SessionController.DeleteSession(idSelection);
    }
}
