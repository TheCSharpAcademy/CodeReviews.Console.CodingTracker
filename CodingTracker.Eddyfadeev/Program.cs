using CodingTracker.enums;
using CodingTracker.services;
using CodingTracker.views;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker;

internal static class Program
{
    private static readonly DatabaseService DatabaseService = new();
    static Program()
    {
        DatabaseService.InitializeDatabase();
    }
    
    private static void Main(string[] args)
    {
        Start();
    }

    private static void Start()
    {
        var controller = new CodingController(DatabaseService);
        var isRunning = true;

        do
        {
            try
            {
                var userChoice = MenuView.ShowMainMenu();

                if (userChoice == MainMenuEntries.Quit)
                {
                    isRunning = false;
                    AnsiConsole.WriteLine("Goodbye!");

                    continue;
                }

                if (userChoice == MainMenuEntries.Reports)
                {
                    OpenReportsMenu();
                    continue;
                }

                if (userChoice == MainMenuEntries.Timer)
                {
                    OpenTimerMenu();
                }

                ServiceHelpers.InvokeActionForMenuEntry(userChoice, controller);
            }
            catch (SqliteException)
            {
                AnsiConsole.WriteLine("Problem with the database. Please try again or restart the app.");
            }
            
        } while(isRunning);
    }

    private static void OpenReportsMenu()
    {
        var reportsService = new ReportService(DatabaseService);
        var isRunning = true;

        do
        {
            var userChoice = MenuView.ShowReportsMenu();

            if (userChoice == ReportTypes.BackToMainMenu)
            {
                isRunning = false;
                continue;
            }
            
            reportsService.HandleUserChoice(userChoice);
            
        } while (isRunning);
    }

    private static void OpenTimerMenu()
    {
        var timerService = new TimerService(DatabaseService);
        var isRunning = true;

        do
        {
            var userChoice = MenuView.ShowTimerMenu();
            
            if (userChoice == TimerMenuEntries.BackToMainMenu)
            {
                isRunning = false;
                continue;
            }
            
            ServiceHelpers.InvokeActionForMenuEntry(userChoice, timerService);
        } while (isRunning);
    }
}