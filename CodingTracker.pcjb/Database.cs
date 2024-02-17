namespace CodingTracker;

using System.Globalization;
using Microsoft.Data.Sqlite;

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

    public List<CodingSession> ReadAllCodingSessions(SortOrder sortOrder, FilterPeriod filterPeriod)
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
            ";

            DateTime? minStart = null;
            if (filterPeriod != FilterPeriod.None)
            {
                command.CommandText += " WHERE start >= $min_start";
            }
            switch (filterPeriod)
            {
                case FilterPeriod.LastSevenDays:
                    minStart = DateTime.Now.AddDays(-7);
                    break;
                case FilterPeriod.LastFourWeeks:
                    minStart = CultureInfo.InvariantCulture.Calendar.AddWeeks(DateTime.Now, -4);
                    break;
                case FilterPeriod.LastTwelveMonths:
                    minStart = DateTime.Now.AddMonths(-12);
                    break;
            }

            if (sortOrder == SortOrder.Ascending)
            {
                command.CommandText += " ORDER BY start ASC";
            }
            else
            {
                command.CommandText += " ORDER BY start DESC";
            }

            if (filterPeriod != FilterPeriod.None && minStart != null)
            {
                command.Parameters.AddWithValue("$min_start", minStart);
            }

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

    public List<ReportResultTotalAndAverage> GetTotalAndAverage(ReportPeriod reportPeriod)
    {
        string periodPattern = reportPeriod switch {
            ReportPeriod.Week => "%Y-%W",
            ReportPeriod.Month => "%Y-%m",
            _ => "%Y"
        };

        List<ReportResultTotalAndAverage> results = new();
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            $@"
            SELECT strftime('{periodPattern}', start) AS period, 
            SUM(duration) AS total, 
            ROUND(AVG(duration),0) AS average 
            FROM coding_sessions 
            GROUP BY period;
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var period = reader.GetString(0);
                var total = TimeSpan.FromSeconds(reader.GetInt32(1));
                var average = TimeSpan.FromSeconds(reader.GetInt32(2));
                results.Add(new ReportResultTotalAndAverage(period, total, average));
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return results;
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