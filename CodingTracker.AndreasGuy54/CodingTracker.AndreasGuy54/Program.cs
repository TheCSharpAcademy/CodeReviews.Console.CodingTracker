using CodingTracker.AndreasGuy54;
using Microsoft.Data.Sqlite;

string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connString");

using (SqliteConnection connection = new SqliteConnection(connectionString))
{
    connection.Open();

    SqliteCommand tableCmd = connection.CreateCommand();
    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_hours(
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        StartTime TEXT,            
        EndTime TEXT,
        Duration TEXT)";


    tableCmd.ExecuteNonQuery();

    connection.Close();
}

UserInput.GetUserInput();

Console.ReadLine();