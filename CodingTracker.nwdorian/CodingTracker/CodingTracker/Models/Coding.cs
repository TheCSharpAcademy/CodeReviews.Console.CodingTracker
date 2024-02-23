namespace CodingTracker.Models;

internal class Coding
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration
    {
        get
        {
            return CalculateDuration();
        }
    }

    private TimeSpan CalculateDuration()
    {
        return EndTime - StartTime;
    }
}
