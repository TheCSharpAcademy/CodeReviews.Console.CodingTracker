using CodingTracker.kwm0304.Models;
using Spectre.Console;

namespace CodingTracker.kwm0304.Views;

public class GoalUI
{
  private readonly GoalService _goalService;
  public const string Back = "Back";
  public GoalUI(GoalService goalService)
  {
    _goalService = goalService;
  }
  private List<Goal> GoalsList(string choice)
  {
    if (choice == "View all goals")
    {
      return _goalService.GetAllGoals();
    }
    else
    {
      return _goalService.GetActiveGoals();
    }
  }

  public bool HandleGoalChoice(string choice)
  {
    if (choice == "Create new goal")
    {
      Goal goal = TableConfigurationEngine.CreateNewGoal();
      _goalService.CreateGoal(goal);
    }
    else if (choice == "View goals")
    {
      string goalOption = SelectionPrompt.ViewGoalsOptions();
      List<Goal> goals = GoalsList(goalOption);
      TableConfigurationEngine.ViewGoals(goals);
      AnsiConsole.Markup("[bold yellow]Press any key to return...[/]");
      Console.ReadKey(true);
      Console.Clear();
    }
    else if (choice == Back)
    {
      return true;
    }
    return false;
  }

  public void HandleGoals()
  {
    while (true)
    {
      string choice = SelectionPrompt.GoalsMenu();
      if (HandleGoalChoice(choice))
      {
        break;
      }
    }
  }
}