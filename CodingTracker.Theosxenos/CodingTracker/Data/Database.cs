namespace CodingTracker.Data;

public class Database
{
    private readonly Configuration appConfiguration;
    private readonly string connectionString;
    private readonly string databasePath;

    public Database()
    {
        appConfiguration = new Configuration();
        databasePath = appConfiguration.GetConfigurationItemByKey("DatabasePath");
        var configConnectionString = appConfiguration.GetConfigurationItemByKey("ConnectionString");
        connectionString = configConnectionString + databasePath;
    }

    public void Initialize()
    {
        if (File.Exists(databasePath)) return;

        SeedDatabase();
    }

    public SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(connectionString);

        try
        {
            connection.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine($"A problem occured during database opening: {e.Message}");
        }

        return connection;
    }

    private void SeedDatabase()
    {
        using var connection = GetConnection();

        var createTableQuery =
            """
                CREATE TABLE IF NOT EXISTS Sessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Day TEXT NOT NULL UNIQUE
                );
            
                CREATE TABLE IF NOT EXISTS Logs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SessionId INTEGER NOT NULL,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration INTEGER,
                    FOREIGN KEY (SessionId) REFERENCES Sessions(Id) ON DELETE CASCADE
                );
            """;

        var createTableCommand = new SqliteCommand(createTableQuery, connection);
        createTableCommand.ExecuteNonQuery();

        var seedDataQuery =
            """
                -- Seed sessions
                INSERT INTO Sessions (Day) VALUES
                ('1-1-24'),
                ('2-1-24'),
                ('10-2-24');
                
                -- Seed logs
                INSERT INTO Logs (SessionId, StartTime, EndTime, Duration) VALUES
                (1, '09:00', '10:00', 36000000000),
                (1, '10:00', '11:00', 36000000000),
                (2, '11:00', '12:00', 36000000000),
                (2, '12:00', '13:00', 36000000000),
                (3, '13:00', '14:00', 36000000000),
                (3, '15:00', '16:55', 69000000000);
            """;

        var seedDataCommand = new SqliteCommand(seedDataQuery, connection);
        seedDataCommand.ExecuteNonQuery();
    }
}