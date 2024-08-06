using Coding_Tracking_Application.Services;
using Spectre.Console;

namespace Coding_Tracking_Application;

public class UserInput
{
    public static void MainMenuOptions()
    {
        AnsiConsole.Markup("[springgreen4]Welcome to the Coding Tracker!\nPlease select from the following options\n\n[/]");
        AnsiConsole.Markup(
            "[turquoise4]\t - Type 0 to Close the application.\n" +
            "\t - Type 1 to View your Coding Session Info.\n" +
            "\t - Type 2 to Create your Coding Tracker.\n" +
            "\t - Type 3 to Delete your Coding Tracker.\n" +
            "\t - Type 4 to Add Coding Session.\n[/]" +
            "\n--------------------------------------------------------\n\n"
            );

        string userMenuInput = Console.ReadLine();

        ValidationServices.ParseUserMenuInput(userMenuInput);
    }

    public static void CreateEntryInput()
    {
        AnsiConsole.Markup("[springgreen4]** LETS GET CODING! **[/]");
        AnsiConsole.Markup("[turquoise4]\n\nPlease enter your first coding start and end time & date (in the following format - 00:00:00 31/07/2024)\n\n[/]");
        AnsiConsole.Markup("Start time & date: ");
        string startTime = Console.ReadLine();

        Thread.Sleep(1000); //adding 1sec pause for UX

        AnsiConsole.Markup("\nEnd time & date: ");
        string endTime = Console.ReadLine();

        Thread.Sleep(1000); //adding 1sec pause for UX

        ValidationServices.ParseUserDateInput(startTime, endTime);

        AnsiConsole.Markup("[green]\n\nYour coding entry has been added.\n\n\n[/]");
        MainMenuOptions();
    }


    public static void AddSessionInput()
    {
        AnsiConsole.Markup("[turquoise4]\n\nPlease enter details of your latest coding session (in the following format - 00:00:00 31/07/2024)\n\n[/]");
        AnsiConsole.Markup("Start time & date: ");
        string startTime = Console.ReadLine();

        Thread.Sleep(1000); //adding 1sec pause for UX

        AnsiConsole.Markup("\nEnd time & date: ");
        string endTime = Console.ReadLine();

        Thread.Sleep(1000); //adding 1sec pause for UX

        ValidationServices.ParseUserDateInput(startTime, endTime);

        AnsiConsole.Markup("[green]\n\nYour coding entry has been added.\n\n\n[/]");
        MainMenuOptions();
    }
}
