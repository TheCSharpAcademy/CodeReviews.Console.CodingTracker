using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTrackerLibrary;
public static class CrudController
{
    private static string ConnString(string name)
    {
        return ConfigurationManager.ConnectionStrings[name].ConnectionString;
    }

    public static void InitDatabase()
    {
        using (var connection = new SqliteConnection(ConnString("CodingDb")))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                @$"CREATE TABLE IF NOT EXISTS CodingSession
                (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                Duration TEXT)";

            command.ExecuteNonQuery();
        }
    }

    public static void CreateSession(DateTime startDate)
    {
        using (var connection = new SqliteConnection(ConnString("CodingDb")))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                @$"INSERT INTO CodingSession (StartTime)
                VALUES ('{startDate:MM/dd/yy}')";

            command.ExecuteNonQuery();
        }
    }
}
