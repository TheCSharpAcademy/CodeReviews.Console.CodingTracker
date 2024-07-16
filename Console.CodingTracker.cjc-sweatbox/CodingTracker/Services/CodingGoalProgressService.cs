using CodingTracker.Controllers;

namespace CodingTracker.Services;

/// <summary>
/// Service to handle Coding Goal Process calculations.
/// </summary>
public class CodingGoalProgressService
{
    #region Fields

    private readonly CodingSessionController _codingSessionController;
    private readonly CodingGoalController _codingGoalController;
    
    #endregion
    #region Constructors

    public CodingGoalProgressService(CodingSessionController codingSessionController, CodingGoalController codingGoalController)
    {
        _codingSessionController = codingSessionController;
        _codingGoalController = codingGoalController;
    }

    #endregion
    #region Methods

    public string GetCodingGoalProgress()
    {
        var codingGoal = _codingGoalController.GetCodingGoal();
        if (codingGoal == null || codingGoal.WeeklyDurationInHours == 0)
        {
            return "please set a coding goal for motivation.";
        }

        // Lets say a week is Monday - Sunday.
        var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

        // Get the total duration spent this week.
        var codingSessions = _codingSessionController.GetCodingSessions().Where(w => w.StartTime >= startOfWeek && w.EndTime <= endOfWeek);
        double totalDuration = codingSessions.Sum(x => x.Duration);

        // Get difference
        double difference = codingGoal.WeeklyDurationInHours - totalDuration;

        // Goal Reached?
        if (difference <= 0)
        {
            return $"you have reached your weekly coding goal. Well done!";
        }
        else if (difference < 0)
        {
            return $"you are {Math.Abs(difference):F2} hours over your weekly coding goal. Well done!";
        }

        // Get required.
        var daysRemaining = (endOfWeek.Date - DateTime.Today).Days;
        var averagePerDay = difference / daysRemaining;

        return $"you require {difference:F2} more hours to reach your weekly coding goal. Which is {averagePerDay:F2} hours per day. You can do it!";
    }

    #endregion
}
