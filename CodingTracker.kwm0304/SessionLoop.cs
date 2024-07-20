using CodingTracker.kwm0304;
using CodingTracker.kwm0304.Reports;
using CodingTracker.kwm0304.Views;
using Spectre.Console;

public class SessionLoop
{
  private readonly CodingSessionService _sessionService;
  private readonly GoalService _goalService;
  private readonly GoalUI _goalUi;
  private readonly SessionUI _sessionUi;
  private readonly SessionReport report = new();

  public SessionLoop()
  {
    _sessionService = new CodingSessionService();
    _goalService = new GoalService();
    _goalUi = new GoalUI(_goalService);
    _sessionUi = new SessionUI(_sessionService, _goalService);
  }

  public void OnStart()
  {
    while (true)
    {
      Console.Clear();
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
        _sessionUi.HandleSessions();
        break;
      case "Goals":
        _goalUi.HandleGoals();
        break;
      case "Reports":
        report.HandleDisplayReports();
        break;
      case "Exit":
        AnsiConsole.WriteLine("Goodbye!");
        Environment.Exit(0);
        break;
      default:
        break;
    }
  }
}