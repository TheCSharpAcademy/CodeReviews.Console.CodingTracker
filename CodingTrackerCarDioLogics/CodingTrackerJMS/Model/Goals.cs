namespace CodingTrackerJMS.Model;

public class Goals
{
    public string Goal { get; set; }
    public int TimeToGoal { get; set; }

    public Goals(string goal, int timeToGoal)
    {
        Goal = goal;
        TimeToGoal = timeToGoal;
    }
}
