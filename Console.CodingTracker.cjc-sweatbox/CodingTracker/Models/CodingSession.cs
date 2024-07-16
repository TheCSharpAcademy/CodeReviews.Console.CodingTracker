using CodingTracker.Data.Entities;

namespace CodingTracker.Models;

/// <summary>
/// Coding session data transformation object.
/// </summary>
public class CodingSession
{
    #region Constructors

    public CodingSession(CodingSessionEntity entity)
    {
        Id = entity.Id;
        StartTime = entity.StartTime;
        EndTime = entity.EndTime;
        Duration = entity.Duration;
    }

    public CodingSession(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public DateTime StartTime { get; init; }

    public DateTime EndTime { get; init; }

    public double Duration { get; init; }

    #endregion
}
