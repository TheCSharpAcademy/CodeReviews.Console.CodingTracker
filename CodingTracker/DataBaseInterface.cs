using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker;

public static class DataBaseInterface
{
    private static readonly SqliteConnection DbConnection;

    static DataBaseInterface()
    {
        string dbPath = ReadConfig.Config.DbPath;
        DbConnection = new SqliteConnection(dbPath);
        DbConnection.Open();
        CreateTable();
    }

    private static void CreateTable()
    {
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS CodingSessions (
                Id INT PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration INT NOT NULL);";
        
        using var command = new SqliteCommand(createTableQuery, DbConnection);
        command.ExecuteNonQuery();
    }

    public static async Task AddCodingSession(CodingSession codingSession)
    {
        string addCodingSessionQuery = @"
            INSERT INTO CodingSessions (StartTime, EndTime, Duration) 
            VALUES (@StartTime, @EndTime, @Duration);";

        await DbConnection.ExecuteAsync(addCodingSessionQuery, codingSession);
    }
    
    public static async Task<List<CodingSession>> ViewCodingSessions(DateTime? minStartDate = null, DateTime? maxStartDate = null)
    {
        var conditions = new List<string>();
        if (minStartDate.HasValue) conditions.Add("StartTime >= @MinDate");
        if (maxStartDate.HasValue) conditions.Add("StartTime <= @MaxDate");
        
        string whereClauses = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "";
        
        string viewCodingSessionsQuery = $"SELECT * FROM CodingSessions{whereClauses} ORDER BY StartTime;";
        
        return (await DbConnection.QueryAsync<CodingSession>(viewCodingSessionsQuery,
            new { MinDate = minStartDate, MaxDate = maxStartDate })).ToList();
    }

}