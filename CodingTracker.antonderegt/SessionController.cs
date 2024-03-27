using System.Data.SQLite;
using Dapper;

namespace CodingTracker;

public class SessionController
{
    private readonly string _connectionString;

    public SessionController(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int CreateSession(CodingSession session)
    {
        var parameters = new { session.StartTime, session.EndTime, session.Duration };

        string sql =
        @"
            INSERT INTO CodingTracker (
                StartTime,
                EndTime,
                Duration
            )
            VALUES (
                @StartTime,
                @EndTime,
                @Duration
            );
        ";

        using var connection = new SQLiteConnection(_connectionString);
        int numberOfRowsAffected = connection.Execute(sql, parameters);

        return numberOfRowsAffected;
    }

    public bool UpdateSession(CodingSession session)
    {
        var parameters = new { session.Id, session.StartTime, session.EndTime, session.Duration };

        string sql =
        @"
            UPDATE CodingTracker
            SET StartTime = @StartTime,
                EndTime = @EndTime,
                Duration = @Duration
            WHERE Id = @Id;
        ";

        using var connection = new SQLiteConnection(_connectionString);
        int result = connection.Execute(sql, parameters);

        return result == 1;
    }

    public bool DeleteSession(CodingSession session)
    {
        return DeleteSession(session.Id);
    }

    public bool DeleteSession(int id)
    {
        var parameters = new { id };

        string sql =
        @"
            DELETE FROM CodingTracker
            WHERE Id = @id;
        ";

        using var connection = new SQLiteConnection(_connectionString);
        int result = connection.Execute(sql, parameters);

        return result == 1;
    }

    public IEnumerable<CodingSession> GetAllSessions()
    {
        string sql =
        @"
            SELECT *
            FROM CodingTracker;
        ";

        using var connection = new SQLiteConnection(_connectionString);
        return connection.Query<CodingSession>(sql);
    }

    public TimeSpan GetTotalCodingTime()
    {
        string sql =
        @"
            SELECT Duration
            FROM CodingTracker;
        ";

        using var connection = new SQLiteConnection(_connectionString);
        var sum = connection.Query<string>(sql).Select(TimeSpan.Parse);
        var total = new TimeSpan(sum.Sum(x => x.Ticks));

        return total;
    }
}