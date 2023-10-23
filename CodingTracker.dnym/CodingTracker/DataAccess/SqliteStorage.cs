using CodingTracker.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTracker.DataAccess;

internal class SqliteStorage : IDataAccess
{
    private const string _createTableSql = """
        CREATE TABLE IF NOT EXISTS "sessions" (
        	"id" INTEGER NOT NULL UNIQUE,
        	"starttime" TEXT NOT NULL,
        	"endtime" TEXT NOT NULL,
        	PRIMARY KEY("id" AUTOINCREMENT)
        );
        """;
    private const string _insertSessionSql = """INSERT INTO "sessions" ("starttime", "endtime") VALUES (@starttime, @endtime);""";
    private const string _selectSessionSql = """SELECT "id", "starttime", "endtime" FROM "sessions" WHERE "id" = @id;""";
    private const string _selectSubsetSql = """SELECT "id", "starttime", "endtime" FROM "sessions" ORDER BY "starttime" @order LIMIT @limit OFFSET @skip;""";
    private const string _countSessionsSql = """SELECT COUNT(*) FROM "sessions";""";
    private const string _updateSessionSql = """UPDATE "sessions" SET "starttime" = @starttime, "endtime" = @endtime WHERE "id" = @id;""";
    private const string _deleteSessionSql = """DELETE FROM "sessions" WHERE "id" = @id;""";
    private const string _overlapTestSql = """
        SELECT "id", "starttime", "endtime" FROM "sessions"
        WHERE ("starttime" <= @starttime AND @starttime <= "endtime")
        OR ("starttime" <= @endtime AND @endtime <= "endtime")
        OR (@starttime <= "starttime" AND "starttime" <= @endtime)
        OR (@starttime <= "endtime" AND "endtime" <= @endtime)
        ORDER BY "starttime" ASC;
        """;

    private readonly string _connectionString;

    public SqliteStorage(string connectionString)
    {
        _connectionString = connectionString;

        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, "create table");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _createTableSql;
        TryOrDie(() => cmd.ExecuteNonQuery(), "create table");

        connection.Close();
    }

    public void Insert(CodingSession session)
    {
        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, "insert session");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _insertSessionSql;
        cmd.Parameters.AddWithValue("@starttime", session.StartTime.ToString("O"));
        cmd.Parameters.AddWithValue("@endtime", session.EndTime.ToString("O"));
        TryOrDie(() => cmd.ExecuteNonQuery(), "insert session");

        connection.Close();
    }

    public CodingSession? Get(int id)
    {
        CodingSession? output = null;

        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, $"get session with id={id}");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _selectSessionSql;
        cmd.Parameters.AddWithValue("@id", id);
        TryOrDie(() => {
            using var reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                var _id = reader.GetInt32(0);
                var startTime = reader.GetString(1);
                var endTime = reader.GetString(2);
                output = new CodingSession
                {
                    Id = _id,
                    StartTime = DateTime.ParseExact(startTime, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    EndTime = DateTime.ParseExact(endTime, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                };
            }
        }, $"get session with id={id}", $"Session with id={id} has bad data");

        connection.Close();
        return output;
    }

    public IList<CodingSession> GetAll(IDataAccess.Order order = IDataAccess.Order.Ascending, int skip = 0, int limit = int.MaxValue)
    {
        var output = new List<CodingSession>();

        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, "get sessions");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _selectSubsetSql.Replace("@order", order == IDataAccess.Order.Ascending ? "ASC" : "DESC");
        cmd.Parameters.AddWithValue("@limit", limit);
        cmd.Parameters.AddWithValue("@skip", skip);
        TryOrDie(() => {
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                output.Add(new CodingSession
                {
                    Id = reader.GetInt32(0),
                    StartTime = DateTime.ParseExact(reader.GetString(1), "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    EndTime = DateTime.ParseExact(reader.GetString(2), "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                });
            }
        }, "get sessions", "Session has bad data");

        connection.Close();
        return output;
    }

    public int Count()
    {
        var output = 0;

        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, "count sessions");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _countSessionsSql;
        TryOrDie(() => {
            if (cmd.ExecuteScalar() is long result)
            {
                output = (int)result;
            }
            else
            {
                Console.WriteLine("Failed to count sessions!\nAborting!");
                Environment.Exit(1);
            }
        }, "count sessions");

        connection.Close();
        return output;
    }

    public void Update(CodingSession session)
    {
        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, $"update session with id={session.Id}");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _updateSessionSql;
        cmd.Parameters.AddWithValue("@id", session.Id);
        cmd.Parameters.AddWithValue("@starttime", session.StartTime.ToString("O"));
        cmd.Parameters.AddWithValue("@endtime", session.EndTime.ToString("O"));
        TryOrDie(() => cmd.ExecuteNonQuery(), $"update session with id={session.Id}");

        connection.Close();
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, $"delete session with id={id}");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _deleteSessionSql;
        cmd.Parameters.AddWithValue("@id", id);
        TryOrDie(() => cmd.ExecuteNonQuery(), $"delete session with id={id}");

        connection.Close();
    }

    public IList<CodingSession> CheckForOverlap(CodingSession session)
    {
        var output = new List<CodingSession>();

        using var connection = new SqliteConnection(_connectionString);
        TryOrDie(connection.Open, "get overlapping sessions");

        var cmd = connection.CreateCommand();
        cmd.CommandText = _overlapTestSql;
        cmd.Parameters.AddWithValue("@starttime", session.StartTime.ToString("O"));
        cmd.Parameters.AddWithValue("@endtime", session.EndTime.ToString("O"));
        TryOrDie(() =>
        {
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                output.Add(new CodingSession
                {
                    Id = reader.GetInt32(0),
                    StartTime = DateTime.ParseExact(reader.GetString(1), "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    EndTime = DateTime.ParseExact(reader.GetString(2), "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                });
            }
        }, "get overlapping sessions", "Session has bad data");

        connection.Close();
        return output;
    }

    private static void TryOrDie(Action action, string purpose, string? formatError = null)
    {
        if (formatError == null)
        {
            try
            {
                action.Invoke();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
        else
        {
            try
            {
                action.Invoke();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"{formatError}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
    }
}
