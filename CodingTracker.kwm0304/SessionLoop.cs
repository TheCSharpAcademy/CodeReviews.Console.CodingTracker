using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Reports;
using CodingTracker.kwm0304.Repositories;
using CodingTracker.kwm0304.Services;
using CodingTracker.kwm0304.Views;
using Spectre.Console;

namespace CodingTracker.kwm0304;

public class SessionLoop
{
  private readonly CodingSessionService _sessionService;
  private readonly GoalService _goalService;
  public SessionLoop()
  {
    _sessionService = new CodingSessionService();
    _goalService = new GoalService();
  }

  public void OnStart()
  {
    while (true)
    {
      AnsiConsole.WriteLine(Global.header);
      string choice = SelectionPrompt.MainMenu();
      HandleChoice(choice);
    }
  }

  private void HandleChoice(string choice)
  {
    Console.Clear();
    switch (choice)
    {
      case "Sessions":
        HandleSessions();
        break;
      case "Goals":
        HandleGoals();
        break;
      case "Reports":
        HandleDisplayReports();
        break;
      case "Exit":
        AnsiConsole.WriteLine("Goodbye!");
        break;
      default:
        break;
    }
  }

  private void HandleSessions()
  {
    Console.Clear();
    string choice = SelectionPrompt.CodingSessionsMenu();
    HandleSessionChoice(choice);
  }

  private void HandleSessionChoice(string choice)
  {
    Console.Clear();
    if (choice == "Create new session")
    {
      LiveSession();
    }
    else if (choice == "View past sessions")
    {
      List<CodingSession> sessions = _sessionService.GetAllCodingSessions() ?? [];
      if (sessions.Count > 0)
      {
        TableConfigurationEngine.ViewSessions(sessions);
      }
      else
      {
        AnsiConsole.WriteLine("[red]No sessions available to view[/]");
      }
    }
  }

  private void HandleGoals()
  {
    string choice = SelectionPrompt.GoalsMenu();
    HandleGoalChoice(choice);
  }

  private void HandleGoalChoice(string choice)
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
    }
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

  public void LiveSession()
  {
    CodingSession session = new();
    session.StartSession();
    TableConfigurationEngine.LiveSessionDisplay(session);
    session.EndSession();
    _sessionService.CreateCodingSession(session);
    UpdateGoalsOnComplete(session);
    AnsiConsole.Markup("[bold yellow]Press any key to return to the main menu...[/]");
    Console.ReadKey(true);
  }

  public void UpdateGoalsOnComplete(CodingSession session)
  {
    var activeGoals = _goalService.GetActiveGoals();

    if (activeGoals != null && activeGoals.Count > 0)
    {
      foreach (var goal in activeGoals)
      {
        goal.AddSession(session);
        _goalService.SaveGoal(goal);
      }
      Console.WriteLine("Goals updated successfully.");
    }
    else
    {
      AnsiConsole.WriteLine("No goals to update.");
    }
  }

  private static void HandleDisplayReports()
  {
    DateRange range = SelectionPrompt.ReportsMenu();
    SessionReport report = new();
    report.CreateReportNumbers(range);
  }
}