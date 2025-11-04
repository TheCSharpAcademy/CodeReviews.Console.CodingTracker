namespace CodingTracker;

public class Report
{
    public int CountToday { get; set; }
    public int TotalToday { get; set; }
    public int CountWeek { get; set; }
    public int TotalWeek { get; set; }
    public int CountYear { get; set; }
    public int TotalYear { get; set; }
    public int Count { get; set; }
    public int Total { get; set; }
    private TimeSpan ElapsedToday => TimeSpan.FromSeconds(TotalToday);
    private TimeSpan ElapsedWeek => TimeSpan.FromSeconds(TotalWeek);
    private TimeSpan ElapsedYear => TimeSpan.FromSeconds(TotalYear);
    private TimeSpan Elapsed => TimeSpan.FromSeconds(Total);

    public string TodayDurationToString()
    {
        return ElapsedToday.ToString(@"hh\:mm\:ss\.ff");
    }

    public string WeekDurationToString()
    {
        return ElapsedWeek.ToString(@"hh\:mm\:ss\.ff");
    }

    public string YearDurationToString()
    {
        return ElapsedYear.ToString(@"hh\:mm\:ss\.ff");
    }

    public string TotalDurationToString()
    {
        return Elapsed.ToString(@"hh\:mm\:ss\.ff");
    }
}
