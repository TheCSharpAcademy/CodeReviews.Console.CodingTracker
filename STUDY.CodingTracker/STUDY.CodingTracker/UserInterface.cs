using Microsoft.Extensions.Configuration;
using Spectre.Console;
using STUDY.CodingTracker.Controllers;
using STUDY.CodingTracker.Helper;
using STUDY.CodingTracker.Models;
using System.Globalization;

namespace STUDY.CodingTracker;

internal class UserInterface
{
    private CodingSessionController _codingSessionController;

    public UserInterface()
    {
        // Can't move it to CodingSessionController because of "Directory.GetCurrentDirectory()"
        IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false,
            reloadOnChange: true)
        .Build();

        _codingSessionController = new CodingSessionController(config);
    }

    public void MainMenu()
    {
        while (true)
        {
            MainMenuChoice choice = GetMainMenuChoice();

            switch (choice)
            {
                case MainMenuChoice.ViewCodingSessions:
                    FilterChoice filterChoice = AskFilter();
                    int periodNum = AskPeriodNum(filterChoice);
                    OrderChoice orderChoice = AskOrder();

                    DisplayCodingSessionsTable(filterChoice, periodNum, orderChoice);
                    DisplayPressKeyToContinue();
                    break;
                case MainMenuChoice.AddCodingSession:
                    AskUserStopwatchChoice();
                    DisplayPressKeyToContinue();
                    break;
                case MainMenuChoice.DeleteCodingSession:
                    DisplayDeleteCodingSessionUI();
                    DisplayPressKeyToContinue();
                    break;
                case MainMenuChoice.UpdateCodingSession:
                    DisplayUpdateCodingSessionUI();
                    DisplayPressKeyToContinue();
                    break;
                case MainMenuChoice.Exit:
                    AnsiConsole.MarkupLine("Thank you for using the app! See you soon!");
                    return;
            }
        }
    }

    private MainMenuChoice GetMainMenuChoice()
    {
        Console.Clear();

        AnsiConsole.MarkupLine("Welcome to [cyan]Coding Tracker[/]!");
        AnsiConsole.MarkupLine("-----------------------------------");

        MainMenuChoice choice = AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuChoice>()
            .Title("Please select one of the option:")
            .AddChoices(Enum.GetValues<MainMenuChoice>())
            );

        return choice;
    }

    private FilterChoice AskFilter()
    {
        Console.Clear();

        FilterChoice filterChoice = AnsiConsole.Prompt(
            new SelectionPrompt<FilterChoice>()
            .Title("Please select one of the filter:")
            .AddChoices(Enum.GetValues<FilterChoice>())
        );

        return filterChoice;
    }

    private int AskPeriodNum(FilterChoice filterChoice)
    {
        while (true)
        {
            string userInput = "";
            int periodNum = 0;
            (bool correct, int periodNum) validationResult = (false, 0);

            if (filterChoice != FilterChoice.None)
            {
                userInput = UserInput.GetUserFilterPeriod(filterChoice);
            }
            else
            {
                return periodNum;
            }

            switch (filterChoice)
            {
                case FilterChoice.Week:
                    validationResult = Verification.VerifyWeek(userInput);
                    break;
                case FilterChoice.Day:
                    validationResult = Verification.VerifyDay(userInput);
                    break;
                case FilterChoice.Year:
                    validationResult = Verification.VerifyYear(userInput);
                    break;
            }

            if (validationResult.correct)
            {
                return validationResult.periodNum;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Please enter a correct number based on the period you've chosen.[/]");
                DisplayPressKeyToContinue();
            }
        }
    }

    private OrderChoice AskOrder()
    {
        OrderChoice orderChoice = AnsiConsole.Prompt(
            new SelectionPrompt<OrderChoice>()
            .Title("Please select one of the order:")
            .AddChoices(Enum.GetValues<OrderChoice>())
        );

        return orderChoice;
    }

    private void DisplayCodingSessionsTable(FilterChoice filterChoice, int periodNum, OrderChoice orderChoice)
    {
        Console.Clear();

        var codingSessions = _codingSessionController.GetCodingSessions(filterChoice, periodNum, orderChoice);

        var table = new Table().RoundedBorder().BorderColor(Color.Gold1);

        table.AddColumn("[DarkOrange]ID[/]");
        table.AddColumn("[DarkOrange]Start Time[/]");
        table.AddColumn("[DarkOrange]End Time[/]");
        table.AddColumn("[DarkOrange]Duration[/]");

        foreach (CodingSessionModel codingSession in codingSessions)
        {
            table.AddRow(
                $"[yellow]{codingSession.id.ToString()}[/]",
                codingSession.startTime.ToString("dd/MMM/yyyy HH:mm:ss"),
                codingSession.endTime.ToString("dd/MMM/yyyy HH:mm:ss"),
                codingSession.duration.ToString()
            );
        }

        AnsiConsole.Write(table);
    }

    private void AskUserStopwatchChoice()
    {
        StopwatchChoice choice = AnsiConsole.Prompt(
            new SelectionPrompt<StopwatchChoice>()
            .Title("Do you wish to use the stopwatch?")
            .AddChoices(Enum.GetValues<StopwatchChoice>())
            );

        if (choice == StopwatchChoice.Yes)
            ManageStopWatch();
        else
            DisplayManualAddingCodingSessionUI();
    }

    private void ManageStopWatch()
    {
        bool success;
        StopWatch stopwatch = new StopWatch();

        DateTime startTime = DateTime.Now;

        TimeSpan duration = stopwatch.LaunchTimer();

        DateTime endTime = DateTime.Now;

        CodingSessionModel newCodingSession = new CodingSessionModel(startTime, endTime, duration);

        success = _codingSessionController.AddCodingSession(newCodingSession);

        DisplaySuccessResultWhenAddingSession(success);
    }

    private void DisplayManualAddingCodingSessionUI()
    {
        bool success;

        CodingSessionModel newCodingSession = CreateCodingSession();

        success = _codingSessionController.AddCodingSession(newCodingSession);

        DisplaySuccessResultWhenAddingSession(success);
    }

    private void DisplaySuccessResultWhenAddingSession(bool success)
    {
        if (success)
        {
            AnsiConsole.MarkupLine("[green]Coding session added successfully![/]");
            return;
        }

        AnsiConsole.MarkupLine("[red]Coding session was not added...[/]");
    }

    private void DisplayDeleteCodingSessionUI()
    {
        FilterChoice filterChoice = AskFilter();
        int periodNum = AskPeriodNum(filterChoice);
        OrderChoice orderChoice = AskOrder();

        while (true)
        {
            DisplayCodingSessionsTable(filterChoice, periodNum, orderChoice);

            string idToDelete = UserInput.GetUserIDInput("delete");
            var verificationResult = Verification.VerifyId(idToDelete);

            if (!verificationResult.correct)
            {
                DisplayInputIDErrorMessage();
                continue;
            }
            int numberOfRows = _codingSessionController.DeleteCodingSession(verificationResult.id);

            if (numberOfRows > 0)
            {
                AnsiConsole.MarkupLine("[green]Coding session deleted successfully![/]");
                return;
            }

            AnsiConsole.MarkupLine("[red]The id you entered doesn't exist.[/]");
            DisplayPressKeyToContinue();
        }
    }

    private void DisplayUpdateCodingSessionUI()
    {
        FilterChoice filterChoice = AskFilter();
        int periodNum = AskPeriodNum(filterChoice);
        OrderChoice orderChoice = AskOrder();

        while (true)
        {
            DisplayCodingSessionsTable(filterChoice, periodNum, orderChoice);

            string idToUpdate = UserInput.GetUserIDInput("update");
            var verificationResult = Verification.VerifyId(idToUpdate);

            if (!verificationResult.correct)
            {
                DisplayInputIDErrorMessage();
                continue;
            }

            CodingSessionModel updatedCodingSession = CreateCodingSession();

            int numberOfRows = _codingSessionController.UpdateCodingSession(verificationResult.id, updatedCodingSession);

            if (numberOfRows > 0)
            {
                AnsiConsole.MarkupLine("[green]Coding session updated successfully![/]");
                return;
            }

            AnsiConsole.MarkupLine("[red]The id you entered doesn't exist.[/]");
        }
    }

    private void DisplayPressKeyToContinue()
    {
        AnsiConsole.MarkupLine("(Press Any Key to Continue.)");
        Console.ReadKey();
    }

    private CodingSessionModel CreateCodingSession()
    {
        while (true)
        {
            Console.Clear();

            string startTime = UserInput.GetUserDateInput("start");
            var verificationResultStartTime = Verification.VerifyDate(startTime);

            if (!verificationResultStartTime.correct)
            {
                DisplayInputDateErrorMessage();
                continue;
            }

            string endTime = UserInput.GetUserDateInput("end");
            var verificationResultEndTime = Verification.VerifyEndDate(endTime, verificationResultStartTime.date);

            if (!verificationResultEndTime.correct)
            {
                DisplayInputDateErrorMessage();
                continue;
            }

            CodingSessionModel newCodingSession = new CodingSessionModel(verificationResultStartTime.date, verificationResultEndTime.date);

            return newCodingSession;
        }
    }

    private void DisplayInputIDErrorMessage()
    {
        AnsiConsole.MarkupLine("[red]You've inputted the ID in a wrong format. Please try again![/]");
        AnsiConsole.MarkupLine("Good format: [green]It must be a whole positive number.[/]");
        DisplayPressKeyToContinue();
    }

    private void DisplayInputDateErrorMessage()
    {
        AnsiConsole.MarkupLine("[red]You've inputted the date in a wrong format and/or in the wrong order. Please try again![/]");
        AnsiConsole.MarkupLine("Good format: [green]dd/MM/yyyy HH:mm:ss[/] (d = day, M = month, y = year, H = hour, m = minute, s = second)");
        AnsiConsole.MarkupLine("The start time must be [green]earlier[/] than end time.");
        DisplayPressKeyToContinue();
    }
}
