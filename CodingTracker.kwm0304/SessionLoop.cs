using CodingTracker.kwm0304.Models;
using CodingTracker.kwm0304.Services;
using CodingTracker.kwm0304.Views;

namespace CodingTracker.kwm0304;

public class SessionLoop(CodingSessionService service)
{
  private readonly CodingSessionService _service = service;

  public async Task LiveSession()
  {
    CodingSession session = new();
    session.StartSession();
    await TableConfigurationEngine.LiveSessionDisplay();
    session.EndSession();
    _service.CreateSession(session);
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
      HandleDisplayReports();
      break;
    }
  }

    private void HandleDisplayReports()
    {
      string chocie = TableConfigurationEngine.DisplayReportOptions();
    }
}
