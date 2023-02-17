namespace CodingTracker.kraven88.Models;

public static class CodingSessionExtensions
{
    public static TimeSpan AverageDuration(this IEnumerable<CodingSession> list)
    {
        var duration = TimeSpan.FromSeconds(list.Select(x => x.Duration.TotalSeconds).Average());
        return duration;
    }

    public static TimeSpan TotalDuration(this IEnumerable<CodingSession> list)
    {
        var duration = TimeSpan.FromSeconds(list.Select(x => x.Duration.TotalSeconds).Sum());
        return duration;
    }
}
