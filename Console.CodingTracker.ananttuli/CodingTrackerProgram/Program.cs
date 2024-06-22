namespace CodingTrackerProgram;

using Spectre.Console;
using CodingTrackerProgram.Database;
using CodingTrackerProgram.Services;

public class Program
{
    public static void Main()
    {
        bool keepAppRunning = true;

        Connection.Init();
        ShowWelcomeScreen();

        do
        {
            keepAppRunning = StartApp();
        } while (keepAppRunning == true);

        ShowExitMessage();
    }

    public static void ShowExitMessage()
    {
        AnsiConsole.MarkupLine("\n\n\t[green]Thanks[/] for using [red]Coding[/][blue]Tracker[/]");
    }

    public static void ShowWelcomeScreen()
    {
        AnsiConsole.MarkupLine("\n\n[green]Welcome[/] to [red]Coding[/][blue]Tracker[/]");
    }

    public static bool StartApp()
    {
        bool isAppRunning = true;

        string option = Menu.ShowMenu();

        Console.Clear();

        switch (option)
        {
            case Menu.CREATE_SESSION:
                CodingSessionService.CreateOrUpdateSession();
                break;
            case Menu.VIEW_SESSIONS:
                CodingSessionService.ViewSessions();
                break;
            case Menu.UPDATE_SESSION:
                CodingSessionService.ViewSessions();
                CodingSessionService.CreateOrUpdateSession(updateMode: true);
                break;
            case Menu.DELETE_SESSION:
                CodingSessionService.ViewSessions();
                CodingSessionService.DeleteSession();
                break;
            case Menu.EXIT_PROGRAM:
                isAppRunning = false;
                break;
            default:
                AnsiConsole.WriteLine("Please select a valid menu option.");
                break;
        }

        if (isAppRunning)
        {
            AnsiConsole.MarkupLine("[green]Press any key to go back to the menu[/]");
            Console.ReadKey();
            Console.Clear();
        }

        return isAppRunning;
    }

}