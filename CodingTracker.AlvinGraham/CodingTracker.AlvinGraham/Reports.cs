using Spectre.Console;

namespace CodingTracker;

internal class Reports
{
	private List<CodingRecord> RecordList { get; set; }
	public Reports()
	{
		var dataAccess = new DataAccess();
		RecordList = dataAccess.GetAllRecords().ToList();
	}

	internal void RunReports()
	{
		bool moreRports = true;

		while (moreRports)
		{
			Utilities.ClearScreen("Reports Menu");
			Console.WriteLine("\nView a report of number of coding sessions and average duration of"
			+ " each coding session in a coding period.\n");

			DateTime[] periodDates = Utilities.GetDateInputs("Reports Menu");
			if (periodDates == null)
				continue;

			var startDate = periodDates[0];
			var endDate = periodDates[1];

			var filteredResults = this.RecordList.Where(x => x.DateStart > startDate && x.DateEnd < endDate);
			int periodCount = filteredResults.Count();

			if (periodCount == 0)
			{
				Console.WriteLine($"\nThere are no sessions between {startDate:dd-MM-yy} at {startDate:mm:ss} and {endDate:dd-MM-yy} at {endDate:mm:ss}\n");
			}
			else
			{
				int totalSeconds = 0;
				var recordTable = new Table();
				recordTable.AddColumn("ID");
				recordTable.AddColumn("Session Start");
				recordTable.AddColumn("Session End");
				recordTable.AddColumn("Session Duration");

				foreach (var record in filteredResults)
				{
					totalSeconds += (int)record.Duration.TotalSeconds;
					recordTable.AddRow(record.Id.ToString(), record.DateStart.ToString(), record.DateEnd.ToString(), $"{(int)record.Duration.TotalHours} hours {record.Duration.TotalMinutes % 60} minutes");
				}

				int avgSeconds = totalSeconds / periodCount;
				TimeSpan duration = TimeSpan.FromSeconds(avgSeconds);
				var resultsTable = new Table();
				resultsTable.AddColumns("Total Sessions", "Average Duration");
				resultsTable.AddRow(periodCount.ToString(), duration.ToString());

				AnsiConsole.WriteLine($"\nPeriod Sessions ({startDate:dd-MM-yy} at {startDate:mm:ss} through {endDate:dd-MM-yy} at {endDate:mm:ss}):");
				AnsiConsole.Write(recordTable);
				Console.WriteLine($"\nPeriod Statistics ({startDate:dd-MM-yy} at {startDate:mm:ss} through {endDate:dd-MM-yy} at {endDate:mm:ss}):");
				AnsiConsole.Write(resultsTable);
				Console.WriteLine();

			}

			moreRports = AnsiConsole.Confirm("Run another Report?");
		}
	}
}
