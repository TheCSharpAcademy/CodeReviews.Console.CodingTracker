namespace CodingTracker;

public class CodingSession
{
    public long? Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public CodingSession() { }
    
    public CodingSession(DateTime startTime, DateTime? endTime, TimeSpan duration, long? id = null)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }
}