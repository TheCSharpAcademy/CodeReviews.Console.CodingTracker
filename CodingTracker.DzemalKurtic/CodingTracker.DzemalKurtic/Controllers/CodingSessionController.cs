using CodingTracker.DzemalKurtic.Models;
using CodingTracker.DzemalKurtic.Views;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.DzemalKurtic.Controllers;

internal class CodingSessionController 
{
    public string ConnectionString { get; set; }
    public CodingSessionController(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public void AddItem(DateTime start, DateTime end)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var sql = 
            """
            INSERT INTO coding_sessions
            (StartTime, EndTime)
            VALUES (@StartTime, @EndTime)
            """;

        var session = new CodingSession { StartTime = start, EndTime = end };

        connection.Execute(sql, session);
    }

    public void DeleteItem(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var sql =
            """
            DELETE FROM coding_sessions
            WHERE Id = @Id;
            """;

        connection.Execute(sql, new { Id = id });
    }

    public void UpdateItem(int id, DateTime start, DateTime end)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var sql =
            """
            UPDATE coding_sessions SET StartTime = @StartTime, EndTime = @EndTime 
            WHERE Id = @Id
            """;
        var session = new CodingSession { Id = id, StartTime = start, EndTime = end };

        connection.Execute(sql, session);
    }

    public List<CodingSession> ViewItems()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var sql = "SELECT * FROM coding_sessions";
        var sessions = connection.Query<CodingSession>(sql).ToList();

        return sessions;
    }
}
