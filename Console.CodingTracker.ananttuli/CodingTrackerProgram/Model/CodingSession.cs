namespace CodingTrackerProgram.Model;

public class CodingSession
{
    public Int64 Id;
    public DateTime StartDateTime;
    public DateTime EndDateTime;
    public TimeSpan Duration
    {
        get
        {
            return CalculateDuration(StartDateTime, EndDateTime);
        }
    }

    public static TimeSpan CalculateDuration(DateTime startDateTime, DateTime endDateTime)
    {
        return endDateTime.Subtract(startDateTime);
    }

    public CodingSession(Int64 id, string startDateTime, string endDateTime)
    {
        Id = id;
        StartDateTime = DateTime.Parse(startDateTime);
        EndDateTime = DateTime.Parse(endDateTime);
    }
}