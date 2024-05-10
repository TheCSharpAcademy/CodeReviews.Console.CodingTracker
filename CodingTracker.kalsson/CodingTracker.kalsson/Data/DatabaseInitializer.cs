namespace CodingTracker.kalsson.Data;

public static class DatabaseInitializer
{
    /// <summary>
    /// Creates the 'CodingSessions' table in the SQLite database if it doesn't exist.
    /// </summary>
    /// <param name="connection">The SQLite connection object.</param>
    private static void CreateCodingSessionsTable(SQLiteConnection connection)
    {
        connection.Execute(
            @"CREATE TABLE CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL
            );");
        Console.WriteLine("Database and table 'CodingSessions' created.");
    }

    /// <summary>
    /// Initializes the database by creating the 'CodingSessions' table if it doesn't exist.
    /// </summary>
    /// <param name="connectionString">The connection string for the SQLite database.</param>
    public static void InitializeDatabase(string connectionString)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            var tableExists = connection.ExecuteScalar<string>(
                "SELECT name FROM sqlite_master WHERE type='table' AND name='CodingSessions';"
            );

            if (string.IsNullOrEmpty(tableExists))
            {
                CreateCodingSessionsTable(connection);
            }
            else
            {
                Console.WriteLine("Database already initialized.");
            }
        }
    }
}
