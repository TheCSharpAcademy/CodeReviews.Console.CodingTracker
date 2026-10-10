using System.Globalization;
using Spectre.Console;

namespace CodingTracker;

public class CodingSession
{
    public DateTime StartTime { get; set; }
    private DateTime? EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public CodingSession(DateTime startTime, DateTime? endTime, TimeSpan duration)
    {
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }
}