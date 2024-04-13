using CodingTracker.Database.DbContext;
using CodingTracker.Database.enums;
using CodingTracker.Database.Helpers;
using Spectre.Console;

namespace CodingTracker.BBualdo;

public class AppEngine
{
  public DbContext Db { get; set; }
  public bool IsRunning { get; set; }
  public Stopwatch Stopwatch { get; set; }

  public AppEngine()
  {
    Db = new DbContext();
    IsRunning = true;
    Stopwatch = new Stopwatch();
  }

  public void MainMenu()
  {
    string choice = ConsoleEngine.GetSelection("MAIN MENU", "What would you like to do?", ["Start Coding", "Sessions", "Goals", "Close App"]);

    switch (choice)
    {
      case "Start Coding":
        string[]? dates = StartCoding();

        if (dates == null)
        {
          AnsiConsole.Markup("[red]Something went wrong :([/]");
          break;
        }

        Db.InsertSession(dates[0], dates[1]);
        break;
      case "Sessions":
        SessionsMenu();
        break;
      case "Goals":
        GoalsMenu();
        break;
      case "Close App":
        AnsiConsole.Markup("[green]Goodbye![/]");
        IsRunning = false;
        break;
    }
  }

  public void SessionsMenu()
  {
    string choice = ConsoleEngine.GetSelection("SESSIONS MENU", "What would you like to do?", ["Back", "Get All Sessions", "Insert Session", "Update Session", "Delete Session", "Get Reports"]);

    switch (choice)
    {
      case "Back":
        break;
      case "Get All Sessions":
        Db.GetAllSessions();
        break;
      case "Insert Session":
        Db.InsertSession();
        break;
      case "Update Session":
        Db.UpdateSession();
        break;
      case "Delete Session":
        Db.DeleteSession();
        break;
      case "Get Reports":
        ReportsMenu();
        break;
    }
  }

  public void GoalsMenu()
  {
    string choice = ConsoleEngine.GetSelection("GOALS MENU", "What would you like to do?", ["Back", "Get All Goals", "Add Goal", "Delete Goal", "Get Completed Goals"]);

    switch (choice)
    {
      case "Back":
        break;
      case "Get All Goals":
        Db.GetAllGoals();
        break;
      case "Add Goal":
        Db.AddGoal();
        break;
      case "Delete Goal":
        Db.DeleteGoal();
        break;
      case "Get Completed Goals":
        Db.GetAllGoals(true);
        break;
    }
  }

  public void ReportsMenu()
  {
    string choice = ConsoleEngine.GetSelection("REPORTS MENU", "Select period:", ["Back", "Daily Report", "Weekly Report", "Monthly Report", "Yearly Report"]);
    ReportOptions reportOption;
    OrderOptions? orderOption;

    switch (choice)
    {
      case "Back":
        SessionsMenu();
        break;
      case "Daily Report":
        reportOption = ReportOptions.Daily;
        orderOption = GetOrderOption();
        Db.GetReport(reportOption, orderOption);
        break;
      case "Weekly Report":
        reportOption = ReportOptions.Weekly;
        orderOption = GetOrderOption();
        Db.GetReport(reportOption, orderOption);
        break;
      case "Monthly Report":
        reportOption = ReportOptions.Monthly;
        orderOption = GetOrderOption();
        Db.GetReport(reportOption, orderOption);
        break;
      case "Yearly Report":
        reportOption = ReportOptions.Yearly;
        orderOption = GetOrderOption();
        Db.GetReport(reportOption, orderOption);
        break;
    }
  }

  private OrderOptions? GetOrderOption()
  {
    string choice = ConsoleEngine.GetSelection("REPORTS MENU", "Select order option:", ["Back", "Default", "Ascending", "Descending"]);

    switch (choice)
    {
      case "Back":
        ReportsMenu();
        break;
      case "Default":
        return null;
      case "Ascending":
        return OrderOptions.ASC;
      case "Descending":
        return OrderOptions.DESC;
    }

    return null;
  }

  private string[]? StartCoding()
  {
    AnsiConsole.Clear();
    Stopwatch.Start();
    AnsiConsole.Markup("Your coding session is [green]active[/].\n");
    string stop = ConsoleEngine.GetSelection("CODING SESSION", "Your coding session is [green]Active[/]. Press Enter when you are done.", ["Stop Coding"]);

    if (stop == "Stop Coding")
    {
      Stopwatch.Stop();

      string startDate = Stopwatch.StartDate.ToString("yyyy-MM-dd HH:mm");
      string endDate = Stopwatch.StopDate.ToString("yyyy-MM-dd HH:mm");
      return [startDate, endDate];
    }

    return null;
  }
}