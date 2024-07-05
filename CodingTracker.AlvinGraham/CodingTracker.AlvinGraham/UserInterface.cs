﻿using Spectre.Console;
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

			try
			{
				switch (userChoice)
				{
					case MainMenuChoices.AddRecord:
						Utilities.ClearScreen("Adding Record");
						AddRecord();
						break;
					case MainMenuChoices.ViewRecords:
						Utilities.ClearScreen("Viewing Records");
						var dataAccess = new DataAccess();
						var records = dataAccess.GetAllRecords();
						ViewRecords(records);
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
			catch (ArgumentException)
			{
				Console.WriteLine("\nReturning to Main Menu.  Press any key to continue...");
				Console.ReadKey();
			}
		}
	}

	private static void ManageGoals()
	{
		Console.WriteLine("Implementation in Progress. Press any key to continue.");
		Console.ReadKey();
	}

	private static void RunReports()
	{
		Console.WriteLine("Implementation in Progress. Press any key to continue.");
		Console.ReadKey();
	}

	private static void FilterRecords()
	{
		var filterSession = new RecordFilter();
		filterSession.filterRecordsMenu();
		Console.WriteLine("\nPress any Key to Return to main menu.");
		Console.ReadKey();
	}

	private static void LiveTrack()
	{
		var trackingSession = new LiveTracker();
		trackingSession.trackSession();
		Console.WriteLine("\nPress any Key to Return to main menu.");
		Console.ReadKey();
	}

	private static void DeleteRecord()
	{
		var dataAccess = new DataAccess();
		var records = dataAccess.GetAllRecords();
		ViewRecords(records);

		var id = Utilities.GetNumber("Please type the id of the habit you want to delete.");

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

		var id = Utilities.GetNumber("Please type the id of the habit you want to update.");

		var record = records.Where(x => x.Id == id).Single();
		var dates = Utilities.GetDateInputs();

		record.DateStart = dates[0];
		record.DateEnd = dates[1];

		dataAccess.UpdateRecord(record);
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

		Console.WriteLine("Press any key to return to main menu.");
		Console.ReadKey();
	}

	private static void AddRecord()
	{
		CodingRecord record = new();

		var dateInputs = Utilities.GetDateInputs();
		record.DateStart = dateInputs[0];
		record.DateEnd = dateInputs[1];

		var dataAccess = new DataAccess();
		dataAccess.InsertRecord(record);
	}
}
