using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker;

internal static class UserInterface
{
	internal static void MainMenu()
	{
		var isMenuRunning = true;

		while (isMenuRunning)
		{
			Utilities.ClearScreen(Utilities.titleText);
			var selectionPrompt = new SelectionPrompt<MainMenuChoices>()
				.Title("What would you like to do?")
				.AddChoices(
					MainMenuChoices.AddRecord,
					MainMenuChoices.ViewRecords,
					MainMenuChoices.UpdateRecord,
					MainMenuChoices.DeleteRecord,
					MainMenuChoices.Seperator,
					MainMenuChoices.LiveTrack,
					MainMenuChoices.FilterRecords,
					MainMenuChoices.Reports,
					MainMenuChoices.ManageGoals,
					MainMenuChoices.Seperator,
					MainMenuChoices.Quit);
			selectionPrompt.PageSize = 12;
			selectionPrompt.Converter = MenuChoiceConverter.ChoiceToString;

			var userChoice = AnsiConsole.Prompt(selectionPrompt);


			switch (userChoice)
			{
				case MainMenuChoices.AddRecord:
					Utilities.ClearScreen("Adding Record");
					AddRecord();
					break;
				case MainMenuChoices.ViewRecords:
					Utilities.ClearScreen("Viewing Records");
					var dataAccess = new DataAccess();
					try
					{
						var records = dataAccess.GetAllRecords();
						ViewRecords(records);
					}
					catch (InvalidOperationException)
					{
						Console.WriteLine("There are currently no records to display. Press any key to return to Main Menu");
						Console.ReadKey(false);
						break;
					}
					Console.WriteLine("Press any key to continue.");
					Console.ReadKey(false);
					break;
				case MainMenuChoices.UpdateRecord:
					Utilities.ClearScreen("Updating Record");
					UpdateRecord();
					break;
				case MainMenuChoices.DeleteRecord:
					Utilities.ClearScreen("Deleting Record");
					DeleteRecord();
					break;
				case MainMenuChoices.LiveTrack:
					Utilities.ClearScreen("Recoring Live Tracking Session");
					LiveTrack();
					break;
				case MainMenuChoices.FilterRecords:
					FilterRecords();
					break;
				case MainMenuChoices.Reports:
					Utilities.ClearScreen("Reports Menu");
					RunReports();
					break;
				case MainMenuChoices.ManageGoals:
					Utilities.ClearScreen("Managing Goals");
					ManageGoals();
					break;
				case MainMenuChoices.Quit:
					Console.WriteLine("Goodbye");
					isMenuRunning = false;
					break;
			}

		}
	}

	private static void ManageGoals()
	{
		var goalsSession = new Goals();
		goalsSession.GoalsMenu();
	}

	private static void RunReports()
	{
		var reportSession = new Reports();
		reportSession.RunReports();
		Console.WriteLine("\nPress any Key to Return to main menu.");
		Console.ReadKey();
	}

	private static void FilterRecords()
	{
		var filterSession = new RecordFilter();
		filterSession.FilterRecordsMenu();
		Console.WriteLine("\nPress any Key to Return to main menu.");
		Console.ReadKey();
	}

	private static void LiveTrack()
	{
		var trackingSession = new LiveTracker();
		trackingSession.TrackSession();
		Console.WriteLine("\nPress any Key to Return to main menu.");
		Console.ReadKey();
	}

	private static void DeleteRecord()
	{
		var dataAccess = new DataAccess();
		var records = dataAccess.GetAllRecords();


		if (records.Count() == 0)
		{
			Console.WriteLine("There are currently no records to delete. Press any key to return to the Main Menu");
			Console.ReadKey(false);
			return;
		}

		ViewRecords(records);

		var id = Utilities.GetNumber("Please type the id of the record you want to delete.");

		if (!AnsiConsole.Confirm("Are you sure?"))
			return;

		var response = dataAccess.DeleteRecord(id);

		var responseMessage = response < 1
			? $"Record with id {id} doesn't exit. Press any key to return to Main Menu."
			: "Record deleted successfully. Press any key to return to Main Menu";

		Console.WriteLine(responseMessage);
		Console.ReadKey(false);
	}

	private static void UpdateRecord()
	{
		var dataAccess = new DataAccess();
		var records = dataAccess.GetAllRecords();

		if (records.Count() == 0)
		{
			Console.WriteLine("There are currently no records to update. Press any key to return to the Main Menu");
			Console.ReadKey(false);
			return;
		}
		ViewRecords(records);

		var id = Utilities.GetNumber("Please type the id of the record you want to update.");

		try
		{
			var record = records.Where(x => x.Id == id).Single();
			var dates = Utilities.GetDateInputs("Main Menu");
			if (dates == null)
				return;

			record.DateStart = dates[0];
			record.DateEnd = dates[1];

			dataAccess.UpdateRecord(record);

			Console.WriteLine("Record Updated. Press any key to continue.");
			Console.ReadKey(false);
		}
		catch (InvalidOperationException)
		{
			Console.WriteLine("No record with that ID exists. Press any key to return to Main Menu");
			Console.ReadKey(false);
		}
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

		Console.WriteLine();

	}

	private static void AddRecord()
	{
		CodingRecord record = new();

		var dateInputs = Utilities.GetDateInputs("Main Menu");
		if (dateInputs == null)
			return;
		record.DateStart = dateInputs[0];
		record.DateEnd = dateInputs[1];

		var dataAccess = new DataAccess();
		dataAccess.InsertRecord(record);
	}
}
