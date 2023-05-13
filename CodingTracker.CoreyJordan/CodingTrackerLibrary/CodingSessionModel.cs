namespace CodingTrackerLibrary;
public class CodingSessionModel
{
    public int SessionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string Status
    {
        get
        {
            if (EndTime == DateTime.MinValue)
            {
                return "Open";
            }
            else
            {
                return "Closed";
            }
        }
    }

    public TimeSpan Duration 
    {
        get
        {
            if (Status == "Open")
            {
                return TimeSpan.Zero;
            }
            else
            {
                return EndTime - StartTime;
            }
        } 
    }
}
