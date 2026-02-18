namespace CodingTracker.Models;

internal class CodingSession
{
  public int Id { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
  public string Description { get; set; }

  public TimeSpan Duration => EndTime - StartTime;

  public CodingSession() { }

  public CodingSession(int id, DateTime startTime, DateTime endTime, string description)
  {
    Id = id;
    StartTime = startTime;
    EndTime = endTime;
    Description = description;
  }
}
