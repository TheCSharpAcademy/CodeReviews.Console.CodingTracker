using System.ComponentModel.DataAnnotations.Schema;

namespace CodingTracker;

public class CodingSession
{
    public long? Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public long? DurationTicks { get; set; }
    // We use Duration to use TimeSpan in the code even if SQLite doesn't support it
    [NotMapped]
    public TimeSpan Duration
    {
        get => TimeSpan.FromTicks(DurationTicks ?? 0);
        set => DurationTicks = value.Ticks;
    }

    public CodingSession() { }
    
    public CodingSession(DateTime startTime, DateTime? endTime, TimeSpan duration, long? id = null)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }

    public override string ToString()
    {
        return $@"Coding Session {{start: {StartTime}, end: {EndTime}, duration: {Duration} }}";
    }
}