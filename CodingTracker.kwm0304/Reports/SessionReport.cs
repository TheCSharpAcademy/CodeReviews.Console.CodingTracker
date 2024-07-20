using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Views;
using Spectre.Console;

namespace CodingTracker.kwm0304.Reports;

public class SessionReport
{
  private readonly CodingSessionService _sessionService;
  private readonly GoalService _goalService;

  private TimeSpan averageTime;
  private TimeSpan totalTime;
  public SessionReport()
  {
    _sessionService = new CodingSessionService();
    _goalService = new GoalService();
  }

  public void CreateReportNumbers(DateRange range)
  {
    List<CodingSession>? sessions = _sessionService?.GetAllCodingSessionsInDateRange(range);
    TableConfigurationEngine.ViewSessions(sessions!);
    if (sessions == null || sessions.Count == 0)
    {
      totalTime = TimeSpan.Zero;
      averageTime = TimeSpan.Zero;
      return;
    }
    TimeSpan total = GetTotalTime(sessions);
    TimeSpan average = GetAverageTime(sessions.Count);
    TableConfigurationEngine.CreateReports(total, average);
  }
  public TimeSpan GetTotalTime(List<CodingSession> sessions)
  {
    totalTime = TimeSpan.Zero;
    foreach (CodingSession session in sessions)
    {
      totalTime += session.SessionLength;
    }
    return totalTime;
  }

  public TimeSpan GetAverageTime(int count)
  {
    return count > 0 ? totalTime / count : TimeSpan.Zero;
  }

  public void HandleDisplayReports()
  {
    DateRange range = SelectionPrompt.ReportsMenu();
    CreateReportNumbers(range);
    AnsiConsole.Markup("[bold yellow]Press any key to return...[/]");
    Console.ReadKey(true);
  }
}