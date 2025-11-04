namespace CodingTracker;

public class FilteredCodingSession
{
    public required string FilterId { get; set; }
    public double Duration { get; set; }
    private TimeSpan Elapsed => TimeSpan.FromSeconds(Duration);

    public string DurationToStringComplete()
    {
        return Elapsed.ToString(@"hh\:mm\:ss\.ff");
    }
}
