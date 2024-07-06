using Spectre.Console;
using System.Globalization;

namespace CodingTracker;

internal class LiveTracker
{
	DateTime StartTime { get; set; }
	DateTime EndTime { get; set; }
	TimeSpan Duration { get; set; }
	public LiveTracker()
	{
	}

	internal void TrackSession()
	{
		Console.WriteLine("This option will track a session in coding session in " +
			"real time and allow you to store the results.\n");

		if (!AnsiConsole.Confirm("Would you like to start a coding session now?"))
		{
			Console.WriteLine("Ending session tracking");
			return;
		}

		StartTime = DateTime.Now;
		Console.WriteLine($"Coding Session Started at {StartTime}");
		Console.WriteLine("Press any key to stop session");

		int currentSeconds = 99;
		while (!Console.KeyAvailable)
		{
			Duration = DateTime.Now - StartTime;
			if (Duration.Seconds != currentSeconds)
			{
				Console.CursorLeft = 0;
				Console.Write($"Timer: {Duration:hh\\:mm\\:ss}");
				currentSeconds = Duration.Seconds;
			}

		}
		Console.ReadKey(false);

		EndTime = DateTime.Now;

		Console.WriteLine($"\n\nSession Complete. Total Time Elapsed {Duration:hh\\:mm\\:ss}\n");

		if (AnsiConsole.Confirm("Would you like to record these result?"))
		{
			StartTime = DateTime.ParseExact($"{StartTime:dd}-{StartTime:MM}-{StartTime:yy} {StartTime:HH}:{StartTime:mm}", "dd-MM-yy HH:mm",
				CultureInfo.InvariantCulture, DateTimeStyles.None);
			EndTime = DateTime.ParseExact($"{EndTime:dd}-{EndTime:MM}-{EndTime:yy} {EndTime:HH}:{EndTime:mm}", "dd-MM-yy HH:mm",
				CultureInfo.InvariantCulture, DateTimeStyles.None);
			var session = new CodingRecord { DateStart = StartTime, DateEnd = EndTime };
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
