using CodingTracker.library.Controller;

namespace CodingTracker.library.View;

public static class Menu
{
    private static string ShowMainMenu()
    {
        Dictionary<string, string> mainMenu = new Dictionary<string, string>() { 
            {"exit", "Exit Coding Tracker"},
            {"automatic", "Start New Automatic Coding Session" },
            {"view all", "View History Of Coding Sessions" },
            {"insert", "Insert New Coding Session" },
            {"delete", "Delete Coding Session" },
            {"update", "Update Coding Session"},
            {"report", "View Reports of Coding Sessions"}
        };

        TableVisualizationEngine.PrintMenues(mainMenu, "Main Menu");

        Console.Write("Your choice: ");
        string choice = Validations.GetMenuChoice(Console.ReadLine());

        return choice;
    }

    private static bool MainMenuChoice(string choice)
    {
        bool continueApp = true;

        switch(choice.Trim().ToLower()) 
        {
            case "exit":
                continueApp = false;
                break;

            case "automatic":
                AutomaticSession.NewAutomaticSession();
                continueApp = true;
                break;

            case "view all":
                CrudController.ViewAllSessions();
                continueApp = true;
                break;

            case "insert":
                CrudController.InsertNewSession();
                continueApp = true;
                break;

            case "delete":
                CrudController.DeleteSession();
                continueApp = true;
                break;

            case "update":
                CrudController.UpdateSession();
                continueApp = true;
                break;

            case "report":
                ReportMenu();
                continueApp = true;
                break;

            default:
                Console.WriteLine("Invalid command.\n\nPress any key to continue...");
                Console.ReadKey();
                break;
        }

        return continueApp;
    }

    public static void MainMenu()
    {
        CrudController.CreateTable();
       
        bool continueApp = true;

        while (continueApp)
        {
            string choice = ShowMainMenu();
            continueApp = MainMenuChoice(choice);
        }

        Console.Clear();
        Console.WriteLine("Thank you for using Coding Tracker app\n\nPress any key to exit the app...");
        Console.ReadKey();

    }

    private static string ShowReportMenu()
    {
        Dictionary<string, string> mainMenu = new Dictionary<string, string>() {
            {"main menu", "Main Menu"},
            {"max", "Longest Coding Session" },
            {"min", "Shortest Coding Session"},
            {"avg", "Average Coding Session Time" },
            {"year", "Total Coding Session In Selected Year" },
            {"month", "Total Coding Session In Selected Month" },
            {"week", "Total Coding Session In Selected Week" },
            {"day", "Total Coding Session In Selected Day" },
            {"goal", "Report about coding goal status in last week" }
        };

        TableVisualizationEngine.PrintMenues(mainMenu, "Report Menu");

        Console.Write("Your choice: ");
        string choice = Validations.GetMenuChoice(Console.ReadLine());

        return choice;
    }

    private static void ReportMenuChoice(string choice)
    {
        switch (choice.Trim().ToLower())
        {
            case "main menu":
                MainMenu();
                break;

            case "max":
                Reports.MaxDuration();
                break;

            case "min":
                Reports.MinDuration();
                break;

            case "avg":
                Reports.AvgDuration();
                break;

            case "year":
                Reports.CodingSessionPerYear();
                break;

            case "month":
                Reports.CodingSessionPerMonth();
                break;

            case "week":
                Reports.CodingSessionPerWeek();
                break;

            case "day":
                Reports.CodingSessionPerDay();
                break;

            case "goal":
                CodingSessionGoalHour.CodingHoursGoalCheck();
                break;

            default:
                Console.WriteLine("Invalid command.\n\nPress any key to continue...");
                Console.ReadKey();
                ReportMenu();
                break;
        }
    }

    internal static void ReportMenu()
    {
        string choice = ShowReportMenu();
        ReportMenuChoice(choice);
    }

}