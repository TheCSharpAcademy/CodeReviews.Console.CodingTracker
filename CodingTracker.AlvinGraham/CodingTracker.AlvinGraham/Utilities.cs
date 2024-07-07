using Spectre.Console;

namespace CodingTracker;

internal static class Utilities
{

	internal static void ClearScreen(string message)
	{
		Console.Clear();
		AnsiConsole.Clear();
		Console.WriteLine(message);
		Console.WriteLine("--------------------------------------");
	}

	internal static int GetNumber(string message)
	{
		string numberInput = AnsiConsole.Ask<string>(message);

		var output = Validation.ValidateInt(numberInput, message);

		return output;
	}

	internal static DateTime[] GetDateInputs(string menuMessage)
	{
		var startDateInput = AnsiConsole.Ask<string>($"Input Start Date with the format: dd-mm-yy hh:mm (24 hour clock), or enter 0 to return to {menuMessage}.");

		if (startDateInput == "0")
		{
			Console.WriteLine($"Returning to {menuMessage}. Please press any key to continue.");
			Console.ReadKey(false);
			return null!;
		}

		var startDate = Validation.ValidateStartDate(startDateInput);

		var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: dd-mm-yy hh:mm (24 hour clock), or enter 0 to return to main menu.");

		if (endDateInput == "0")
		{
			Console.WriteLine($"Returning to {menuMessage}. Please press any key to continue.");
			Console.ReadKey(false);
			return null!;
		}

		var endDate = Validation.ValidateEndDate(startDate, endDateInput);

		return [startDate, endDate];
	}

	internal static DateTime GetStartDate()
	{
		var startDateInput = AnsiConsole.Ask<string>("Enter the start day of the desired period with the format: dd-mm-yy: ");

		var startDate = Validation.ValidateStartDay(startDateInput);

		return startDate;
	}

	internal static DateTime GetEndDate()
	{
		var startDateInput = AnsiConsole.Ask<string>("Enter the end day of the desired period with the format: dd-mm-yy: ");

		var startDate = Validation.ValidateStartDay(startDateInput);

		return startDate;
	}

	internal static string GetSortOrder()
	{
		var sortOrderPrompt = new TextPrompt<string>("Sort in Ascending or Descending order?")
	.AddChoice<string>("ASC")
	.AddChoice<string>("DESC")
	.AddChoice<string>("NONE")
	.DefaultValue<string>("ASC")
	.ShowDefaultValue(true);

		var sortOrder = AnsiConsole.Prompt(sortOrderPrompt);
		return sortOrder;
	}

	internal static IEnumerable<CodingRecord> SortResults(string sortOrder, IEnumerable<CodingRecord> filteredResults)
	{
		switch (sortOrder)
		{
			case "ASC":
				filteredResults = filteredResults.OrderBy(x => x.DateStart);
				break;
			case "DESC":
				filteredResults = filteredResults.OrderByDescending(x => x.DateStart);
				break;
		}

		return filteredResults;
	}

	internal static string titleText = @"
░░      ░░░░      ░░░       ░░░        ░░   ░░░  ░░░      ░░░░░░░░    
▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒▒▒▒  ▒▒▒▒▒    ▒▒  ▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒▒    
▓  ▓▓▓▓▓▓▓▓  ▓▓▓▓  ▓▓  ▓▓▓▓  ▓▓▓▓▓  ▓▓▓▓▓  ▓  ▓  ▓▓  ▓▓▓   ▓▓▓▓▓▓▓    
█  ████  ██  ████  ██  ████  █████  █████  ██    ██  ████  ███████    
██      ████      ███       ███        ██  ███   ███      ████████    
                                                                      
░        ░░       ░░░░      ░░░░      ░░░  ░░░░  ░░        ░░       ░░
▒▒▒▒  ▒▒▒▒▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒  ▒▒▒  ▒▒▒  ▒▒▒▒▒▒▒▒  ▒▒▒▒  ▒
▓▓▓▓  ▓▓▓▓▓       ▓▓▓  ▓▓▓▓  ▓▓  ▓▓▓▓▓▓▓▓     ▓▓▓▓▓      ▓▓▓▓       ▓▓
████  █████  ███  ███        ██  ████  ██  ███  ███  ████████  ███  ██
████  █████  ████  ██  ████  ███      ███  ████  ██        ██  ████  █
                                                                      
";

}
