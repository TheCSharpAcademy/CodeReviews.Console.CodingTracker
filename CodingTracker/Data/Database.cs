using CodingSession;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker;

internal class Database
{
  private static string ConnectionString;

  internal static void Configure(string connectionString)
  {
    ConnectionString = connectionString;
  }

  internal static T Execute<T>(Func<SqliteConnection, T> action)
  {
    try
    {
      using var connection = new SqliteConnection(ConnectionString);
      return action(connection);
    }
    catch (SqliteException ex)
    {
      throw new InvalidOperationException("Database access failed.", ex);
    }
  }

  internal static void CreateDB()
  {
    bool tableExisted;

    using (var connection = new SqliteConnection(ConnectionString))
    {
      const string existsSql =
        $@"SELECT EXISTS (SELECT name
        FROM sqlite_master
        WHERE type = 'table'
        AND name = 'coding_tracker')";

      tableExisted = connection.ExecuteScalar<bool>(existsSql);

      string createSql =
        $@"CREATE TABLE IF NOT EXISTS coding_tracker(
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        activity TEXT,
        startTime TEXT,
        endTime TEXT,
        duration INTEGER)";

      connection.Execute(createSql);
    }

    if (!tableExisted)
      DebugTool.SeedData();
  }

  internal static void Add(string activity, string startTime, string endTime, int duration)
  {
    string sql = @$"INSERT INTO coding_tracker 
                  (activity, startTime, endTime, duration)
                  VALUES (@activity, @startTime, @endTime, @duration)";

    Execute(connection => connection.Execute(sql, new
    {
      activity = activity,
      startTime = startTime,
      endTime = endTime,
      duration = duration
    }));
  }

  internal static void ConvertAndAdd(string activity, DateTime startTime, DateTime endTime, TimeSpan duration)
  {
    string startTimeDB = TimeFormatter.ConvertDateForDb(startTime);
    string endTimeDB = TimeFormatter.ConvertDateForDb(endTime);
    int durationDB = TimeFormatter.MinutesToInt(duration);

    Add(activity, startTimeDB, endTimeDB, durationDB);
  }

  internal static void Delete(CodingSession entry)
  {
    string sql = @$"DELETE FROM coding_tracker WHERE id = @id";
    Execute(connection => connection.Execute(sql, new { id = entry.Id }));
  }

  internal static void UpdateRow(CodingSession entry)
  {
    string sql = @$"UPDATE coding_tracker 
                    SET activity = @activity, 
                        startTime = @startTime, 
                        endTime = @endTime,
                        duration = @duration 
                    WHERE id = @id";

    Execute(connection => connection.Execute(sql, new
    {
      activity = entry.Activity,
      startTime = entry.StartTime,
      endTime = entry.EndTime,
      duration = entry.Duration,
      id = entry.Id
    }));
  }
}
