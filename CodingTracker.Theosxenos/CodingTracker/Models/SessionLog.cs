namespace CodingTracker.Models;

public class SessionLog
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public long CalculateDuration => (EndTime - StartTime).Ticks;
}