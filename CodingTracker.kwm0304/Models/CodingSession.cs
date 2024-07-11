namespace CodingTracker.kwm0304.Models;

public class CodingSession
{
  public int Id { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
  public TimeSpan SessionLength { get; set; }
  public CodingSession(DateTime startTime, DateTime endTime)
  {
    StartTime = startTime;
    EndTime = endTime;
    SessionLength = StartTime - EndTime;
  }
}
