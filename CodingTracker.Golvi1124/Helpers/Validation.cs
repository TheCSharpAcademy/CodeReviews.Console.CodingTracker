using System.Globalization;
using Spectre.Console;

namespace CodingTracker.Golvi1124.Helpers;

internal class Validation
{
    internal static int ValidateInt(string input, string message)
    {
        int output = 0;
        while (!int.TryParse(input, out output) || Convert.ToInt32(input) < 0)
        {
            input = AnsiConsole.Ask<string>("[red]Invalid number:[/] " + message);
        }

        return output;
    }

    internal static DateTime ValidateStartDate(string input)
    {
        DateTime date;
        while (!DateTime.TryParseExact(input, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            input = AnsiConsole.Ask<string>("\n\n[red]Invalid date.[/] Format: dd-mm-yy hh:mm (24 hour clock). Please try again\n\n");
        }

        return date;
    }

    internal static DateTime ValidateEndDate(DateTime startDate, string endDateInput)
    {
        DateTime endDate;
        while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
        {
            endDateInput = AnsiConsole.Ask<string>("\n\n[red]Invalid date.[/] Format: dd-mm-yy hh:mm (24 hour clock). Please try again\n\n");
        }

        while (startDate > endDate)
        {
            endDateInput = AnsiConsole.Ask<string>("\n\nEnd date can't be before start date. Please try again\n\n");

            while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                endDateInput = AnsiConsole.Ask<string>("\n\n[red]Invalid date.[/] Format: dd-mm-yy hh:mm (24 hour clock). PLease try again\n\n");
            }
        }

        return endDate;
    }

    internal static DateTime GetValidDate(string prompt, Func<DateTime, bool> dateValidator)
    {
        while (true)
        {
            AnsiConsole.Markup(prompt + " "); // Allows markup
            string? input = Console.ReadLine();

            if (DateTime.TryParseExact(input, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                if (dateValidator(parsedDate))
                {
                    return parsedDate;
                }

                AnsiConsole.MarkupLine("[red]Date is out of valid range.[/] Try again.");
            }
            else
            {
                AnsiConsole.Markup("[red]Invalid format.[/] Please use dd-MM-yy.");
            }
        }
    }
}