namespace CodingTracker;

public class CodingSession(long id, string task, string startTime, string endTime, int duration)
{
    public long Id { get; set; } = id;
    public string Task { get; set; } = task;
    public string StartTime { get; set; } = startTime;
    public string EndTime { get; set; } = endTime;
    public int Duration { get; set; } = duration;
}
