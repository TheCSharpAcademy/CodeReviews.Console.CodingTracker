namespace CodingTracker.MartinL_no.Models;

internal class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
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
