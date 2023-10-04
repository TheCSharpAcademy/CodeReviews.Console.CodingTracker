using CodingTracker.Services;
using CodingTracker.Program;

namespace CodingTracker;

public class MainMenu
{
    public static void ShowMenu()
    {
        List<string> menuOptions = new()
        {
            "1 - New Coding Session", // works
            "2 - Edit Coding Session", // works
            "3 - Delete Coding Session",
            "4 - View All Sessions",
            "5 - New Goal",
            "6 - Edit Goal",
            "7 - Delete Goal",
            "8 - View All Goals",
            "0 - Exit Program"
        };

        Console.WriteLine($"Welcome To Your Coding Tracker!");
        Console.WriteLine($"Current Date: {Helpers.GetDateTime()}");
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
                CRUDController.NewSession();
                break;

            case "2":
                CRUDController.EditSession();
                break;

            case "3":
                break;

            case "4":
                break;

            case "5":
                break;

            case "6":
                break;

            case "7":
                break;

            case "8":
                break;

            case "0":
                Console.WriteLine("Exiting Application...");
                Environment.Exit(0);
                break;
        }
    }
}