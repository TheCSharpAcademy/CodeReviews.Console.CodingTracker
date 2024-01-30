using Library;
using Spectre.Console;

namespace frockett.CodingTracker;

internal class MenuHandler
{
    private readonly CodingSessionController codingSessionController;
    private readonly DisplayService displayService;
    
    public MenuHandler(CodingSessionController controller, DisplayService display)
    {
        codingSessionController = controller;
        displayService = display;
    }

    public void ShowMainMenu()
    {
        AnsiConsole.Clear();

        string[] menuOptions =
                {"Insert Coding Session", "Modify Coding Session Record",
                "Delete Coding Session Record", "Coding Session Timer",
                "Generate Reports", "DEVELOPER TOOLS: Seed Random Data", "Exit Program",};

        string choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
                            .PageSize(10)
                            .MoreChoicesText("Keep scrolling for more options")
                            .AddChoices(menuOptions));

            /* Before, the menu selection was parsed based on an int.parse of the first character, which was a number. 
            *  But having the numbers could confuse the user, since you can't input a number in the menu.
            *  So instead, menuSelection is the index in the menu array + 1 (the +1 is for ease of human readability) */

        int menuSelection = Array.IndexOf(menuOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                HandleInsertRecord();
                break;
            case 2:
                HandleUpdateRecord();
                break;
            case 3:
                HandleDeleteRecord();
                break;
            case 4:
                HandleStartCodingSession();
                break;
            case 5:
                HandleReportSubmenu();
                break;
            case 6:
                HandleSeedData();
                break;
            case 7:
                Environment.Exit(0);
                break;
        }
    }

    private void HandleSeedData()
    {
        if (!AnsiConsole.Confirm("Are you sure you want to seed random data? WARNING: only suitable for testing"))
        {
            AnsiConsole.Markup("\nOkay! Press any key to return to the main menu\n");
            Console.ReadKey(true);
            ShowMainMenu();
        }
        else
        {
            int iterations = AnsiConsole.Ask<int>("\nHow many random sessions would you like to generate?");
            while (iterations <= 0)
            {
                iterations = AnsiConsole.Ask<int>("Invalid choice, you can't seed a negative number. Enter a positive integer.");
            }
            codingSessionController.SeedRandomSessions(iterations);
            ShowMainMenu();
        }
    }

    private void HandleInsertRecord() 
    {
        codingSessionController.InsertCodingSession();
        ShowMainMenu();
    }

    private void HandleDeleteRecord()
    {
        displayService.PrintSessionList(codingSessionController.FetchAllSessionHistory());
        codingSessionController.DeleteCodingSession();
        ShowMainMenu();
    }

    private void HandleUpdateRecord()
    {
        displayService.PrintSessionList(codingSessionController.FetchAllSessionHistory());
        codingSessionController.UpdateCodingSession();
        ShowMainMenu();
    }

    private void HandleStartCodingSession()
    {
        var stopwatch = displayService.DisplayStopwatch(codingSessionController.StartCodingSession());
        codingSessionController.StopCodingSession(stopwatch);
        AnsiConsole.Markup("\n[green]Session recorded, please press enter to return to the main menu...[/]");
        Console.ReadLine();
        Console.ReadLine(); // this consumes any additional input. It's hack-ish, but previously ShowMainMenu was getting called immediately, with no time for the user to read the message. Idk why
        ShowMainMenu();
    }

    private void HandleReportSubmenu()
    {
        string[] reportMenuOptions =
                {"Display All Reports", "Display Sessions by Month", "Display Monthly Averages",
                "Display Goal Report - UNDER CONSTRUCTION", "Return to Main Menu",};

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Which operation would you like to perform? Use [green]arrow[/] and [green]enter[/] keys to make a selection.")
            .PageSize(10)
            .MoreChoicesText("Keep scrolling for more options")
            .AddChoices(reportMenuOptions));

        int menuSelection = Array.IndexOf(reportMenuOptions, choice) + 1;

        switch (menuSelection)
        {
            case 1:
                displayService.PrintSessionList(codingSessionController.FetchAllSessionHistory());
                ShowMainMenu();
                break;
            case 2:
                displayService.PrintSessionList(codingSessionController.FetchCustomSessionHistory(false));
                ShowMainMenu();
                break;
            case 3:
                displayService.PrintMonthlyAverages(codingSessionController.FetchCustomSessionHistory(true));
                ShowMainMenu();
                break;
            case 4:
                AnsiConsole.Markup("Sorry, this feature isn't ready yet! The developer hasn't met his goal either!!");
                Console.ReadLine();
                ShowMainMenu();
                break;
            case 5: 
                ShowMainMenu();
                break;
            default:
                AnsiConsole.Markup("[red]Invalid input![/]");
                break;
        }
    }
}
