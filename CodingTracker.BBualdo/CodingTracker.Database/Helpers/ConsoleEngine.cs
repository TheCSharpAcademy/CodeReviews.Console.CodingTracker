using CodingTracker.Database.enums;
using CodingTracker.Database.Models;
using Spectre.Console;

namespace CodingTracker.Database.Helpers;

public class ConsoleEngine
{
  public static string GetSelection(string header, string title, string[] choices)
  {
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine($"--------[bold green]{header}[/]--------");

    string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                    .Title(title)
                                    .HighlightStyle(new Style().Foreground(Color.Green))
                                    .AddChoices(choices)
                                    );

    return choice;
  }

  public static void GetCodingSessionsTable(List<CodingSession> sessions)
  {
    if (sessions.Count == 0)
    {
      AnsiConsole.WriteLine("Sessions list is empty. Create one first. Press any key to return to Main Menu.");
      Console.ReadKey();
      return;
    }

    Table table = new Table();
    table.AddColumn(new TableColumn("ID"));
    table.AddColumn(new TableColumn("Start Date"));
    table.AddColumn(new TableColumn("End Date"));
    table.AddColumn(new TableColumn("Duration"));

    foreach (CodingSession session in sessions)
    {
      table.AddRow(session.Session_Id.ToString(), session.Start_Date.ToString(), session.End_Date.ToString(), session.Duration.ToString());
    }

    AnsiConsole.Write(table);
  }

  public static void GetGoalsTable(List<Goal> goals)
  {
    if (goals.Count == 0)
    {
      AnsiConsole.WriteLine("Goals list is empty. Create one first. Press any key to return to Main Menu.");
      Console.ReadKey();
      return;
    }

    Table table = new Table();
    table.AddColumn(new TableColumn("ID"));
    table.AddColumn(new TableColumn("Start Date"));
    table.AddColumn(new TableColumn("End Date"));
    table.AddColumn(new TableColumn("Target Duration"));
    table.AddColumn(new TableColumn("Is Completed"));

    foreach (Goal goal in goals)
    {
      table.AddRow(goal.Goal_Id.ToString(), goal.Start_Date.ToString(), goal.End_Date.ToString(), goal.Target_Duration.ToString(), goal.Is_Completed == 1 ? "True" : "False");
    }

    AnsiConsole.Write(table);
  }

  public static void GetDurationSummary(List<CodingSession> sessions, ReportOptions reportOption)
  {
    int durationSum = 0;
    string period = "";

    foreach (CodingSession session in sessions)
    {
      durationSum += session.Duration;
    }

    switch (reportOption)
    {
      case ReportOptions.Daily:
        period = "today";
        break;
      case ReportOptions.Weekly:
        period = "this week";
        break;
      case ReportOptions.Monthly:
        period = "this month";
        break;
      case ReportOptions.Yearly:
        period = "this year";
        break;
    }

    int hours = durationSum / 60;
    int minutes = durationSum % 60;


    AnsiConsole.Markup($"You were coding for [green]{hours}h {minutes}m[/] {period}. ");
  }
}