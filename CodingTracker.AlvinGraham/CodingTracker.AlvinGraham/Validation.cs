using Spectre.Console;
using System.Globalization;

namespace CodingTracker;

internal class Validation
{
	internal static int ValidateInt(string input, string message)
	{
		int output = 0;
		while (!int.TryParse(input, out output) || Convert.ToInt32(input) < 0)
		{
			input = AnsiConsole.Ask<string>("Invalid number: " + message);
		}

		return output;
	}

	internal static DateTime ValidateStartDate(string input)
	{
		DateTime date;
		while (!DateTime.TryParseExact(input, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
		{
			input = AnsiConsole.Ask<string>("\n\nInvalid date. Format : dd-mm-yy hh:mm (24 hour clock). Please try again\n\n");
		}

		return date;
	}

	internal static DateTime ValidateStartDay(string input)
	{
		DateTime date;
		while (!DateTime.TryParseExact(input, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
		{
			input = AnsiConsole.Ask<string>("\n\nInvalid date. Format : dd-mm-yy. Please try again\n\n");
		}

		return date;
	}

	internal static DateTime ValidateEndDate(DateTime startDate, string endDateInput)
	{
		DateTime endDate;
		while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
		{
			endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date. Format: dd-mm-yy hh:mm (24 hour clock). Please try again\n\n");
		}

		while (startDate > endDate)
		{
			endDateInput = AnsiConsole.Ask<string>("\n\nEnd date can't be before start date. Please try again\n\n");

			while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
			{
				endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date. Format dd-mm-yy hh:mm (24 hour clock). Please try again\n\n");
			}
		}

		return endDate;
	}
}

