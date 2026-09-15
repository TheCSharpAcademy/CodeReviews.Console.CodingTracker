using System.Collections.Immutable;
using Microsoft.Data.Sqlite;
using Dapper;

public class DataAccess
{
    string _ConnectionString;
    
    public DataAccess(string Connectionstring)
    {
        _ConnectionString = Connectionstring;
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_ConnectionString);
        connection.Open();

        connection.Execute("""
            CREATE TABLE IF NOT EXISTS CodingSessions(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT NOT NULL,
            EndTime TEXT NOT NULL,
            Duration TEXT NOT NULL,
            Description TEXT)
        """);
    }

    public void CreateSession(Session session)
    {
        var sql = """
        INSERT INTO CodingSessions (StartTime, EndTime, Duration, Description)
        VALUES (@StartTime, @EndTime, @Duration, @Description)
        """;
        using var connection = new SqliteConnection(_ConnectionString);
        connection.Execute(sql, session);
    }

    public void DeleteSession(int SessionId)
    {
        var sql = """
        DELETE FROM CodingSessions WHERE Id = @Id
        """;
        using var connection = new SqliteConnection(_ConnectionString);
        connection.Execute(sql, new {Id = SessionId});
    }

    public void UpdateSession(Session session)
    {
        var sql = """
        UPDATE CodingSessions
        SET StartTime = @StartTime, EndTime = @EndTime,
        Duration = @Duration, Description = @Description
        WHERE
        Id = @Id
        """;
        using var connection = new SqliteConnection(_ConnectionString);
        connection.Execute(sql, session);
    }

    public List<Session> ReadSessions()
    {
        var sql = """
        SELECT * FROM CodingSessions 
        """;
        using var connection = new SqliteConnection(_ConnectionString);
        
        var sessions = connection.Query<Session>(sql);
        
        return sessions.ToList();
    }
}