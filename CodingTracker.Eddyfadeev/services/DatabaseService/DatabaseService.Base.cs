using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.services;

/// <summary>
/// Provides methods for interacting with the database and performing CRUD operations on coding sessions.
/// </summary>
internal partial class DatabaseService
{
    /// <summary>
    /// Represents the connection string used to connect to the database.
    /// </summary>
    private readonly string _connectionString = AppConfig.GetConnectionString();
    
    /// <summary>
    /// Retrieves a connection to the database.
    /// </summary>
    /// <returns>A <see cref="SqliteConnection"/> object representing a connection to the database.</returns>
    private SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        return connection;
    }

    /// <summary>
    /// Initializes the database by creating the necessary table if it doesn't exist and optionally seeds it with sample data in debug mode.
    /// </summary>
    /// <remarks>
    /// This method should be called before any other CRUD operations are performed on the database.
    /// </remarks>
    internal void InitializeDatabase()
    {
        using var connection = GetConnection();
        
        const string createTableQuery = """
                                            CREATE TABLE IF NOT EXISTS records (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            StartTime DATETIME NOT NULL,
                                            EndTime DATETIME NOT NULL,
                                            Duration INTEGER NOT NULL
                                        )
                                        """;

        connection.Execute(createTableQuery);
        
        #if DEBUG
        //SeedData.SeedSessions(100);
        #endif
    }
}