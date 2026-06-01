using Microsoft.Extensions.Configuration;
using Spectre.Console;
using STUDY.CodingTracker.Controllers;
using STUDY.CodingTracker.Helper;
using STUDY.CodingTracker.Models;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    if (filterChoice == FilterChoice.Return) break;

                    DateTime periodDate = AskPeriodDate(filterChoice);
                    if (periodDate == new DateTime(1)) break;

                    OrderChoice orderChoice = AskOrder();
                    if (orderChoice == OrderChoice.ReturnToMainMenu) break;

                    DisplayCodingSessionsTable(filterChoice, periodDate, orderChoice);
                    DisplayPressKeyToContinue();
                    break;
                case MainMenuChoice.AddCodingSession:
                    AskUserStopwatchChoice();
                    break;
                case MainMenuChoice.DeleteCodingSession:
                    DisplayDeleteCodingSessionUI();
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

    private DateTime AskPeriodDate(FilterChoice filterChoice)
    {
        while (true)
        {
            string userInput = "";
            DateTime periodDate = new DateTime(0);
            (bool correct, DateTime periodDate) validationResult = (false, new DateTime(0));

            if (filterChoice != FilterChoice.None)
            {
                userInput = UserInput.GetUserFilterPeriod(filterChoice);
            }
            else
            {
                return periodDate;
            }

            if (userInput == "-1") return new DateTime(1);

            validationResult = Verification.VerifyPeriodDate(userInput);

            if (validationResult.correct)
            {
                return validationResult.periodDate;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Please enter a correct date based on the period you've chosen. (Format : dd/MM/yyyy)[/]");
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

    private void DisplayCodingSessionsTable(FilterChoice filterChoice, DateTime periodDate, OrderChoice orderChoice)
    {
        Console.Clear();

        var codingSessions = _codingSessionController.GetCodingSessions(filterChoice, periodDate, orderChoice);

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
        else if (choice == StopwatchChoice.No)
            DisplayManualAddingCodingSessionUI();
        else
            return;

        DisplayPressKeyToContinue();
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

        if (newCodingSession.startTime == new DateTime(0)) return;

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
        if (filterChoice == FilterChoice.Return) return;

        DateTime periodDate = AskPeriodDate(filterChoice);
        if (periodDate == new DateTime(1)) return;

        OrderChoice orderChoice = AskOrder();
        if (orderChoice == OrderChoice.ReturnToMainMenu) return;

        while (true)
        {
            DisplayCodingSessionsTable(filterChoice, periodDate, orderChoice);

            string idToDelete = UserInput.GetUserIDInput("delete");
            if (idToDelete == "-1") return;

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
        if (filterChoice == FilterChoice.Return) return;

        DateTime periodDate = AskPeriodDate(filterChoice);
        if (periodDate == new DateTime(1)) return;

        OrderChoice orderChoice = AskOrder();
        if (orderChoice == OrderChoice.ReturnToMainMenu) return;

        while (true)
        {
            DisplayCodingSessionsTable(filterChoice, periodDate, orderChoice);

            string idToUpdate = UserInput.GetUserIDInput("update");
            if (idToUpdate == "-1") return;
            var verificationResult = Verification.VerifyId(idToUpdate);

            if (!verificationResult.correct)
            {
                DisplayInputIDErrorMessage();
                continue;
            }

            CodingSessionModel updatedCodingSession = CreateCodingSession();

            if (updatedCodingSession.startTime == new DateTime(0)) return;

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
            DateTime dateReturn = new DateTime(0);
            DateTime.TryParseExact("0/0/0 0:0", "dd/MM/yyyy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dateReturn);
            CodingSessionModel codingSessionReturn = new CodingSessionModel(dateReturn, dateReturn);

            string startTime = UserInput.GetUserDateInput("start");
            if (startTime == "-1") return codingSessionReturn; 
            var verificationResultStartTime = Verification.VerifyDate(startTime);

            if (!verificationResultStartTime.correct)
            {
                DisplayInputDateErrorMessage();
                continue;
            }

            string endTime = UserInput.GetUserDateInput("end");
            if (endTime == "-1") return codingSessionReturn;
            var verificationResultEndTime = Verification.VerifyEndDate(endTime, verificationResultStartTime.date);

            if (!verificationResultEndTime.correct)
            {
                DisplayInputDateErrorMessage();
                continue;
            }

            CodingSessionModel newCodingSession = new CodingSessionModel(verificationResultStartTime.date, verificationResultEndTime.date);;
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
