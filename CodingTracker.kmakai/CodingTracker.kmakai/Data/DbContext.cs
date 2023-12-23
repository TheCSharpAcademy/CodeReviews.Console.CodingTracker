using CodingTracker.kmakai.Models;
using System.Data.SQLite;

namespace CodingTracker.kmakai.Data;


public class DbContext
{
    public string? ConnectionString;
    public string? DbPath;

    public DbContext()
    {
        ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");
        DbPath = ConfigurationManager.AppSettings.Get("dbPath");
        CreateDatabase();
    }

    public void CreateDatabase()
    {
        if (ConnectionString is null || DbPath is null)
        {
            throw new Exception("Connection string or database path is not set.");
        }

        if (!File.Exists(DbPath))
        {
            SQLiteConnection.CreateFile(DbPath);
        }

        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS CodeSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL
                );
            ";

            tableCommand.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Add(CodeSession codeSession)
    {
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                INSERT INTO CodeSessions (Date, StartTime, EndTime)
                VALUES ('{codeSession.Date}', '{codeSession.StartTime}', '{codeSession.EndTime}');
            ";

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Update(CodeSession codeSession)
    {
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                UPDATE CodeSessions
                SET Date = '{codeSession.Date}', StartTime = '{codeSession.StartTime}', EndTime = '{codeSession.EndTime}'
                WHERE Id = '{codeSession.Id}';
            ";

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Delete(int id)
    {
        if (id <= 0)
        {
            throw new Exception("Id is invalid.");
        }

        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                DELETE FROM CodeSessions
                WHERE Id = {id};
            ";

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public CodeSession GetCodeSession(int id)
    {
        if (id <= 0)
        {
            throw new Exception("Id is invalid.");
        }

        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT Id, Date, StartTime, EndTime
                FROM CodeSessions
                WHERE Id = {id};
            ";

            var reader = command.ExecuteReader();
            var codeSession = new CodeSession();
            while (reader.Read())
            {
                codeSession.Id = reader.GetInt32(0);
                codeSession.Date = reader.GetString(1);
                codeSession.StartTime = reader.GetString(2);
                codeSession.EndTime = reader.GetString(3);
            }

            connection.Close();
            return codeSession;
        }
    }

    public List<CodeSession> GetCodeSessions()
    {
        var codeSessions = new List<CodeSession>();

        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT Id, Date, StartTime, EndTime
                FROM CodeSessions;
            ";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var codeSession = new CodeSession();
                codeSession.Id = reader.GetInt32(0);
                codeSession.Date = reader.GetString(1);
                codeSession.StartTime = reader.GetString(2);
                codeSession.EndTime = reader.GetString(3);
                codeSessions.Add(codeSession);
            }

            connection.Close();
            return codeSessions;
        }
    }

    public int GetLastId()
    {
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT Id
                FROM CodeSessions
                ORDER BY Id DESC
                LIMIT 1;
            ";

            var reader = command.ExecuteReader();
            var id = 0;
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

            connection.Close();
            return id;
        }
    }

}