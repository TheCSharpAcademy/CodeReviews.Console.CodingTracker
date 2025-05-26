using System.Globalization;
using Spectre.Console;

namespace CodingTracker.Golvi1124.Helpers;

internal class Sorting
{
    internal record DateRange(DateTime Start, DateTime End);

    internal static DateRange AskForYearRange()
    {
        var year = AnsiConsole.Ask<int>("Enter [blue]year[/] (e.g., 2025):");
        return new DateRange(
            new DateTime(year, 1, 1),
            new DateTime(year, 12, 31, 23, 59, 59)
        );
    }

    internal static DateRange AskForMonthRange()
    {
        var input = AnsiConsole.Ask<string>("Enter [blue]year and month[/] (format: yyyy-MM):");

        DateTime parsed;

        while (!DateTime.TryParseExact(input, "yyyy-MM", null, DateTimeStyles.None, out parsed))
        {
            input = AnsiConsole.Ask<string>("[red]Invalid format.[/] Try again (yyyy-MM):");
        }

        var start = new DateTime(parsed.Year, parsed.Month, 1);
        var end = start.AddMonths(1).AddSeconds(-1); // End of the month

        return new DateRange(start, end);
    }

    internal static DateRange AskForWeekRange()
    {
        var input = AnsiConsole.Ask<string>("Enter a [blue]date[/] within the week (format: dd-MM-yy):");

        DateTime parsed;
        while (!DateTime.TryParseExact(input, "dd-MM-yy", null, DateTimeStyles.None, out parsed))
        {
            input = AnsiConsole.Ask<string>("[red]Invalid format.[/] Try again (dd-MM-yy):");
        }

        int daysToMonday = ((int)parsed.DayOfWeek + 6) % 7;
        var monday = parsed.AddDays(-daysToMonday);
        var sunday = monday.AddDays(6).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        return new DateRange(monday, sunday);
    }

    internal static DateRange AskForDayRange()
    {
        var input = AnsiConsole.Ask<string>("Enter [blue]exact date[/] (format: dd-MM-yy):");

        DateTime parsed;

        while (!DateTime.TryParseExact(input, "dd-MM-yy", null, DateTimeStyles.None, out parsed))
        {
            input = AnsiConsole.Ask<string>("[red]Invalid format.[/] Try again (dd-MM-yy):");
        }

        var start = parsed.Date;
        var end = parsed.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        return new DateRange(start, end);
    }
}