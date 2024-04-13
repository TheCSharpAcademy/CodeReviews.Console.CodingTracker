using Spectre.Console;
using System.Globalization;

namespace CodingTracker.Database.Helpers;

public class DateTimeValidator
{
  public static bool IsValid(string date)
  {
    if (DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime temp))
    {
      if (temp < new DateTime(2000, 1, 1))
      {
        AnsiConsole.Markup("[red]Date and time can't be older than 2000-01-01 00:00.[/] ");
        return false;
      }

      return true;
    };

    AnsiConsole.Markup("[red]Invalid date. Must be in format (yyyy-MM-dd HH:mm).[/] ");
    return false;
  }

  public static bool AreValid(string startDate, string endDate)
  {
    DateTime startDateTemp = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));
    DateTime endDateTemp = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"));

    if (endDateTemp > startDateTemp)
    {
      return true;
    }

    AnsiConsole.Markup($"[red]End date and time can't be older than start date. Please enter date newer than {startDate}.[/] ");
    return false;
  }

  public static bool IsInThePast(string date)
  {
    if (DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime temp))
    {
      if (temp > DateTime.Now)
      {
        return false;
      }
      return true;
    }

    return false;
  }
}