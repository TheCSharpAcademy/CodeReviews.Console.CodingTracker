using CodingTracker.Models;
using Spectre.Console;
using System.Globalization;
using static CodingTracker.Models.Enums;

namespace CodingTracker;

internal class UserInput
{
    CodingController codingController = new();
    GoalController goalController = new();
    internal void MainMenu()
    {
        bool repeatMenu = true;
        while (repeatMenu)
        {
            repeatMenu = false;
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<MenuSelection>()
                .Title("Welcome to [green]Coding tracker[/]\nWhat would you like to do?")
                .PageSize(10)
                .AddChoices(MenuSelection.LiveSession,
                            MenuSelection.ManageRecords,
                            MenuSelection.ViewAllRecords,
                            MenuSelection.ViewReports,
                            MenuSelection.SetGoal,
                            MenuSelection.ViewGoals,
                            MenuSelection.CloseApplication)
                            );

            switch (selection)
            {
                case MenuSelection.LiveSession:
                    ProcessLiveSession();
                    MainMenu();
                    break;
                case MenuSelection.ViewAllRecords:
                    codingController.Get();
                    AnsiConsole.Write("\nPress any key to continue... ");
                    Console.ReadKey();
                    MainMenu();
                    break;
                case MenuSelection.ManageRecords:
                    RecordsMenu();
                    MainMenu();
                    break;
                case MenuSelection.ViewReports:
                    ReportsMenu();
                    MainMenu();
                    break;
                case MenuSelection.SetGoal:
                    ProcessAddGoal();
                    MainMenu();
                    break;
                case MenuSelection.ViewGoals:
                    ProcessGoals();
                    MainMenu();
                    break;
                case MenuSelection.CloseApplication:
                    if (AnsiConsole.Confirm("Are you sure you want to exit?"))
                    {
                        Console.WriteLine("\nGoodbye!");
                    }
                    else
                    {
                        repeatMenu = true;
                    }
                    break;
            }
        }
    }

    internal void RecordsMenu()
    {
        bool repeat = true;

        while (repeat)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<RecordsSelection>()
                .Title("Select from the options below:")
                .PageSize(10)
                .AddChoices(RecordsSelection.AddRecord,
                            RecordsSelection.DeleteRecord,
                            RecordsSelection.UpdateRecord,
                            RecordsSelection.MainMenu)
                );
            switch (selection)
            {
                case RecordsSelection.AddRecord:
                    ProcessAdd();
                    break;
                case RecordsSelection.DeleteRecord:
                    ProcessDelete();
                    break;
                case RecordsSelection.UpdateRecord:
                    ProcessUpdate();
                    break;
                case RecordsSelection.MainMenu:
                    repeat = false;
                    break;
            }
        }
    }

    internal void UpdateMenu(Coding coding)
    {
        bool updating = true;
        while (updating)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<UpdatingSelection>()
                .Title("Select from the options below:")
                .PageSize(10)
                .AddChoices(UpdatingSelection.UpdateStartTime,
                            UpdatingSelection.UpdateEndTime,
                            UpdatingSelection.SaveChanges,
                            UpdatingSelection.MainMenu)
                );
            switch (selection)
            {
                case UpdatingSelection.UpdateStartTime:
                    var newStart = Helpers.GetDateTimeInput("Please insert new start date and time (format: dd-MM-yy H:mm):");
                    coding.StartTime = DateTime.ParseExact(newStart, "dd-MM-yy H:mm", new CultureInfo("en-US"));
                    break;
                case UpdatingSelection.UpdateEndTime:
                    var newEnd = Helpers.GetDateTimeInput("Please insert new end date and time (format: dd-MM-yy H:mm):");

                    while (!Helpers.ValidateDateTime(coding.StartTime.ToString("dd-MM-yy H:mm"), newEnd))
                    {
                        AnsiConsole.MarkupLine("\n[red]Invalid input! End time can't be before start time![/]\n");
                        newEnd = Helpers.GetDateTimeInput("Please insert a valid end date and time (format: dd-MM-yy H:mm): ");
                    }

                    coding.EndTime = DateTime.ParseExact(newEnd, "dd-MM-yy H:mm", new CultureInfo("en-US"));
                    break;
                case UpdatingSelection.SaveChanges:
                    codingController.Update(coding);
                    updating = false;
                    break;
                case UpdatingSelection.MainMenu:
                    updating = false;
                    break;
            }
        }
    }

    internal void ReportsMenu()
    {
        bool repeat = true;

        while (repeat)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<ReportSelection>()
                .Title("Select from the options below:")
                .PageSize(10)
                .AddChoices(ReportSelection.Weekly,
                            ReportSelection.Monthly,
                            ReportSelection.Yearly,
                            ReportSelection.MainMenu)
                );
            switch (selection)
            {
                case ReportSelection.Weekly:
                    ProcessReport(DateTime.Now.AddDays(-7), "WEEKLY CODING REPORTS");
                    break;
                case ReportSelection.Monthly:
                    ProcessReport(DateTime.Now.AddMonths(-1), "MONTHLY CODING REPORTS");
                    break;
                case ReportSelection.Yearly:
                    ProcessReport(DateTime.Now.AddYears(-1), "YEARLY CODING REPORTS");
                    break;
                case ReportSelection.MainMenu:
                    repeat = false;
                    break;
            }
        }
    }

    private void ProcessAdd()
    {
        var startTime = Helpers.GetDateTimeInput("Please insert the start date and time (format: dd-MM-yy H:mm): ");
        var endTime = Helpers.GetDateTimeInput("Please insert the end date and time (format: dd-MM-yy H:mm): ");

        while (!Helpers.ValidateDateTime(startTime, endTime))
        {
            AnsiConsole.MarkupLine("\n[red]Invalid input! End time can't be before start time![/]\n");
            endTime = Helpers.GetDateTimeInput("Please insert a valid end date and time (format: dd-MM-yy H:mm): ");
        }

        Coding coding = new Coding();

        coding.StartTime = DateTime.ParseExact(startTime, "dd-MM-yy H:mm", new CultureInfo("en-US"), DateTimeStyles.None);
        coding.EndTime = DateTime.ParseExact(endTime, "dd-MM-yy H:mm", new CultureInfo("en-US"), DateTimeStyles.None);

        codingController.Post(coding);
    }

    private void ProcessDelete()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        codingController.Get();

        int id = Helpers.GetNumberInput("Please type the Id of the record you want to delete:");

        var coding = codingController.GetById(id);

        if (coding is null)
        {
            AnsiConsole.Write($"\nRecord with Id {id} doesn't exist! Press any key to continue...");
            Console.ReadKey();
            ProcessDelete();
        }
        else
        {
            codingController.Delete(coding);
        }
    }

    private void ProcessUpdate()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        codingController.Get();

        int id = Helpers.GetNumberInput("Please type the Id of the record you want to update:");

        var coding = codingController.GetById(id);

        if (coding is null)
        {
            AnsiConsole.Write($"\nRecord with Id {id} doesn't exist! Press any key to continue...");
            Console.ReadKey();
            ProcessUpdate();
        }
        else
        {
            UpdateMenu(coding);
        }
    }

    private void ProcessLiveSession()
    {
        AnsiConsole.Write("Press any key to start a live session...");
        Console.ReadKey();
        AnsiConsole.MarkupLine("\n\n[green]Live session started![/]");

        Coding coding = new();
        coding.StartTime = DateTime.Now;

        AnsiConsole.Write("Press any key to stop the live session...");
        Console.ReadKey();
        AnsiConsole.MarkupLine("\n\n[red]Live session ended![/]");

        coding.EndTime = DateTime.Now;

        codingController.Post(coding);
    }

    private void ProcessReport(DateTime period, string title)
    {
        var tableData = codingController.GetByTimePeriod(period);

        var order = Helpers.SelectOrdering();

        if (order == "Ascending")
        {
            var asc = tableData?.OrderBy(s => s.StartTime).ToList();
            TableVisualisation.ShowCodingTable(asc!, title);
        }
        else
        {
            var desc = tableData?.OrderByDescending(s => s.StartTime).ToList();
            TableVisualisation.ShowCodingTable(desc!, title);
        }

        TimeSpan totalDuration = new TimeSpan();

        foreach (var c in tableData!)
        {
            totalDuration += c.Duration;
        }

        AnsiConsole.MarkupLineInterpolated($"\nTotal coding duration: {(int)totalDuration.TotalHours} hours and {totalDuration.Minutes} minutes");

        var average = totalDuration.Ticks / tableData.Count;
        var averageTime = new TimeSpan(average);
        AnsiConsole.MarkupLineInterpolated($"Average coding duration per session: {averageTime.Hours} hours and {averageTime.Minutes} minutes");

        AnsiConsole.Write("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void ProcessAddGoal()
    {
        var startDate = Helpers.GetDateInput("Please insert the goal start date (format: dd-MM-yy):");
        var endDate = Helpers.GetDateInput("Please insert the goal end date (format: dd-MM-yy):");

        while (!Helpers.ValidateDate(startDate, endDate))
        {
            AnsiConsole.MarkupLine("\n[red]Invalid input! End date can't be before start date![/]\n");
            endDate = Helpers.GetDateInput("Please insert a valid goal end date (format: dd-MM-yy): ");
        }

        var goalAmount = Helpers.GetNumberInput("Please insert the amount of coding hours for the goal:");

        Goal goal = new Goal();

        goal.StartDate = DateTime.ParseExact(startDate, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None);
        goal.EndDate = DateTime.ParseExact(endDate, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None);
        goal.Amount = goalAmount;

        goalController.Post(goal);
    }

    private void ProcessGoals()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        var tableData = goalController.GetAll();

        TableVisualisation.ShowGoalsTable(tableData!, "GOAL RECORDS");

        var id = Helpers.GetNumberInput("Enter the goal Id for which you want to see the progress:");

        var goal = goalController.GetById(id);

        if (goal is null)
        {
            AnsiConsole.Write($"\nGoal with Id {id} doesn't exist! Press any key to continue...");
            Console.ReadKey();
            ProcessGoals();
        }
        else
        {
            var codingRecords = codingController.GetByTimePeriod(goal.StartDate);

            TimeSpan codingHours = new TimeSpan();

            foreach (var c in codingRecords!)
            {
                codingHours += c.Duration;
            }

            var remainingHours = goal.Amount - codingHours.TotalHours;

            if (remainingHours > 0)
            {
                AnsiConsole.MarkupLineInterpolated($"\n[green]{goal.StartDate.ToString("dd-MM-yy")} - {goal.EndDate.ToString("dd-MM-yy")}[/]");

                AnsiConsole.Write(new BreakdownChart()
                    .Width(60)
                    .UseValueFormatter(v => v.ToString("N0"))
                    .AddItem("Progress(h)", codingHours.TotalHours, Color.Blue)
                    .AddItem("Remaining(h)", remainingHours, Color.Red));

                TimeSpan daysToReachGoal = new TimeSpan();
                daysToReachGoal = goal.EndDate - DateTime.Now;
                var hoursPerDay = remainingHours / daysToReachGoal.TotalDays;

                AnsiConsole.WriteLine($"\nYou need to code for {hoursPerDay:N0} hours per day to reach your goal of {goal.Amount}h until {goal.EndDate.ToString("dd-MM-yy")}");
                AnsiConsole.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                AnsiConsole.WriteLine($"You have finished the goal with the Id {goal.Id}!");
                AnsiConsole.Write("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
