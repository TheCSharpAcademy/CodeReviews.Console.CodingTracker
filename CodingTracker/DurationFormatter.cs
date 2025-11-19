namespace CodingTracker;

public class DurationFormatter
{
    public static string DurationToHourString(double duration)
    {
        TimeSpan t = TimeSpan.FromSeconds(duration);
        if ((t.Days * 24) + t.Hours > 0)
        {
            return $"{(t.Days * 24) + t.Hours}h {t.Minutes}m {t.Seconds}s";
        }

        if (t.Minutes > 0)
        {
            return $"{t.Minutes}m {t.Seconds}s";
        }

        return $"{t.Seconds}s";
    }
}
