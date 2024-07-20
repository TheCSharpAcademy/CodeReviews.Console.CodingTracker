using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;
using Spectre.Console;

namespace CodingTracker.kwm0304.Views;

public class TableConfigurationEngine
{
  public static void LiveSessionDisplay(CodingSession session)
  {
    Console.Clear();
    var timer = new Timer(TimerCallback!, session, 0, 1000);
    Console.ReadKey(true);
    timer.Dispose();
    AnsiConsole.MarkupLine($"[green]Timer stopped at: {session.SessionLength:hh\\:mm\\:ss}[/]");
  }

  public static void TimerCallback(object? state)
  {
    if (state is CodingSession session)
    {
      var elapsed = session._stopWatch!.Elapsed;
      var dateNow = DateOnly.FromDateTime(DateTime.Now);
      var timerDisplay = new Panel($"[bold yellow2]{elapsed:hh\\:mm\\:ss} [bold blue]{dateNow}[/][/]")
      {
        Border = BoxBorder.Double,
        Padding = new Padding(6, 3, 6, 3),
        BorderStyle = new Style(Color.Chartreuse3_1),
        Header = new PanelHeader("[bold yellow2]Session Timer[/]").Centered()
      };
      AnsiConsole.Cursor.SetPosition(0, 6);
      AnsiConsole.MarkupLine("[chartreuse3_1]Press any key to stop the timer and end the session.[/]");
      AnsiConsole.Write(timerDisplay);
      AnsiConsole.Cursor.SetPosition(0, 11);
    }
  }

  public static Goal CreateNewGoal()
  {
    Console.Clear();
    string name = AnsiConsole.Ask<string>("What label would you like to give this goal?");
    DateRange range = SelectionPrompt.SelectGoalType();
    int target = AnsiConsole.Ask<int>("How many hours should this be?");
    Goal goal = new(name, target, range, 0);
    return goal;
  }

  public static void CreateReports(TimeSpan total, TimeSpan average)
  {
    AnsiConsole.MarkupLine($"[bold green]Total time:[/] {total}");
    AnsiConsole.MarkupLine($"[bold green]Average time per session:[/] {average}");
  }
  public static void ViewGoals(List<Goal> goals)
  {
    var table = new Table();
    table.Title("Goal Table");
    table.AddColumns("GoalId", "Name", "Target", "Progress", "Percentage", "Accomplished", "CreatedOn", "EndDate");
    foreach (var goal in goals)
    {
      table.AddRow(
        goal.GoalId.ToString(),
        goal.GoalName,
        goal.TargetNumber.ToString(),
        goal.Progress.ToString(),
        goal.CalculateProgressPercentage().ToString() + "%",
        goal.Accomplished.ToString(),
        goal.CreatedOn.ToString("yyyy-MM-dd"),
        goal.EndDate.ToString("yyyy-MM-dd")
      );
    }
    AnsiConsole.Write(table);
  }

  public static void ViewSessions(List<CodingSession> sessions)
  {
    Console.Clear();
    var table = new Table();
    table.Title("Coding Sessions");
    table.AddColumns("Id", "Start Time", "End Time", "Hours");
    foreach (var session in sessions)
    {
      table.AddRow(
        session.Id.ToString(),
        session.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
        session.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
        session.SessionLength.TotalHours.ToString()
      );
    }
    AnsiConsole.Write(table);
  }
}