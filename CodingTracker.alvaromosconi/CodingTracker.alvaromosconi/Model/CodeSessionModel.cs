namespace CodingTracker.alvaromosconi.Model;

public class CodeSessionModel
{
    public int Id { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public String Duration
    {
        get
        {
            return CalculateDuration();
        }
    }

    private string CalculateDuration()
    {
        TimeSpan duration = EndDateTime - StartDateTime;
        return duration.ToString(@"hh\:mm\:ss");
    }
}
