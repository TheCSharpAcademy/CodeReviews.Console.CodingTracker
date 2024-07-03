using Spectre.Console;
using System.Globalization;
using static CodingTracker.Enums;

namespace CodingTracker;

internal static class UserInterface
{
	internal static void MainMenu()
	{
		var isMenuRunning = true;

		while (isMenuRunning)
		{
			var userChoice = AnsiConsole.Prompt(
				new SelectionPrompt<MainMenuChoices>()
				.Title("What would you like to do?")
				.AddChoices(
					MainMenuChoices.AddRecord,
					MainMenuChoices.ViewRecords,
					MainMenuChoices.UpdateRecord,
					MainMenuChoices.DeleteRecord,
					MainMenuChoices.Quit)
				);

			switch (userChoice)
			{
				case MainMenuChoices.AddRecord:
					AddRecord();
					break;
				case MainMenuChoices.ViewRecords:
					ViewRecords();
					break;
				case MainMenuChoices.UpdateRecord:
					UpdateRecord();
					break;
				case MainMenuChoices.DeleteRecord:
					DeleteRecord();
					break;
				case MainMenuChoices.Quit:
					Console.WriteLine("Goodbye");
					isMenuRunning = false;
					break;
			}
		}

	}

	private static void DeleteRecord()
	{

	}

	private static void UpdateRecord()
	{

	}

	private static void ViewRecords()
	{

	}

	private static void AddRecord()
	{
		CodingRecord record = new();

		var dateInputs = GetDateInputs();
		record.DateStart = dateInputs[0];
		record.DateEnd = dateInputs[1];

		var dataAccess = new DataAccess();
		dataAccess.InsertRecord(record);
	}

	private static DateTime[] GetDateInputs()
	{
		var startDateInput = AnsiConsole.Ask<string>("Input Start Date with the format: dd-mm-yy hh:mm (24 hour clock), or enter 0 to return to main menu.");

		if (startDateInput == "0")
			MainMenu();

		DateTime startDate;
		while (!DateTime.TryParseExact(startDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
		{
			startDateInput = AnsiConsole.Ask<string>("\n\nInvalid Date. Format: dd-mm-yy hh:mm (24 hour clock). Please try again.\n\n");
		}

		var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: dd-mm-yy hh:mm (24 hour clock), or enter 0 to return to main menu.");

		if (endDateInput == "0")
			MainMenu();

		DateTime endDate;
		while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
		{
			endDateInput = AnsiConsole.Ask<string>("\n\nInvalid Date. Format: dd-mm-yy hh:mm (24 hour clock). Please try again.\n\n");
		}

		while (startDate > endDate)
		{
			endDateInput = AnsiConsole.Ask<string>("\n\nEnd date can't be before start date. Please try again\n\n");

			while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
			{
				endDateInput = AnsiConsole.Ask<string>("\n\nInvalid Date. Format: dd-mm-yy hh:mm (24 hour clock). Please try again.\\n\\n\")");
			}
		}

		return [startDate, endDate];
	}
}
