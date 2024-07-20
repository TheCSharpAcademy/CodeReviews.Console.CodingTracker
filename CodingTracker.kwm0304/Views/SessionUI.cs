using CodingTracker.kwm0304.Models;
using Spectre.Console;

namespace CodingTracker.kwm0304.Views;

public class SessionUI
{
  private readonly CodingSessionService _sessionService;
  private readonly GoalService _goalService;
  public const string Edit = "Edit";
  public const string Delete = "Delete";
  public const string View = "View";
  public const string Create = "Create";
  public const string Back = "Back";

  public SessionUI(CodingSessionService sessionService, GoalService goalService)
  {
    _sessionService = sessionService;
    _goalService = goalService;
  }

  public void HandleSessions()
  {
    while (true)
    {
      Console.Clear();
      string choice = SelectionPrompt.CodingSessionsMenu();
      if (choice == Back) return;

      switch (choice)
      {
        case View:
        case Edit:
        case Delete:
          HandleSessionChoice(choice);
          break;
        case Create:
          LiveSession();
          break;
      }
    }
  }

  public void HandleSessionChoice(string choice)
  {
    List<CodingSession> sessions = _sessionService.GetAllCodingSessions() ?? [];

    if (choice == View)
    {
      TableConfigurationEngine.ViewSessions(sessions);
      AnsiConsole.Markup("[bold yellow]Press any key to return...[/]");
      Console.ReadKey(true);
    }
    else if (choice == Edit)
    {
      EditSession(sessions);
    }
    else if (choice == Delete)
    {
      DeleteSession(sessions);
    }
  }

  private void EditSession(List<CodingSession> sessions)
  {
    while (true)
    {
      Console.Clear();
      CodingSession session = SelectionPrompt.ViewCodingSessionsMenu(sessions);
      if (session == null) return;

      string action = SelectionPrompt.ChangeEndTimeOptions();
      int change = AnsiConsole.Ask<int>($"How much do you want to {action} by?");
      change = action == "Decrease" ? -change : change;
      string type = SelectionPrompt.TimeOptions();

      if (_sessionService.UpdateCodingSessionById(session, change, type))
      {
        _goalService.UpdateGoalsOnComplete(session);
        break;
      }
      else
      {
        AnsiConsole.MarkupLine("[bold red]The new end time cannot be in the future. Please try again.[/]");
        Thread.Sleep(1500);
      }
    }
  }

  private void DeleteSession(List<CodingSession> sessions)
  {
    CodingSession session = SelectionPrompt.ViewCodingSessionsMenu(sessions);
    if (session == null) return;
    bool confirm = AnsiConsole.Confirm("Are you sure you want to delete this?");
    if (confirm)
    {
      _sessionService.DeleteCodingSession(session);
      _goalService.UpdateGoalsOnDelete(session);
      Thread.Sleep(2000);
    }
  }

  public void LiveSession()
  {
    CodingSession session = new();
    session.StartSession();
    TableConfigurationEngine.LiveSessionDisplay(session);
    session.EndSession();
    _sessionService.CreateCodingSession(session);
    _goalService.UpdateGoalsOnComplete(session);
    AnsiConsole.Markup("[bold yellow]Press any key to return to the main menu...[/]");
    Console.ReadKey(true);
  }
}