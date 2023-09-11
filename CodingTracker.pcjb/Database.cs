using Microsoft.Data.Sqlite;

namespace CodingTracker;

class Database
{
    private readonly string databaseFilename;

    public Database(string? databaseFilename)
    {
        if (String.IsNullOrEmpty(databaseFilename))
        {
            throw new ArgumentException($"Invalid database filename: {databaseFilename}");
        }
        this.databaseFilename = databaseFilename;
    }

    public bool CreateDatabaseIfNotPresent()
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();

            var createTableCodingSessionsCmd = connection.CreateCommand();
            createTableCodingSessionsCmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS coding_sessions (
                id INTEGER PRIMARY KEY,
                start TEXT NOT NULL,
                end TEXT NOT NULL,
                duration INTEGER NOT NULL
            )
            ";
            createTableCodingSessionsCmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    private string GetConnectionString()
    {
        return String.Format("Data Source={0}", databaseFilename);
    }
}