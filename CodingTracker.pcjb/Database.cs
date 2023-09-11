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

    public bool CreateCodingSession(CodingSession session)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            INSERT INTO coding_sessions
            (start, end, duration) 
            VALUES 
            ($start, $end, $duration)
            ";
            command.Parameters.AddWithValue("$start", session.StartTime);
            command.Parameters.AddWithValue("$end", session.EndTime);
            command.Parameters.AddWithValue("$duration", session.Duration.TotalSeconds);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public CodingSession? ReadCodingSession(long id)
    {
        CodingSession? session = null;
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT start, end, duration 
            FROM coding_sessions
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var start = reader.GetDateTime(0);
                var end = reader.GetDateTime(1);
                var duration = TimeSpan.FromSeconds(reader.GetDouble(2));
                session = new CodingSession(id, start, end, duration);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return session;
    }

    public List<CodingSession> ReadAllCodingSessions()
    {
        List<CodingSession> sessions = new();
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, start, end, duration 
            FROM coding_sessions
            ORDER BY start ASC
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt64(0);
                var start = reader.GetDateTime(1);
                var end = reader.GetDateTime(2);
                var duration = TimeSpan.FromSeconds(reader.GetDouble(3));
                sessions.Add(new CodingSession(id, start, end, duration));
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return sessions;
    }

    public bool UpdateCodingSession(CodingSession session)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            UPDATE coding_sessions
            SET start=$start, end=$end, duration=$duration
            WHERE id=$id
            ";
            command.Parameters.AddWithValue("$id", session.Id);
            command.Parameters.AddWithValue("$start", session.StartTime);
            command.Parameters.AddWithValue("$end", session.EndTime);
            command.Parameters.AddWithValue("$duration", session.Duration.TotalSeconds);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool DeleteCodingSession(long id)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            DELETE FROM coding_sessions
            WHERE id=$id
            ";
            command.Parameters.AddWithValue("$id", id);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
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