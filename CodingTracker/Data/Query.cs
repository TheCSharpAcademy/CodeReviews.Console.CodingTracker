using Dapper;
using Microsoft.Data.Sqlite;
using static CodingTracker.Repository;

namespace CodingTracker;

internal class Query
{
  internal static List<CodingSession> ViewAll()
  {
    using var connection = new SqliteConnection(ConnectionString);
    string sql = @$"SELECT * FROM coding_tracker ORDER BY startTime DESC";

    return connection.Query<CodingSession>(sql).ToList();
  }

  internal static int CountRows()
  {
    using var connection = new SqliteConnection(ConnectionString);
    string sql = @$"SELECT COUNT(*) FROM coding_tracker";

    return Convert.ToInt32(connection.ExecuteScalar(sql));
  }

  internal static int RowFromID(int id)
  {
    using var connection = new SqliteConnection(ConnectionString);
    string sql = @$"SELECT * FROM coding_tracker WHERE id = @id";

    return Convert.ToInt32(connection.ExecuteScalar(sql, new { id }));
  }

  internal static List<CodingSession> FilterByActivity(string filter)
  {
    using var connection = new SqliteConnection(ConnectionString);
    string sql = @$"SELECT * FROM coding_tracker WHERE activity LIKE @filter ORDER BY startTime DESC";

    return connection.Query<CodingSession>(sql, new { filter = $"%{filter}%" }).ToList();
  }

  internal static List<CodingSession> FilterByDate(string startDate, string endDate)
  {
    using var connection = new SqliteConnection(ConnectionString);
    string sql = @$"SELECT * FROM coding_tracker 
                    WHERE startTime BETWEEN @startDate AND @endDate
                    ORDER BY startTime DESC";

  internal static string LastActivity()
  {
    string sql = @$"SELECT activity FROM coding_tracker 
                    ORDER BY id DESC
                    LIMIT 1";

    using var connection = new SqliteConnection(ConnectionString);
    return Convert.ToString(connection.ExecuteScalar(sql));
  }
}