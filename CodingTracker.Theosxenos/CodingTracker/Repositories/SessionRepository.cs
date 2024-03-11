namespace CodingTracker.Repositories;

public class SessionRepository
{
    private readonly Database db = new();

    public void CreateSession(DateOnly day)
    {
        var dayString = DateToString(day);
        try
        {
            using var connection = db.GetConnection();

            var query = "INSERT INTO Sessions (Day) VALUES (@Day)";
            connection.Execute(query, new { Day = dayString });
        }
        catch (SqliteException e) when (e.SqliteErrorCode == 19)
        {
            throw new CodingTrackerException($"A session with the date {dayString} already exists.", e);
        }
        catch (Exception e)
        {
            throw new CodingTrackerException("An unexpected error occurred while creating the session.", e);
        }
    }

    public List<Session> GetAllSessions()
    {
        try
        {
            using var connection = db.GetConnection();
            // Query into a dynamic or an intermediate DTO that closely represents the raw data structure
            var sessionRecords = connection.Query("SELECT * FROM Sessions");

            // Map the dynamic or DTO results to your Session model, handling the conversion
            var sessions = sessionRecords.Select(record =>
                new Session
                {
                    Id = (int)record.Id,
                    Day = DateOnly.Parse(record.Day)
                }
            ).OrderBy(s => s.Day).ToList();

            return sessions;
        }
        catch (Exception e)
        {
            throw new CodingTrackerException("An unexpected error occurred while getting the sessions.", e);
        }
    }

    public void UpdateSession(Session updatedSession)
    {
        try
        {
            using var connection = db.GetConnection();
            var query = "UPDATE Sessions SET Day = @Day WHERE Id = @Id";
            connection.Execute(query, new { Day = DateToString(updatedSession.Day), updatedSession.Id });
        }
        catch (SqliteException e) when (e is { SqliteErrorCode: 19 })
        {
            throw new CodingTrackerException(
                $"A session with the date '{DateToString(updatedSession.Day)}' already exists.");
        }
        catch (Exception e)
        {
            throw new CodingTrackerException("An unexpected error occurred while updating the session.", e);
        }
    }

    public void DeleteSession(int sessionId)
    {
        using var connection = db.GetConnection();
        connection.Execute("DELETE FROM Sessions WHERE Id = @Id", new { Id = sessionId });
    }

    private string DateToString(DateOnly date)
    {
        return date.ToString("d-M-yy");
    }
}