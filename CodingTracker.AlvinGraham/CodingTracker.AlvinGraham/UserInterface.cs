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
					var dataAccess = new DataAccess();
					var records = dataAccess.GetAllRecords();
					ViewRecords(records);
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
		var dataAccess = new DataAccess();
		var records = dataAccess.GetAllRecords();
		ViewRecords(records);

		var id = GetNumber("Please type the id of the habit you want to delete.");

		if (!AnsiConsole.Confirm("Are you sure?"))
			return;

		var response = dataAccess.DeleteRecord(id);

		var responseMessage = response < 1
			? $"Record with id {id} doesn't exit. Press any key to return to Main Menu."
			: "Record deleted successfully. Press any key to return to Main Menu";

		Console.WriteLine(responseMessage);
		Console.ReadKey();
	}

	private static void UpdateRecord()
	{
		var dataAccess = new DataAccess();
		var records = dataAccess.GetAllRecords();
		ViewRecords(records);

		var id = GetNumber("Please type the id of the habit you want to update.");

		var record = records.Where(x => x.Id == id).Single();
		var dates = GetDateInputs();

		record.DateStart = dates[0];
		record.DateEnd = dates[1];

		dataAccess.UpdateRecord(record);
	}

	private static int GetNumber(string message)
	{
		string numberInput = AnsiConsole.Ask<string>(message);

		if (numberInput == "0")
			MainMenu();

		int output = 0;
		while (!int.TryParse(numberInput, out output) || Convert.ToInt32(numberInput) < 0)
		{
			numberInput = AnsiConsole.Ask<string>("Invalid number: " + message);
		}

		return output;
	}

	private static void ViewRecords(IEnumerable<CodingRecord> records)
	{
		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Start Date");
		table.AddColumn("End Date");
		table.AddColumn("Duration");

		foreach (var record in records)
		{
			table.AddRow(record.Id.ToString(), record.DateStart.ToString(), record.DateEnd.ToString(), $"{(int)record.Duration.TotalHours} hours {record.Duration.TotalMinutes % 60} minutes");
		}

		AnsiConsole.Write(table);
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
