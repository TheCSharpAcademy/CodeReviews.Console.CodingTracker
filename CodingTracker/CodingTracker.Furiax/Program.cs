using Microsoft.Data.Sqlite;
using System.Configuration;

var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

using(var connection = new SqliteConnection(connectionString))
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