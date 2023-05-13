namespace CodingTrackerLibrary;
public class CodingSessionModel
{
    public int SessionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public State Status
    {
        get
        {
            if (EndTime == DateTime.MinValue)
            {
                return State.Open;
            }
            else
            {
                return State.Closed;
            }
        }
    }

    public TimeSpan Duration 
    {
        get
        {
            if (Status == State.Open)
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
