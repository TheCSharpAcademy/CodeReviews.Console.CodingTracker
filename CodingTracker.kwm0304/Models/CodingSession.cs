namespace CodingTracker.kwm0304.Models;

public class CodingSession
{
  public int Id { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
  public CodingSession(DateTime startTime)
  {
    StartTime = startTime;
  }
  public TimeSpan SessionLength
  {
    get
    {
      return EndTime - StartTime;
    }
  }
}
