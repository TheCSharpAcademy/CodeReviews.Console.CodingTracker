namespace CodingTracker.Repositories;

public class SessionLogRepository
{
    private readonly Database db = new();

    public IEnumerable<SessionLog> GetLogsBySessionId(int sessionId)
    {
        using var connection = db.GetConnection();
        var query = connection
            .Query("SELECT * FROM Logs WHERE SessionId = @SessionId", new { SessionId = sessionId });
        return query.Select(l => new SessionLog
        {
            Id = (int)l.Id,
            SessionId = (int)l.SessionId,
            StartTime = TimeOnly.Parse(l.StartTime),
            EndTime = TimeOnly.Parse(l.EndTime),
            Duration = TimeSpan.FromTicks(l.Duration)
        });
    }

    public void DeleteSessionLog(SessionLog log)
    {
        using var connection = db.GetConnection();
        connection.Execute("DELETE FROM Logs WHERE Id = @Id", log);
    }

    public void UpdateSessionLog(SessionLog sessionLog)
    {
        if (sessionLog.EndTime < sessionLog.StartTime)
            throw new ArgumentException("Log not created due to: End Time must be greater than Start Time.",
                nameof(sessionLog));

        using var connection = db.GetConnection();
        connection.Execute(
            "UPDATE Logs SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id", new
            {
                sessionLog.Id,
                StartTime = sessionLog.StartTime.ToString("t"),
                EndTime = sessionLog.EndTime.ToString("t"),
                Duration = sessionLog.CalculateDuration
            });
    }

    public List<SessionLog> GetAllSessionLogs()
    {
        try
        {
            using var connection = db.GetConnection();
            var logRecords = connection.Query("SELECT * FROM Logs");

            var logs = logRecords.Select(l => new SessionLog
            {
                Id = (int)l.Id,
                SessionId = (int)l.SessionId,
                StartTime = TimeOnly.Parse(l.StartTime),
                EndTime = TimeOnly.Parse(l.EndTime),
                Duration = TimeSpan.FromTicks(l.Duration)
            });

            return logs.ToList();
        }
        catch (Exception e)
        {
            throw new CodingTrackerException("An unexpected error occurred while getting the session logs.", e);
        }
    }

    public void CreateLog(SessionLog sessionLog)
    {
        if (sessionLog.EndTime < sessionLog.StartTime)
            throw new ArgumentException("Log not created due to: End Time must be greater than Start Time.",
                nameof(sessionLog));

        try
        {
            using var connection = db.GetConnection();

            var query =
                "INSERT INTO Logs (SessionId, StartTime, EndTime, Duration) VALUES (@SessionId, @StartTime, @EndTime, @Duration)";
            connection.Execute(query,
                new
                {
                    sessionLog.SessionId,
                    StartTime = sessionLog.StartTime.ToString("t"),
                    EndTime = sessionLog.EndTime.ToString("t"),
                    Duration = sessionLog.CalculateDuration
                });
        }
        catch (Exception e)
        {
            throw new CodingTrackerException("An unexpected error occurred while creating the log.", e);
        }
    }
}