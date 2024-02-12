using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Mateusz_Platek;

public static class Database
{
    private static SqliteConnection sqliteConnection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString"));

    public static List<Session> GetSessions()
    {
        List<Session> sessions = new List<Session>();
        
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = "SELECT * FROM sessions";
        SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
        
        if (sqliteDataReader.HasRows)
        {
            while (sqliteDataReader.Read())
            {
                string start = sqliteDataReader.GetString(2);
                DateTime dateTimeStart = DateTime.Parse(start);
                string end = sqliteDataReader.GetString(3);
                DateTime dateTimeEnd = DateTime.Parse(end);
                sessions.Add(new Session(
                    sqliteDataReader.GetInt32(0),
                    sqliteDataReader.GetString(1),
                    dateTimeStart,
                    dateTimeEnd
                ));
            }
        }
        sqliteConnection.Close();
        
        return sessions;
    }

    public static void AddSession(Session session)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        string name = session.name;
        string start = session.GetStart();
        string end = session.GetEnd();
        sqliteCommand.CommandText = $"INSERT INTO sessions (name, start, end) VALUES ('{name}', '{start}', '{end}')";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void UpdateSession(Session session)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        int id = session.id;
        string name = session.name;
        string start = session.GetStart();
        string end = session.GetEnd();
        sqliteCommand.CommandText = $"UPDATE sessions SET name = '{name}', start = '{start}', end = '{end}' WHERE sessionID = {id}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }

    public static void DeleteSession(int id)
    {
        sqliteConnection.Open();
        SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
        sqliteCommand.CommandText = $"DELETE FROM sessions WHERE sessionID = {id}";
        sqliteCommand.ExecuteNonQuery();
        sqliteConnection.Close();
    }
}