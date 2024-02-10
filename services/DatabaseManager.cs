using CodingTracker.models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.services;

public class DatabaseManager
{
    private readonly string _connectionString;
    
    public DatabaseManager()
    {
        _connectionString = AppConfig.GetConnectionString();
    }

    internal void CreateDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        
        const string createTableQuery = """
                                            CREATE TABLE IF NOT EXISTS records (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            StartTime DATETIME NOT NULL,
                                            EndTime DATETIME NOT NULL,
                                            Duration INTEGER NOT NULL
                                        )
                                        """;

        connection.Execute(createTableQuery);
    }

    public List<CodingSession> GetAllCodingSessions()
    {
        using var connection = new SqliteConnection(_connectionString);
        return connection.Query<CodingSession>("SELECT * FROM records").ToList();
    }

    public List<CodingSession> GetCodingSessionsByDate(DateTime start, DateTime end)
    {
        var connection = new SqliteConnection(_connectionString);
        return connection.Query<CodingSession>("SELECT * FROM records WHERE StartTime >= @Start AND EndTime <= @End",
            new {Start = start, End = end}).ToList();
    }
}