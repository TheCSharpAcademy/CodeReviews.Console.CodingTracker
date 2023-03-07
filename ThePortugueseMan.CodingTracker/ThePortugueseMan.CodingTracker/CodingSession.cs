namespace ThePortugueseMan.CodingTracker;

public class CodingSession
{
    public int Id;
    public DateTime StartDateTime;
    public DateTime EndDateTime;
    public TimeSpan Duration;

    public CodingSession(DateTime startTime, DateTime endTime, TimeSpan duration)
    {
        StartDateTime = startTime;
        EndDateTime = endTime;
        Duration = duration;
    }

    public CodingSession() {}
}
