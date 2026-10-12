using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker;

public static class DataBaseInterface
{
    private static SqliteConnection? _dbConnection;
    private static bool _isInitialized = false;

    private static void InitializeDb()
    {
        if (_isInitialized && _dbConnection != null) return;
        string dbPath = ReadConfig.Config.DbPath;
        string connectionString = "Data Source=" + dbPath;
        _dbConnection = new SqliteConnection(connectionString);
        _dbConnection.Open();
        CreateTable();
        _isInitialized = true;
    }

    private static void CreateTable()
    {
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                DurationTicks INTEGER NOT NULL);";
        
        using var command = new SqliteCommand(createTableQuery, _dbConnection);
        command.ExecuteNonQuery();
    }

    public static async Task AddCodingSession(CodingSession codingSession)
    {
        InitializeDb();
        if (_dbConnection == null) throw new Exception("Database connection couldn't initialize");
        
        string addCodingSessionQuery = @"
            INSERT INTO CodingSessions (StartTime, EndTime, DurationTicks) 
            VALUES (@StartTime, @EndTime, @DurationTicks);";

        await _dbConnection.ExecuteAsync(addCodingSessionQuery, codingSession);
    }
    
    public static async Task<List<CodingSession>> ViewCodingSessions(DateTime? minStartDate = null, DateTime? maxStartDate = null)
    {
        InitializeDb();
        if (_dbConnection == null) throw new Exception("Database connection couldn't initialize");
        
        var conditions = new List<string>();
        if (minStartDate.HasValue) conditions.Add("StartTime >= @MinDate");
        if (maxStartDate.HasValue) conditions.Add("StartTime <= @MaxDate");
        
        string whereClauses = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "";
        
        string viewCodingSessionsQuery = $"SELECT * FROM CodingSessions{whereClauses} ORDER BY StartTime;";
        
        return (await _dbConnection.QueryAsync<CodingSession>(viewCodingSessionsQuery,
            new { MinDate = minStartDate, MaxDate = maxStartDate })).ToList();
    }

}