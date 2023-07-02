namespace CodingTracker.kmakai.Models;

public class CodeSession
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public TimeSpan Duration
    {
        get
        {
            return TimeSpan.Parse(EndTime) - TimeSpan.Parse(StartTime);
        }
    }
}
