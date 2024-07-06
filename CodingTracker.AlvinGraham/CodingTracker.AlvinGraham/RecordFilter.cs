using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker;

internal class RecordFilter
{
	private List<CodingRecord> RecordList { get; set; }
	public RecordFilter()
	{
		var dataAccess = new DataAccess();
		RecordList = dataAccess.GetRecordList();
	}

	internal void FilterRecordsMenu()
	{
		var isMenuRunning = true;

		while (isMenuRunning)
		{
			Utilities.ClearScreen("Filtering Records");
			Console.WriteLine("\nYou can filter all sessions by a time period and start date of your " +
				"choice,\n and then sort in ascending or descending order.");

			var selectionPrompt = new SelectionPrompt<FilterMenuChoices>()
				.Title("How would you like to filter?")
				.AddChoices(
					FilterMenuChoices.DailySessions,
					FilterMenuChoices.WeeklySessions,
					FilterMenuChoices.MonthlySessions,
					FilterMenuChoices.YearlySessions,
					FilterMenuChoices.AllSessions,
					FilterMenuChoices.ReturnToMain
					);
			selectionPrompt.Converter = MenuChoiceConverter.ChoiceToString;

			var userChoice = AnsiConsole.Prompt(selectionPrompt);

			switch (userChoice)
			{
				case FilterMenuChoices.DailySessions:
					Utilities.ClearScreen("Filtering by Day");
					FilterByDay();
					break;
				case FilterMenuChoices.WeeklySessions:
					Utilities.ClearScreen("Filtering by Week");
					FilterByWeek();
					break;
				case FilterMenuChoices.MonthlySessions:
					Utilities.ClearScreen("Filtering by Month");
					FilterByMonth();
					break;
				case FilterMenuChoices.YearlySessions:
					Utilities.ClearScreen("Filtering by Year");
					FilterByYear();
					break;
				case FilterMenuChoices.AllSessions:
					Utilities.ClearScreen("All Sessions - Date Filter");
					FilterByAll();
					break;
				case FilterMenuChoices.ReturnToMain:
					return;
			}
		}
	}

	private void FilterByMonth()
	{
		DateTime startDay = Utilities.GetStartDate();
		DateTime endDay = startDay.AddMonths(1);

		Console.WriteLine($"Filtering sessions between {startDay:dd-MM-yy} and {endDay:dd-MM-yy}.");

		var sortOrder = Utilities.GetSortOrder();
		var filteredResults = this.RecordList.Where(x => x.DateStart > startDay && x.DateEnd < endDay);

		filteredResults = Utilities.SortResults(sortOrder, filteredResults);

		ViewResults(filteredResults);

		Console.ReadKey(false);
	}

	private void FilterByAll()
	{
		Console.WriteLine($"Filtering all sessions.");

		var sortOrder = Utilities.GetSortOrder();
		var filteredResults = (IEnumerable<CodingRecord>)this.RecordList;

		filteredResults = Utilities.SortResults(sortOrder, filteredResults);

		ViewResults(filteredResults);

		Console.ReadKey(false);
	}

	private void FilterByYear()
	{
		DateTime startDay = Utilities.GetStartDate();
		DateTime endDay = startDay.AddYears(1);

		Console.WriteLine($"Filtering sessions between {startDay:dd-MM-yy} and {endDay:dd-MM-yy}.");

		var sortOrder = Utilities.GetSortOrder();
		var filteredResults = this.RecordList.Where(x => x.DateStart > startDay && x.DateEnd < endDay);

		filteredResults = Utilities.SortResults(sortOrder, filteredResults);

		ViewResults(filteredResults);

		Console.ReadKey(false);
	}

	private void FilterByWeek()
	{
		DateTime startDay = Utilities.GetStartDate();
		DateTime endDay = startDay.AddDays(7);

		Console.WriteLine($"Filtering sessions between {startDay:dd-MM-yy} and {endDay:dd-MM-yy}.");

		var sortOrder = Utilities.GetSortOrder();
		var filteredResults = this.RecordList.Where(x => x.DateStart > startDay && x.DateEnd < endDay);

		filteredResults = Utilities.SortResults(sortOrder, filteredResults);

		ViewResults(filteredResults);

		Console.ReadKey(false);
	}

	private void FilterByDay()
	{
		DateTime startDay = Utilities.GetStartDate();
		DateTime endDay = startDay.AddDays(1);

		Console.WriteLine($"Filtering sessions between {startDay:dd-MM-yy} and {endDay:dd-MM-yy}.");

		var sortOrder = Utilities.GetSortOrder();
		var filteredResults = this.RecordList.Where(x => x.DateStart > startDay && x.DateEnd < endDay);

		filteredResults = Utilities.SortResults(sortOrder, filteredResults);

		ViewResults(filteredResults);

		Console.ReadKey(false);
	}

	private static void ViewResults(IEnumerable<CodingRecord> records)
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

		Console.WriteLine("Press any key to return to filter menu.");
	}
}
