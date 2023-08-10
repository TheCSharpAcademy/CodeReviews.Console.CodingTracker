namespace CodingTracker.MartinL_no.Models;

internal class CodingSession
{
    public readonly int Id;
    public DateTime StartTime;
    public DateTime EndTime;
    public TimeSpan Duration => EndTime - StartTime;

    public CodingSession(DateTime startTime, DateTime endTime)
	{
        StartTime = startTime;
        EndTime = endTime;
    }

    public CodingSession(int id, DateTime startTime, DateTime endTime) : this(startTime, endTime)
    {
        Id = id;
    }
}
