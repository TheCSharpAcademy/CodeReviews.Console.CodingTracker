namespace CodingTracker.Database.Models;

public class Goal
{
  public int Goal_Id { get; set; }
  public string Start_Date { get; set; }
  public string End_Date { get; set; }
  public int Target_Duration { get; set; }
  public int Is_Completed { get; set; }

  public Goal() { }

  public Goal(int goalId, string startDate, string endDate, int targetDuration, int isCompleted)
  {
    Goal_Id = goalId;
    Start_Date = startDate;
    End_Date = endDate;
    Target_Duration = targetDuration;
    Is_Completed = isCompleted;
  }
}