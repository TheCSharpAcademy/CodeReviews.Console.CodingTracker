using CodingTracker.DAO;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;
using System.Data;
using System.Xml.Serialization;

namespace CodingTracker.Application;

public class AppGoalManager
{
    private InputHandler _inputHandler;
    CodingGoalDAO _codingGoalDAO;
    private bool _running;

    public AppGoalManager(CodingGoalDAO codingGoalDAO, InputHandler inputHandler)
    {
        _codingGoalDAO = codingGoalDAO;
        _inputHandler = inputHandler;
        _running = true;
    }

    public void Run()
    {
        while (_running)
        {
            AnsiConsole.Clear();
            PromptForSessionAction();
        }
    }

    private void PromptForSessionAction()
    {
        var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an option:")
                .PageSize(10)
                .AddChoices(Enum.GetNames(typeof(ManageGoalsMenuOptions))
                .Select(Utilities.SplitCamelCase)));

        ExecuteSelectedOption(selectedOption);
    }

    private void ExecuteSelectedOption(string option)
    {
        switch (Enum.Parse<ManageGoalsMenuOptions>(option.Replace(" ", "")))
        {
            case ManageGoalsMenuOptions.ShowProgress:
                HandleShowProgressAction();
                break;
            case ManageGoalsMenuOptions.SetNewGoal:
                HandleSetNewGoalAction();
                break;
            case ManageGoalsMenuOptions.DeleteGoal:
                DeleteSpecificGoal();
                break;
            case ManageGoalsMenuOptions.DeleteAllGoals:
                DeleteAllGoals();
                break;
            case ManageGoalsMenuOptions.ReturnToMainMenu:
                _running = false;
                break;
        }
    }

    private void HandleShowProgressAction()
    {
        ShowGoalProgress();
        _inputHandler.PauseForContinueInput();
    }

    private void ShowGoalProgress()
    {
        List<CodingGoalModel> codingGoals = _codingGoalDAO.GetInProgressCodingGoals();

        if (codingGoals.Count == 0)
        {
            Utilities.DisplayWarningMessage("No current goals in progress.");
            return;
        }

        List<(BreakdownChart, CodingGoalModel)> breakdownCharts = BuildCodingGoalsBreakdownCharts(codingGoals);
        PrintBreakDownCharts(breakdownCharts);
    }

    private void PrintBreakDownCharts(List<(BreakdownChart, CodingGoalModel)> charts)
    {
        AnsiConsole.MarkupLine("[bold]Coding Goals in Progress[/]");
        Utilities.PrintNewLines(2);

        foreach (var chart in charts)
        {
            string chartHeader = $"[darkslategray2]  {chart.Item2.Description}  [/]";
            float hoursCurrentProgress, hoursTarget;
            chart.Item2.GetProgressAsIntervals(out hoursCurrentProgress, out hoursTarget);
            string footerText = $"[dim]Current: [lightgreen_1]{hoursCurrentProgress.ToString("F2")}[/] hrs, Target: [yellow]{hoursTarget.ToString("F2")}[/] hrs[/]";

            var panelProgessBar = new Panel(chart.Item1)
                .Header(chartHeader)
                .HeaderAlignment(Justify.Left)
                .BorderColor(Color.Grey)
                .Padding(1, 1, 1, 1);

            AnsiConsole.Write(panelProgessBar);
            AnsiConsole.MarkupLine(footerText);
            Utilities.PrintNewLines(2);

        }

        Utilities.PrintNewLines(2);
    }

    private List<(BreakdownChart, CodingGoalModel)> BuildCodingGoalsBreakdownCharts(List<CodingGoalModel> codingGoals)
    {
        List<(BreakdownChart, CodingGoalModel)> breakdownCharts = new List<(BreakdownChart, CodingGoalModel)>();

        foreach (var goal in codingGoals)
        {
            Color progressBarColor = Color.LightGreen;
            Color targetDurationBarColor = Color.Grey;

            float hoursCurrentProgress;
            float hoursTarget;

            goal.GetProgressAsIntervals(out hoursCurrentProgress, out hoursTarget);

            BreakdownChart breakdownChart = new BreakdownChart()
                                .Width((int)(Console.WindowWidth * 0.65))
                                .AddItem("Current (hrs): ", hoursCurrentProgress, progressBarColor)
                                .AddItem("Target (hrs): ", hoursTarget- hoursCurrentProgress, targetDurationBarColor)
                                .ShowTags(false);

            breakdownCharts.Add(new (breakdownChart, goal));
        }

        return breakdownCharts;
    }

    private void DeleteSpecificGoal()
    {
        List<CodingGoalModel> codingGoals = _codingGoalDAO.GetAllGoalRecords();
        if (codingGoals.Count == 0)
        {
            Utilities.DisplayWarningMessage("No goals found!");
            _inputHandler.PauseForContinueInput();
            return;
        }

        var selectedGoal = _inputHandler.PromptForGoalSelection(codingGoals, "Select a goal to delete:");

        bool goalDeletedResult = _codingGoalDAO.DeleteGoal(selectedGoal.Id);
        if (goalDeletedResult == true)
        {
            Utilities.DisplaySuccessMessage("Goal successfully deleted!");
        }
        else
        {
            Utilities.DisplayWarningMessage("Goal was not deleted.");
        }

        _inputHandler.PauseForContinueInput();
    }


    private void HandleSetNewGoalAction()
    {
        CodingGoalModel goalToInsert = CreateNewGoal();
        InsertNewGoal(goalToInsert);
        _inputHandler.PauseForContinueInput();
    }

    private void InsertNewGoal(CodingGoalModel goalToInsert)
    {
        int newRecordID = _codingGoalDAO.InsertNewGoal(goalToInsert);
        if (newRecordID != -1)
        {
            string successMessage = $"[green]Session successfully logged with Id [[ {newRecordID} ]]![/]";
            Utilities.DisplaySuccessMessage(successMessage);
        }
        else
        {
            Utilities.DisplayWarningMessage("Failed to log the session. Please try again or check the system logs.");
        }
    }   

    private (string, TimeSpan) PromptForGoalProperties()
    {
        string goalDescription = _inputHandler.PromptForGoalDescription("Enter a description for your goal: ");
        int hours = _inputHandler.PromptForPositiveInteger("Enter the number of hours for your goal: ");
        TimeSpan targetDuration = TimeSpan.FromHours(hours);

        return (goalDescription, targetDuration);
    }

    private CodingGoalModel CreateNewGoal()
    {
        (string description, TimeSpan targetDuration) = PromptForGoalProperties();

        return new CodingGoalModel(targetDuration, description);
    }

    private void DeleteAllGoals()
    {
        bool goalDeletedResult = _codingGoalDAO.DeleteAllGoals();
        if (goalDeletedResult == true)
        {
            Utilities.DisplaySuccessMessage("Goals successfully deleted!");
        }
        else
        {
            Utilities.DisplayWarningMessage("No goals were deleted. (The table may have been empty).");
        }


        _inputHandler.PauseForContinueInput();
    }
}
