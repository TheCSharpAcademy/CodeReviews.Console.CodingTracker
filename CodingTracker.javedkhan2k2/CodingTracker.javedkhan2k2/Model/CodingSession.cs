using System.ComponentModel.DataAnnotations.Schema;

namespace CodingTracker.Models;

internal class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public long DurationInSeconds { get; set; }

    [NotMapped] 
    public TimeSpan Duration
    {
        get => TimeSpan.FromSeconds(DurationInSeconds);
        set => DurationInSeconds = (long)value.TotalSeconds;
    }
}

internal class CodingSessionDto
{
    public string StartTime { get; set; } = default!;
    public string EndTime { get; set; } = default!;
    public long Duration { get; set; }
}

