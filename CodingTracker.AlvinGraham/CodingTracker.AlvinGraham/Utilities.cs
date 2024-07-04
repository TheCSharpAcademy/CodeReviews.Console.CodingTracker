using Spectre.Console;

namespace CodingTracker;

internal static class Utilities
{

	internal static void ClearScreen(string message)
	{
		Console.Clear();
		Console.WriteLine(message);
		Console.WriteLine("--------------------------------------");
	}

	internal static int GetNumber(string message)
	{
		string numberInput = AnsiConsole.Ask<string>(message);

		if (numberInput == "0")
			throw new ArgumentException("Returning to Main Menu");

		var output = Validation.ValidateInt(numberInput, message);

		return output;
	}

	internal static DateTime[] GetDateInputs()
	{
		var startDateInput = AnsiConsole.Ask<string>("Input Start Date with the format: dd-mm-yy hh:mm (24 hour clock), or enter 0 to return to main menu.");

		if (startDateInput == "0")
			throw new ArgumentException("Returning to Main Menu");

		var startDate = Validation.ValidateStartDate(startDateInput);

		var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: dd-mm-yy hh:mm (24 hour clock), or enter 0 to return to main menu.");

		if (endDateInput == "0")
			throw new ArgumentException("Returning to Main Menu");

		var endDate = Validation.ValidateEndDate(startDate, endDateInput);

		return [startDate, endDate];
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
