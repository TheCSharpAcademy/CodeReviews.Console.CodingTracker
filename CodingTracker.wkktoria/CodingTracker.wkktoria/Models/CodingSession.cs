namespace CodingTracker.wkktoria.Models;

public class CodingSession
{
    public int Id { get; set; }
    public string StartTime { get; set; } = Helpers.PareDateToDbFormat(DateTime.UtcNow);
    public string EndTime { get; set; } = Helpers.PareDateToDbFormat(DateTime.UtcNow);
    public double Duration { get; set; }
}