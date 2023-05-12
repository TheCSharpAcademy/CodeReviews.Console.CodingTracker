namespace CodingTrackerLibrary;
public class CodingSessionModel
{
    public int SessionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration 
    {
        get
        {
            return EndTime - StartTime;
        } 
    }
}
