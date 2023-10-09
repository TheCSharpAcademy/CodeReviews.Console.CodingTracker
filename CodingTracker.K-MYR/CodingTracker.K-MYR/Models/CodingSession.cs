namespace CodingTracker.K_MYR.Models;


internal class CodingSession
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public TimeSpan Duration { get; set; }
}

