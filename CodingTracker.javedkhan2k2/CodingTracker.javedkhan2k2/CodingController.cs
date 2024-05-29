
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using CodingTracker.Models;

namespace CodingTracker;

internal class CodingController
{
    internal CodingTrackerDbContext dbContext;

    public CodingController(string connectionString)
    {
        dbContext = new CodingTrackerDbContext(connectionString);
    }

    internal void runApp()
    {
        bool runApplication = true;

        while(runApplication)
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
            while(userInput == null || !Regex.IsMatch(userInput, "^[0-4]$"))
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
                    ShowAllSessions();
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
                default:
                    break;
            }
        }
    }

    private void UpdateSession()
    {
        ShowAllSessions();
        int id = UserInput.GetIntegerValue("To update a Coding Session, please Enter an Id from the above Table: ");
        if(dbContext.GetCodingSessionById(id) != null)
        {
            CodingSessionDto codingSession = UserInput.GetNewCodingSession();
            dbContext.UpdateSessionCode(id, codingSession);
            Console.WriteLine("Coding Session updated successfully");
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
        ShowAllSessions();
    }

    private void AddNewSession()
    {
        CodingSessionDto codingSession = UserInput.GetNewCodingSession();
        dbContext.AddSessionCode(codingSession);
        Console.WriteLine("New Coding Session added successfully. Press any key to view main menu");
        Console.ReadLine();
    }

    private void ShowAllSessions()
    {
        var sessions = dbContext.GetAllCodingSessions();
        TableVisualisationEngine.DisplayAllSessions(sessions);
    }

}