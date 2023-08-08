namespace CodingTracker.MartinL_no.Models;

internal class CodingSession
{
    internal int Id { get; set; }
    internal DateTime StartTime { get; set; }
    internal DateTime EndTime { get; set; }
    internal TimeSpan Duration => EndTime - StartTime;

    internal CodingSession(DateTime startTime, DateTime endTime)
	{
        StartTime = startTime;
        EndTime = endTime;
    }

    internal CodingSession(int id, DateTime startTime, DateTime endTime) : this(startTime, endTime)
    {
        Id = id;
    }
}
