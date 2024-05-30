
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal class CodingController
{
    internal CodingTrackerDbContext dbContext;

    public CodingController(string connectionString)
    {
        dbContext = new CodingTrackerDbContext(connectionString);
    }

    // Old version of runApp. Using the console.writeline() functions.
    internal void runAppOldVersion()
    {
        bool runApplication = true;

        while (runApplication)
        {
            Console.WriteLine(@$"
Main Menu

0 - Quit the Application.
1 - Show All Coding Sessions
2 - Add new Coding Session
3 - Update Coding Session
4 - Delete Coding Session
");
            Console.Write("Please enter an Option from the Menu above: ");
            string? userInput = Console.ReadLine();
            while (userInput == null || !Regex.IsMatch(userInput, "^[0-5]$"))
            {
                Console.Write($"Error!!! Invalid option: {userInput}. Please enter an Option from the Menu above: ");
                userInput = Console.ReadLine();
            }
            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Bye Bye");
                    runApplication = false;
                    break;
                case "1":
                    ViewAllSessions();
                    break;
                case "2":
                    AddNewSession();
                    break;
                case "3":
                    UpdateSession();
                    break;
                case "4":
                    DeleteSession();
                    break;
                case "5":
                    ShowMenu();
                    break;
                default:
                    break;
            }
        }
    }

    internal void runApp()
    {
        bool runApplication = true;

        while (runApplication)
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What do you want to do?")
                    .PageSize(6)
                    .AddChoices(new[] {
                        "View All Sessions", 
                        "Add New Session", 
                        "Add New Session using Stop Watch",
                        "Update Session", 
                        "Delete Session", 
                        "Exit"
                    }));
            switch (choice)
            {
                case "Add New Session":
                    AddNewSession();
                    break;
                case "Add New Session using Stop Watch":
                    AddSessionUsingStopWatch();
                    break;
                case "Update Session":
                    UpdateSession();
                    break;
                case "View All Sessions":
                    ViewAllSessions();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "Delete Session":
                    DeleteSession();
                    break;
                case "Exit":
                    return;
            }
        }
    }

    private void AddSessionUsingStopWatch()
    {
        Console.WriteLine("Press any key to start the timer");
        Console.ReadLine();
        DateTime startDateTime = DateTime.Now;
        Console.WriteLine($"Start Time: {startDateTime.ToString("yyyy-MM-dd HH:mm:ss")}");
        Console.WriteLine("Press any key to stop the timer");
        Console.ReadLine();
        DateTime endDateTime = DateTime.Now;
        Console.WriteLine($"End Time: {endDateTime.ToString("yyyy-MM-dd HH:mm:ss")}");
        long duration = (long)endDateTime.Subtract(startDateTime).Duration().TotalSeconds;
        CodingSessionDto codingSessionDto = new CodingSessionDto {
         StartTime = startDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
         EndTime = endDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
         Duration = duration
        };
        dbContext.AddSessionCode(codingSessionDto);
        Console.WriteLine("New Coding Session added successfully. Press any key to view main menu");
        Console.ReadLine();
    }

    private void ShowMenu()
    {
        Console.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What do you want to do?")
                .PageSize(10)
                .AddChoices(new[] {
                            "Add New Session", "Update Session", "View All Sessions", "Delete Session", "Exit"
                }));
        switch (choice)
        {
            case "Add New Session":
                AddNewSession();
                break;
            case "Update Session":
                UpdateSession();
                break;
            case "View All Sessions":
                ViewAllSessions();
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                break;
            case "Delete Session":
                DeleteSession();
                break;
            case "Exit":
                return;
        }
    }

    private void UpdateSession()
    {
        ViewAllSessions();
        int id = UserInput.GetIntegerValue("To update a Coding Session, please Enter an Id from the above Table: ");
        if (dbContext.GetCodingSessionById(id) != null)
        {
            CodingSessionDto codingSession = UserInput.GetNewCodingSession();
            if (dbContext.UpdateSessionCode(id, codingSession))
            {
                Console.WriteLine("Coding Session updated successfully");
            }
            else
            {
                Console.WriteLine($"The record with id {id} could not found in database");
            }
        }
        else
        {
            Console.WriteLine($"The record with id {id} could not found in database");
        }
        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private void DeleteSession()
    {
        ViewAllSessions();
        int id = UserInput.GetIntegerValue("To Delete a Coding Session, please Enter an Id from the above Table: ");
        if (dbContext.DeleteSessionCode(id))
        {
            Console.WriteLine("Coding Session deleted successfully");
        }
        else
        {
            Console.WriteLine($"The record with id {id} could not found in database");
        }
        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private void AddNewSession()
    {
        CodingSessionDto codingSession = UserInput.GetNewCodingSession();
        dbContext.AddSessionCode(codingSession);
        Console.WriteLine("New Coding Session added successfully. Press any key to view main menu");
        Console.ReadLine();
    }

    private void ViewAllSessions()
    {
        var sessions = dbContext.GetAllCodingSessions();
        TableVisualisationEngine.DisplayAllSessions(sessions);
    }

}