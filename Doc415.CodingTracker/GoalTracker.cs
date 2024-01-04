namespace Doc415.CodingTracker;

public class GoalTracker
{
    DataAccess dataAccess = new DataAccess();
    public void AddGoal(Goal _goal)
    {
        dataAccess.AddGoal(_goal);
    }


}
