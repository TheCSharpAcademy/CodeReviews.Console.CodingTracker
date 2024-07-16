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
    string choice = TableConfigurationEngine.MainMenu();
    HandleChoice(choice);
  }

  private async void HandleChoice(string choice)
  {
    Console.Clear();
    switch (choice)
    {
      case "Start a new session":
        await LiveSession();
        break;
      case "View past sessions":
        GenerateReports();
        break;
      case "Generate reports":
        HandleDisplayReports();
        break;
      case "Create a new goal":
        HandleCreateGoal();
        break;
      case "View current goals":
        HandleViewGoals();
        break;
      case "Exit":
        AnsiConsole.WriteLine("Goodbye!");
        break;
      default:
        break;
    }
  }

    private void HandleViewGoals()
    {
        throw new NotImplementedException();
    }

    private void HandleCreateGoal()
  {
    throw new NotImplementedException();
  }

  private void GenerateReports()
  {
    throw new NotImplementedException();
  }

  public async Task LiveSession()
  {
    CodingSession session = new();
    session.StartSession();
    await TableConfigurationEngine.LiveSessionDisplay();
    session.EndSession();
    _sessionService.CreateCodingSession(session);
    UpdateGoalsOnComplete(session);

  }
  public void UpdateGoalsOnComplete(CodingSession session)
  {
    var activeGoals = _goalService.GetActiveGoals();
    foreach (var goal in activeGoals)
    {
      goal.AddSession(session);
      _goalService.SaveGoal(goal);
    }
  }

  private static void HandleDisplayReports()
  {
    DateRange range = TableConfigurationEngine.DisplayReportOptions();
    SessionReport report = new();
    report.CreateReportNumbers(range);
  }
}
