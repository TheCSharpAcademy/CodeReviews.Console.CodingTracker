using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Globalization;

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
                VALUES ('{startDate:MM/dd/yy hh:mm}')";

            command.ExecuteNonQuery();
        }
    }

    public static List<CodingSessionModel> GetAllSessions()
    {
        List<CodingSessionModel> sessions = new();
        using (var connection = new SqliteConnection(ConnString("CodingDb")))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                @$"SELECT * FROM CodingSession";
            
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CodingSessionModel session = new();
                session.SessionId = reader.GetInt32(0);
                session.StartTime = DateTime.ParseExact(reader.GetString(1), 
                                                        "MM/dd/yy hh:mm",
                                                        new CultureInfo("en-US"));
                sessions.Add(session);
            }
        }
        return sessions;
    }
}
