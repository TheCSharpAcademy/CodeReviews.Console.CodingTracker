using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTrackerLibrary;
public static class CrudController
{
    private const string conn = "CodingDb";

    private static string ConnString(string name)
    {
        return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
    }

    public static void InitDatabase()
    {
        using (var connection = new SqliteConnection(ConnString(conn)))
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

    public static void InitGoalTable()
    {
        using (var connecton = new SqliteConnection(ConnString(conn)))
        {
            connecton.Open();
            var command = connecton.CreateCommand();

            command.CommandText =
                $@"CREATE TABLE IF NOT EXISTS CodingGoals
                (Name TEXT,
                Time TEXT)";

            command.ExecuteNonQuery();
        }
    }

    public static void CreateSession(DateTime startDate)
    {
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                @$"INSERT INTO CodingSession (StartTime)
                VALUES ('{startDate:g}')";

            command.ExecuteNonQuery();
        }
    }

    public static List<CodingSessionModel> GetAllSessions()
    {
        List<CodingSessionModel> sessions = new();
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                @$"SELECT * FROM CodingSession";
            
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CodingSessionModel session = new()
                {
                    SessionId = reader.GetInt32(0),
                    StartTime = DateTime.ParseExact(reader.GetString(1), "g", new CultureInfo("en-US"))
                };

                if (!reader.IsDBNull(2))
                {
                    session.EndTime = DateTime.ParseExact(reader.GetString(2), "g", new CultureInfo("en-US"));
                }
                sessions.Add(session);
            }
        }
        return sessions;
    }

    public static List<CodingSessionModel> GetOpenSessions()
    {
        List<CodingSessionModel> sessions = new();
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                @$"SELECT * FROM CodingSession 
                WHERE EndTime IS NULL";

            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                CodingSessionModel session = new()
                {
                    SessionId = reader.GetInt32(0),
                    StartTime = DateTime.ParseExact(reader.GetString(1), "g", new CultureInfo("en-US"))
                };
                sessions.Add(session);
            }
        }
        return sessions;
    }

    public static void CloseSession(DateTime endTime, int primaryKey)
    {
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @$"UPDATE CodingSession
                SET EndTime = '{endTime:g}'
                WHERE Id = {primaryKey}";

            command.ExecuteNonQuery();
        }
    }

    public static void DeleteSession(int key)
    {
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @$"DELETE FROM CodingSession
                WHERE Id = {key}";
            command.ExecuteNonQuery();
        }
    }

    public static void CreateLiveSession(DateTime startDate, DateTime endDate)
    {
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                @$"INSERT INTO CodingSession (StartTime, EndTime)
                VALUES ('{startDate:g}', '{endDate:g}')";

            command.ExecuteNonQuery();
        }
    }

    public static void CreateGoal(string name, int goal)
    {
        TimeSpan deadline = TimeSpan.FromDays(goal);
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var delete = connection.CreateCommand();
            delete.CommandText =
                $"DELETE FROM CodingGoals";

            var command = connection.CreateCommand();
            command.CommandText =
                @$"INSERT INTO CodingGoals (Name, Time)
                VALUES ('{name}', '{deadline}')";

            delete.ExecuteNonQuery();
            command.ExecuteNonQuery();
        }
    }

    public static CodingGoalModel GetGoal()
    {
        CodingGoalModel model = new();
        using (var connection = new SqliteConnection(ConnString(conn)))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @$"SELECT * From CodingGoals";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                model.Name = reader.GetString(0);
                model.Goal = TimeSpan.Parse(reader.GetString(1));
            }
        }
        return model;
    }
}
