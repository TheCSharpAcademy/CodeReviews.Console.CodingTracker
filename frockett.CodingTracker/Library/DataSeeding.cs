
using frockett.CodingTracker.Library;
using System.Dynamic;

namespace Library;

internal class DataSeeding
{
    static Random random = new Random();
    static DateTime start = new DateTime(2023, 1, 1);
    static int dateRange = (DateTime.Today - start).Days;

    public CodingSession GetRandomSession()
    {
        DateTime start = GetRandomStart();
        TimeSpan duration = GetRandomDuration();
        DateTime end = start + duration;

        return new CodingSession
        {
            StartTime = start,
            EndTime = end,
            Duration = duration,
        };
    }

    private TimeSpan GetRandomDuration()
    {
        int hours = random.Next(1, 12);
        return TimeSpan.FromHours(hours);
    }

    private DateTime GetRandomStart()
    {
        int randomDays = random.Next(dateRange);
        return start + TimeSpan.FromDays(randomDays);
    }

}