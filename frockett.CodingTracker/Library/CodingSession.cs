
namespace frockett.CodingTracker.Library;

public class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    //These parts of the class are for the custom reports only
    public string? Month {  get; set; }
    public double TotalTime { get; set; }
    public double AverageTime { get; set; }
}
