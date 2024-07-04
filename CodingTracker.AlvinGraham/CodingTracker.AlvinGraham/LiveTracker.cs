using Spectre.Console;
using System.Globalization;

namespace CodingTracker;

internal class LiveTracker
{
	DateTime startTime { get; set; }
	DateTime endTime { get; set; }
	TimeSpan duration { get; set; }
	public LiveTracker()
	{

	}

	internal void trackSession()
	{
		Console.WriteLine("This option will track a session in coding session in " +
			"real time and allow you to store the results.\n");

		if (!AnsiConsole.Confirm("Would you like to start a coding session now?"))
		{
			Console.WriteLine("Ending session tracking");
			return;
		}

		startTime = DateTime.Now;
		Console.WriteLine($"Coding Session Started at {startTime}");
		Console.WriteLine("Press any key to stop session");

		int currentSeconds = 99;
		while (!Console.KeyAvailable)
		{
			duration = DateTime.Now - startTime;
			if (duration.Seconds != currentSeconds)
			{
				Console.CursorLeft = 0;
				Console.Write($"Timer: {duration:hh\\:mm\\:ss}");
				currentSeconds = duration.Seconds;
			}

		}
		Console.ReadKey(false);

		endTime = DateTime.Now;

		Console.WriteLine($"\n\nSession Complete. Total Time Elapsed {duration:hh\\:mm\\:ss}\n");

		if (AnsiConsole.Confirm("Would you like to record these result?"))
		{
			startTime = DateTime.ParseExact($"{startTime:dd}-{startTime:MM}-{startTime:yy} {startTime:HH}:{startTime:mm}", "dd-MM-yy HH:mm",
				CultureInfo.InvariantCulture, DateTimeStyles.None);
			endTime = DateTime.ParseExact($"{endTime:dd}-{endTime:MM}-{endTime:yy} {endTime:HH}:{endTime:mm}", "dd-MM-yy HH:mm",
				CultureInfo.InvariantCulture, DateTimeStyles.None);
			var session = new CodingRecord { DateStart = startTime, DateEnd = endTime };
			var dataAccess = new DataAccess();

			dataAccess.InsertRecord(session);

			Console.WriteLine("Session recorded");
		}
		else
		{
			Console.WriteLine("Session not recorded.");
		}

	}
}
