namespace CodingTracker.kalsson.Data;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(string connectionString)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Check if the 'CodingSessions' table exists and create it if not
            var tableExists = connection.ExecuteScalar<string>(
                "SELECT name FROM sqlite_master WHERE type='table' AND name='CodingSessions';");

            if (string.IsNullOrEmpty(tableExists))
            {
                connection.Execute(
                    @"CREATE TABLE CodingSessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT NOT NULL
                      );");
                Console.WriteLine("Database and table 'CodingSessions' created.");
            }
            else
            {
                Console.WriteLine("Database already initialized.");
            }
        }
    }
}
