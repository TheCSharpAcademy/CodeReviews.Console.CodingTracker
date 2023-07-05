namespace CodingTracker.alvaromosconi.Model;

public class CodeSessionModel
{
    public int Id { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public TimeSpan Duration
    {
        get
        {
            return CalculateDuration();
        }
    }

    private TimeSpan CalculateDuration()
    {
        return EndDateTime - StartDateTime;
    }
}
