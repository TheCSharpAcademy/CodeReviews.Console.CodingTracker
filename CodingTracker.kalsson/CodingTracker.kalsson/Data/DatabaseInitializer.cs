using System.Data.SQLite;
using Dapper;

namespace CodingTracker.kalsson.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    /// <summary>
    /// Class responsible for initializing the database.
    /// </summary>
    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Initializes the database by creating the necessary table if it does not already exist.
    /// </summary>
    /// <remarks>
    /// The method uses the provided connection string to establish a connection to the database.
    /// It then executes a SQL query to create the "CodingSessions" table if it does not already exist.
    /// If any error occurs during the initialization process, an error message is printed to the console.
    /// </remarks>
    public void InitializeDatabase()
    {
        try
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var tableCreationQuery = @"
                    CREATE TABLE IF NOT EXISTS CodingSessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT NOT NULL
                    );";
            connection.Execute(tableCreationQuery);
            Console.WriteLine("Database initialized and table created if not already present.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
        }
    }
}