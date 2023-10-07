namespace CodingTracker.wkktoria.Models;

public class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double Duration { get; set; }
}