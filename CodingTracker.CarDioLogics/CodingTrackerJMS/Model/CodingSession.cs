namespace CodingTrackerJMS.Model;

public class CodingSession
{
    public int Id { get; set; }  
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalTime { get; set; }
    public string Goal { get; set; }
    public int TimeToGoal { get; set; }

    //below is a constructor used to initialize the properties of the class above
    public CodingSession(int id, DateTime startDate, DateTime endDate, int totalTime, string goal, int timeToGoal)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        TotalTime = totalTime;
        Goal = goal;
        TimeToGoal = timeToGoal;
    }
}
