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

    public void LoadDumpData(int limit)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();

            for (var i = 0; i < limit; i++)
            {
                var startTime = DateTime.UtcNow.AddDays(i);
                var endTime = DateTime.UtcNow.AddDays(i).AddMinutes(new Random().Next(1, 361));
                var duration = Math.Round((endTime - startTime).TotalHours, 2);

                tableCmd.CommandText =
                    $@"INSERT INTO sessions(startTime, endTime, duration) VALUES('{Helpers.PareDateToDbFormat(startTime)}', '{Helpers.PareDateToDbFormat(endTime)}', '{duration}')";
                tableCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Dump data couldn't be loaded.");
        }
    }
}