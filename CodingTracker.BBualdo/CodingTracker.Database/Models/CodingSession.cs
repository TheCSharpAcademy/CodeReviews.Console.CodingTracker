using CodingTracker.Database.Helpers;

namespace CodingTracker.Database.Models;

public class CodingSession
{
  public int Session_Id { get; set; }
  public string Start_Date { get; set; }
  public string End_Date { get; set; }
  public int Duration { get; set; }

  public CodingSession() { }

  public CodingSession(int sessionId, string startDate, string endDate)
  {
    Session_Id = sessionId;
    Start_Date = startDate;
    End_Date = endDate;
    Duration = DateTimeHelper.CalculateDuration(Start_Date, End_Date);
  }
}