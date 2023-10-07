using Microsoft.Data.Sqlite;

namespace CodingTracker.wkktoria;

public class DbManager
{
    private readonly string _connectionString;

    public DbManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Initialize()
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = """
                                   CREATE TABLE IF NOT EXISTS sessions(
                                       Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                       StartTime TEXT,
                                       EndTime TEXT,
                                       Duration REAL
                                   )
                                   """;
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Database couldn't be initialized.");
        }
    }
}