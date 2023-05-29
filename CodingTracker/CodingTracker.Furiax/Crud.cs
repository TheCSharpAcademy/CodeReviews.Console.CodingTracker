using CodingTracker.Furiax.Model;
using Microsoft.Data.Sqlite;
using System.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
	internal static void DeleteRecord()
		{
			Console.Clear();
			Console.WriteLine("delete");
		}

		internal static void UpdateRecord()
		{
			Console.Clear();
			Console.WriteLine("update");
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
							EndTime = DateTime.ParseExact(reader.GetString(2), "dd/MM/yy HH:mm", new CultureInfo("nl-BE"))
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
			DateTime inputStartDate = UserInput.InputStartDate("Please enter the start time in the following format dd/mm/yy hh:mm");
			string startDate =inputStartDate.ToString("dd/MM/yy HH:mm");
			DateTime inputEndDate = UserInput.InputEndDate("Please enter the end time in the following format dd/mm/yy hh:mm", inputStartDate);
			string endDate = inputEndDate.ToString("dd/MM/yy HH:mm");
			TimeSpan timeBetween = inputEndDate - inputStartDate;

			string duration = timeBetween.ToString(@"hh\:mm");

			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = $"INSERT INTO CodeTracker (StartTime, EndTime, Duration) VALUES ('{startDate}','{endDate}', '{duration}')";
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
	}
}
