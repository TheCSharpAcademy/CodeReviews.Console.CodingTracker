using CodingTracker.Furiax.Model;
using Microsoft.Data.Sqlite;
using System.Globalization;
using ConsoleTableExt;

namespace CodingTracker.Furiax
{
	internal class Crud
	{
		internal static void CreateTable(string connectionString)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = @"CREATE TABLE IF NOT EXISTS CodeTracker(
									Id INTEGER PRIMARY KEY AUTOINCREMENT,
									StartTime TEXT NOT NULL,
									EndTime TEXT NOT NULL,
									Duration TEXT)";
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		internal static void DeleteRecord(string connectionString)
		{
			Console.Clear();
			OverviewTable(connectionString);
			while (true)
			{
				
				int recordToDelete = UserInput.GetId("What record do you want to delete: ");
				using (var connection = new SqliteConnection(connectionString))
				{
					connection.Open();
					bool doesIdExist = Validation.CheckIfRecordExists(recordToDelete, connectionString);
					if (doesIdExist)
					{
						
						var command = connection.CreateCommand();
						command.CommandText = "DELETE FROM CodeTracker WHERE id = (@recordToDelete)";
						command.Parameters.Add(new SqliteParameter("@recordToDelete", recordToDelete));
						command.ExecuteNonQuery();
						Console.WriteLine($"The record with id {recordToDelete} has been deleted from the db");
						break;
					}
					else
					{
						Console.WriteLine("A record with that id has not been found");
					}
					connection.Close();
				}
			}	
		}
		internal static void UpdateRecord(string connectionString)
		{
			Console.Clear();
			OverviewTable(connectionString);
			while(true)
			{
				int recordToUpdate = UserInput.GetId("What record do you want to update: ");
				using(var connection = new SqliteConnection( connectionString))
				{
					connection.Open();
					bool doesIdExist = Validation.CheckIfRecordExists(recordToUpdate, connectionString);
					if (doesIdExist)
					{
						DateTime askNewStartTime = UserInput.GetStartDate("Enter a new value for StartDate (format dd/mm/yy hh:mm)");
						DateTime askNewEndTime = UserInput.GetEndDate("Enter a new value for EndDate (format dd/mm/yy hh:mm)", askNewStartTime);
						TimeSpan timeBetween = CalculateDuration(askNewStartTime, askNewEndTime);
						if (timeBetween.TotalDays < 1)
						{
							string newStartTime = askNewStartTime.ToString("dd/MM/yy HH:mm");
							string newEndTime = askNewEndTime.ToString("dd/MM/yy HH:mm");
							string duration = timeBetween.ToString(@"hh\:mm");
							var command = connection.CreateCommand();
							command.CommandText = "UPDATE CodeTracker SET StartTime = @newStartTime, EndTime = @newEndTime, Duration = @duration WHERE Id = @recordToUpdate";
							command.Parameters.Add(new SqliteParameter("@newStartTime", newStartTime));
							command.Parameters.Add(new SqliteParameter("@newEndTime", newEndTime));
							command.Parameters.Add(new SqliteParameter("@duration", duration));
							command.Parameters.Add(new SqliteParameter("@recordToUpdate", recordToUpdate));
							command.ExecuteNonQuery();
							break;
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Duration can't be more than 24 hours, the record was not updated. Try again");
							Console.ForegroundColor = ConsoleColor.White;
						}
					}
					else 
					{
						Console.WriteLine("A record with that id has not been found");
					}
					connection.Close();
				}
			}
            Console.WriteLine("Record succesfully updated");
        }
		internal static void InsertRecord(string connectionString)
		{
			Console.Clear();
			DateTime inputStartDate = UserInput.GetStartDate("Please enter the start time in the following format dd/mm/yy hh:mm");
			DateTime inputEndDate = UserInput.GetEndDate("Please enter the end time in the following format dd/mm/yy hh:mm", inputStartDate);
			TimeSpan timeBetween = CalculateDuration(inputStartDate, inputEndDate);
			if (timeBetween.TotalDays < 1)
			{
				string startDate = inputStartDate.ToString("dd/MM/yy HH:mm");
				string endDate = inputEndDate.ToString("dd/MM/yy HH:mm");
				string duration = timeBetween.ToString(@"hh\:mm");
				using (var connection = new SqliteConnection(connectionString))
				{
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandText = "INSERT INTO CodeTracker (StartTime, EndTime, Duration) VALUES (@startDate,@endDate, @duration)";
					command.Parameters.Add(new SqliteParameter("@startDate", startDate));
					command.Parameters.Add(new SqliteParameter("@endDate", endDate));
					command.Parameters.Add(new SqliteParameter("@duration", duration));
					command.ExecuteNonQuery();
					connection.Close();
				}
				Console.WriteLine("Times succesfully added to database");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Duration can't be more than 24 hours, no record was made. Try again");
				Console.ForegroundColor = ConsoleColor.White;
			}
        }
		internal static TimeSpan CalculateDuration(DateTime startTime, DateTime endTime)
		{
			TimeSpan duration = endTime - startTime;
			return duration;
		}
		internal static void InsertStopwatchRecord(string connectionString, DateTime startTime, DateTime stopTime, string start, string stop)
		{
			TimeSpan timeBetween = CalculateDuration(startTime, stopTime);
			string duration = timeBetween.ToString(@"hh\:mm");
			if (timeBetween.Minutes <= 0)
			{
                Console.WriteLine("Session was less then a minute and therefore not stored");
            }
			else
			{
				using (var connection = new SqliteConnection(connectionString))
				{
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandText = "INSERT INTO CodeTracker (StartTime, EndTime, Duration) VALUES (@start, @stop, @duration)";
					command.Parameters.Add(new SqliteParameter("@start", start));
					command.Parameters.Add(new SqliteParameter("@stop",stop));
					command.Parameters.Add(new SqliteParameter("@duration", duration));
					command.ExecuteNonQuery();
					connection.Close();
				}
				Console.WriteLine("Record added to database");
			}
        }
		internal static void CreateReport(string connectionString)
		{
			Console.Clear();
			string sqlCommand = "SELECT * FROM CodeTracker";
			List<CodingSession> sessions = new List<CodingSession>();
			sessions = BuildList(connectionString, sqlCommand);
			
			TimeSpan totalTime = TimeSpan.Zero;
			foreach (var session in sessions)
			{
					totalTime = totalTime + session.Duration;
			}
			TimeSpan averageTime = TimeSpan.FromTicks(totalTime.Ticks/sessions.Count);

				Console.WriteLine($"You spend a total of {totalTime:%d} days, {totalTime:%h} hours and {totalTime:%m} minutes on coding, divided over {sessions.Count} sessions.");
                Console.WriteLine($"On average you spend {averageTime:%h} hours, {averageTime:%m} minutes per session.");
		}
		internal static void GoalStatus(string connectionString, TimeSpan goalTime)
		{
			Console.Clear();
			using (var connection = new SqliteConnection(connectionString))
			{
				var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = $"SELECT * FROM CodeTracker";
				List<CodingSession> sessions = new List<CodingSession>();
				sessions = BuildList(connectionString, command.CommandText);
				List<CodingSession> sessionsThisWeek = new List<CodingSession>();
				foreach (var session in sessions)
				{
					if (session.StartTime >= monday)
						sessionsThisWeek.Add(session);
				}
				TimeSpan totalTimeCodedThisWeek = TimeSpan.Zero;
				foreach (var sessionThisWeek in sessionsThisWeek)
				{
					totalTimeCodedThisWeek += sessionThisWeek.Duration;
				}
				int daysLeft = 7 - (int)DateTime.Today.DayOfWeek + 1;
				int goal = (goalTime.Days * 24) + goalTime.Hours;
				int totalTimeCoded = (totalTimeCodedThisWeek.Days * 24) + totalTimeCodedThisWeek.Hours;
				if (goalTime <= totalTimeCodedThisWeek)
				{
					Console.WriteLine($"Goal achieved, you coded {totalTimeCoded} hours this week, while the goal was {goal} hours");
				}
				else
				{
					Console.WriteLine($"Progress: {totalTimeCoded} out of {goal} hours");
					int remainingHours = goal - totalTimeCoded;
					Console.WriteLine($"Code for another {remainingHours} hours to reach the weekly goal");
					int hoursPerDayLeft = remainingHours / daysLeft;
					if (hoursPerDayLeft == 0)
					{
						int minutesPerDayLeft = (remainingHours*60)/daysLeft;
						Console.WriteLine($"To achieve the goal, you need to code at least {minutesPerDayLeft} minutes per day for the remainder of the week (today included)");
					}
					else if ( hoursPerDayLeft < 24) 
					{
						Console.WriteLine($"To achieve the goal, you need to code at least {hoursPerDayLeft} hours per day for the remainder of the week (today included)");
					}
					else
					{
                        Console.WriteLine("It will be impossible to achieve the goal, there's not enough time left this week");
                    }
					
				}
				connection.Close();
			}
		}
		internal static List<CodingSession> BuildList(string connectionString, string sqlcommand)
		{
			List<CodingSession> sessions = new List<CodingSession>();
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = sqlcommand;

				SqliteDataReader reader = command.ExecuteReader();
				if (reader.HasRows)
				{
					sessions.Clear();
					while (reader.Read())
					{
						sessions.Add(new CodingSession
						{
							Id = reader.GetInt32(0),
							StartTime = DateTime.ParseExact(reader.GetString(1), "dd/MM/yy HH:mm", new CultureInfo("nl-BE")),
							EndTime = DateTime.ParseExact(reader.GetString(2), "dd/MM/yy HH:mm", new CultureInfo("nl-BE")),
							Duration = TimeSpan.ParseExact(reader.GetString(3), @"hh\:mm", new CultureInfo("nl-BE"))
						});
					}
				}
				else
				{
					Console.WriteLine("Database is empty");
				}
				connection.Close();
			}
			return sessions;
		}
		internal static void OverviewTable(string connectionString)
		{
			Console.Clear();
			string sqlCommand = "SELECT * FROM CodeTracker";
			List<CodingSession> sessions = new List<CodingSession>();
			sessions.Clear();
			sessions = BuildList(connectionString, sqlCommand);
			PrintTable(sessions);
		}
		internal static void PrintTable(List<CodingSession> sessions)
		{
			ConsoleTableBuilder
				.From(sessions)
				.WithTitle("Coding Tracker")
				.WithColumn("Id", "Start", "End", "Time")
				.WithFormatter(1, f => $"{f:dd/MM/yy HH:mm}")
				.WithFormatter(2, f => $"{f:dd/MM/yy HH:mm}")
				.WithFormatter(3, f => $@"{f:hh\:mm}")
				.ExportAndWriteLine();
		}

		
	}
}
