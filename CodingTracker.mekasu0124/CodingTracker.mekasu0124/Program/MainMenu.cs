using CodingTracker.Services;
using CodingTracker.Program;

namespace CodingTracker;

public class MainMenu
{
    public static void ShowMenu()
    {
        List<string> menuOptions = new()
        {
            "1 - New Coding Session",
            "2 - Edit Coding Session",
            "3 - Delete Coding Session",
            "4 - View All Sessions",
            "5 - New Goal",
            "6 - Edit Goal",
            "7 - Delete Goal",
            "8 - View All Goals",
            "0 - Exit Program"
        };

        Console.WriteLine("\nWhat Would You Like To Do?");

        foreach (string option in menuOptions)
        {
            Console.WriteLine(option);
        }

        Console.Write("Your Selection: ");
        string? input = Console.ReadLine();
        input = UserValidation.ValidateMenuInput(input);

        switch (input)
        {
            case "1":
                Console.WriteLine("\nHow Would You Like To Record This Session?");
                Console.WriteLine("1 - Manual Time Entry Session");
                Console.WriteLine("2 - Automated Stopwatch Session");
                Console.Write("\nYour Selection: ");

                input = Console.ReadLine();
                input = UserValidation.ValidateSecondaryMenu(input);

                switch (input)
                {
                    case "1":
                        CrudSessionController.NewSessionManual();
                        break;

                    case "2":
                        CrudSessionController.NewSessionStopWatch();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[Error] Couldn't Determine Session Start Type. Relaunching Main Menu");
                        ShowMenu();
                        break;
                }
                break;

            case "2":
                CrudSessionController.EditSession();
                break;

            case "3":
                CrudSessionController.DeleteSession();
                break;

            case "4":
                CrudSessionController.ViewAllCodingSessions();
                break;

            case "5":
                CrudGoalController.NewGoal();
                break;

            case "6":
                CrudGoalController.EditGoal();
                break;

            case "7":
                CrudGoalController.DeleteGoal();
                break;

            case "8":
                CrudGoalController.ViewAllGoals();
                break;

            case "0":
                Console.WriteLine("Exiting Application...");
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid Input. Try Again!");
                ShowMenu();
                break;
        }
    }
}