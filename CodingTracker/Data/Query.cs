using Dapper;
using Microsoft.Data.Sqlite;
using static CodingTracker.Repository;

namespace CodingTracker;

internal class Query
{
  internal static List<CodingSession> ViewAll()
  {
    string sql = @$"SELECT * FROM coding_tracker ORDER BY startTime DESC";
    return Execute(connection => connection.Query<CodingSession>(sql).ToList());
  }

  internal static int CountRows()
  {
    string sql = @$"SELECT COUNT(*) FROM coding_tracker";
    return Execute(connection => Convert.ToInt32(connection.ExecuteScalar(sql)));
  }

  internal static int RowFromID(int id)
  {
    string sql = @$"SELECT * FROM coding_tracker WHERE id = @id";
    return Execute(connection => Convert.ToInt32(connection.ExecuteScalar(sql, new { id })));
  }

  internal static List<CodingSession> FilterByActivity(string filter)
  {
    string sql = @$"SELECT * FROM coding_tracker WHERE activity LIKE @filter ORDER BY startTime DESC";
    return Execute(connection => connection.Query<CodingSession>(sql, new { filter = $"%{filter}%" }).ToList());
  }

  internal static List<CodingSession> FilterByDate(string startDate, string endDate)
  {
    string sql = @$"SELECT * FROM coding_tracker 
                    WHERE startTime BETWEEN @startDate AND @endDate
                    ORDER BY startTime DESC";
    return Execute(connection => connection.Query<CodingSession>(sql, new { startDate = startDate, endDate = endDate }).ToList());
  }

  internal static string LastActivity()
  {
    string sql = @$"SELECT activity FROM coding_tracker 
                    ORDER BY id DESC
                    LIMIT 1";
    return Execute(connection => Convert.ToString(connection.ExecuteScalar(sql)));
  }
}