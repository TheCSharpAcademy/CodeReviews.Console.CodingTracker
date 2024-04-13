using CodingTracker.Database.Helpers;
using Spectre.Console;

namespace CodingTracker.Database;

public class UserInput
{
  public static string GetStartDate()
  {
    string startDate = AnsiConsole.Ask<string>("Enter date and time when you started your coding session. ([green]Example: 2024-04-12 10:20[/]) ");

    while (!DateTimeValidator.IsValid(startDate))
    {
      startDate = AnsiConsole.Ask<string>("Try again: ");
    }

    return startDate;
  }

  public static string GetEndDate(string startDate)
  {
    string endDate = AnsiConsole.Ask<string>("Enter date and time when you finished your coding session. ([green]Example: 2024-04-12 14:20[/]) ");

    while (!DateTimeValidator.IsValid(endDate))
    {
      endDate = AnsiConsole.Ask<string>("Try again: ");
    }

    while (!DateTimeValidator.IsInThePast(endDate))
    {
      AnsiConsole.Markup("[red]Date and time must be in the past.[/]");
      endDate = AnsiConsole.Ask<string>("Try again: ");
    }

    while (!DateTimeValidator.AreValid(startDate, endDate))
    {
      endDate = AnsiConsole.Ask<string>("Try again: ");
    }

    return endDate;
  }

  public static string GetGoalEndDate(string startDate)
  {
    string endDate = AnsiConsole.Ask<string>("Enter date and time until you want to complete your goal. ([green]Example: 2024-04-12 14:20[/])");

    while (DateTimeValidator.IsInThePast(endDate))
    {
      AnsiConsole.Markup("[red]Date and time of completing goal can't be in the past.[/]");
      endDate = AnsiConsole.Ask<string>("Try again: ");
    }

    while (!DateTimeValidator.AreValid(startDate, endDate))
    {
      endDate = AnsiConsole.Ask<string>("Try again: ");
    };

    return endDate;
  }

  public static int GetTargetDuration(string startDate, string endDate)
  {
    int targetDuration = AnsiConsole.Ask<int>("How much minutes do you want to code in that time?");
    int duration = Convert.ToInt32((Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate)).TotalMinutes);

    while (targetDuration > duration)
    {
      AnsiConsole.Markup("[red]This duration is unachievable.[/]");
      targetDuration = AnsiConsole.Ask<int>("Try again: ");
    }

    return targetDuration;
  }
}