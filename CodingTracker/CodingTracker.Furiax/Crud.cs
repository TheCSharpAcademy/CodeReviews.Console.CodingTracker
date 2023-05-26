using Microsoft.Data.Sqlite;
using System.Configuration;

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

		internal static void ShowTable()
		{
			Console.Clear();
			Console.WriteLine("view data");
		}

		internal static void InsertRecord(string connectionString)
		{
			Console.Clear();
			DateTime inputStartDate = UserInput.InputStartDate("Please enter the start time in the following format dd/mm/yy hh:mm");
			string startDate =inputStartDate.ToString("dd/MM/yy HH:mm");
			DateTime inputEndDate = UserInput.InputEndDate("Please enter the end time in the following format dd/mm/yy hh:mm", inputStartDate);
			string endDate = inputStartDate.ToString("dd/MM/yy HH:mm");
			TimeSpan timeBetween = inputEndDate - inputStartDate;
			string duration = timeBetween.ToString("h'u'mm'm'");

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
