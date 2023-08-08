namespace CodingTracker.MartinL_no.Models;

internal class CodingSession
{
    internal DateTime StartTime { get; set; }
    internal DateTime EndTime { get; set; }
    internal TimeSpan Duration => EndTime - StartTime;

	internal CodingSession(DateTime startTime, DateTime endTime)
	{
        StartTime = startTime;
        EndTime = endTime;
    }
}
