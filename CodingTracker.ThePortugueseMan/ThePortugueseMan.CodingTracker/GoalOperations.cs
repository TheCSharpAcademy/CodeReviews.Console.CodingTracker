namespace ThePortugueseMan.CodingTracker;

public class GoalOperations
{
    DbCommands dbCmd = new();
    ListOperations listOp = new();
    
    public void UpdateGoal()
    {
        Goal goal = dbCmd.GetGoalInTable();
        if (goal is null) return;
        
        List<CodingSession> allSessions = dbCmd.GetAllLogsInTable();
        List<CodingSession> sessionsInGoalRange = listOp.GetLogsBetweenDates(allSessions, goal.StartDate, goal.EndDate);

        Goal updatedGoal = new();
        updatedGoal.HoursSpent = listOp.TotalTimeBetweenDates(sessionsInGoalRange, 
            goal.StartDate, goal.EndDate.AddTicks(TimeSpan.TicksPerDay - 1));

        updatedGoal.StartDate = goal.StartDate;
        updatedGoal.EndDate = goal.EndDate;
        updatedGoal.TargetHours = goal.TargetHours;

        dbCmd.Update(goal.Id, updatedGoal);
    }
}
