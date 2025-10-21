using System.ComponentModel.DataAnnotations.Schema;

namespace CodingTracker;

public class CodingSession
{
    public CodingSession() { }
    public CodingSession(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
        UpdateDuration();
    }

    public int Id { get; set; }

    [Column("start_time")]
    public DateTime Start { get; set; }

    [Column("end_time")]
    public DateTime End { get; set; }
    public double Duration { get; set; }

    public void UpdateDuration()
    {
        Duration = (End - Start).TotalSeconds;
    }
}
