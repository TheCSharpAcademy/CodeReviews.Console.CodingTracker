namespace CodingTracker;

class CodingSession
{
    public long Id { get; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; }
   
    public CodingSession(long id, DateTime start, DateTime end, TimeSpan duration)
    {
        Id = id;
        StartTime = start;
        EndTime = end;
        Duration = duration;
    }

    public CodingSession(DateTime start, DateTime end)
    {
        StartTime = start;
        EndTime = end;
        Duration = CalculateDuration();
    }

    private TimeSpan CalculateDuration()
    {
        return EndTime - StartTime;
    }
}