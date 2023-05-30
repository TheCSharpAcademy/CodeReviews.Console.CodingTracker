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
			ShowTable(connectionString);
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
						command.CommandText = $"DELETE FROM CodeTracker WHERE id = '{recordToDelete}'";
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
			ShowTable(connectionString);
			while(true)
			{
				int recordToUpdate = UserInput.GetId("What record do you want to update: ");
				using(var connection = new SqliteConnection( connectionString))
				{
					connection.Open();
					bool doesIdExist = Validation.CheckIfRecordExists(recordToUpdate, connectionString);
					if (doesIdExist)
					{
						DateTime askNewStartTime = UserInput.GetStartDate("Enter a new value for StartDate");
						string newStartTime = askNewStartTime.ToString("dd/MM/yy HH:mm");
						DateTime askNewEndTime = UserInput.GetEndDate("Enter a new value for EndDate", askNewStartTime);
						string newEndTime = askNewEndTime.ToString("dd/MM/yy HH:mm");
						TimeSpan timeBetween = CalculateDuration(askNewStartTime, askNewEndTime);
						string duration = timeBetween.ToString(@"hh\:mm");
						var command = connection.CreateCommand();
						command.CommandText = $"UPDATE CodeTracker SET StartTime = '{newStartTime}', EndTime = '{newEndTime}', Duration = '{duration}' WHERE Id = '{recordToUpdate}'";
						command.ExecuteNonQuery();
						break;
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
		internal static void ShowTable(string connectionString)
		{
			Console.Clear();
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = "SELECT * from CodeTracker";

				List<CodingSession> sessions = new List<CodingSession>();

				SqliteDataReader reader = command.ExecuteReader();
				if (reader.HasRows)
				{
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
				ConsoleTableBuilder
					.From(sessions)
					.ExportAndWriteLine();
			}
		}
		internal static void InsertRecord(string connectionString)
		{
			Console.Clear();
			DateTime inputStartDate = UserInput.GetStartDate("Please enter the start time in the following format dd/mm/yy hh:mm");
			string startDate =inputStartDate.ToString("dd/MM/yy HH:mm");
			DateTime inputEndDate = UserInput.GetEndDate("Please enter the end time in the following format dd/mm/yy hh:mm", inputStartDate);
			string endDate = inputEndDate.ToString("dd/MM/yy HH:mm");
			TimeSpan timeBetween = CalculateDuration(inputStartDate, inputEndDate);
			string duration = timeBetween.ToString(@"hh\:mm");
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = $"INSERT INTO CodeTracker (StartTime, EndTime, Duration) VALUES ('{startDate}','{endDate}', '{duration}')";
				command.ExecuteNonQuery();
				connection.Close();
			}
            Console.WriteLine("Times succesfully added to database");
        }
		internal static TimeSpan CalculateDuration(DateTime startTime, DateTime endTime)
		{
			TimeSpan duration = endTime - startTime;
			return duration;
		}
	}
}
