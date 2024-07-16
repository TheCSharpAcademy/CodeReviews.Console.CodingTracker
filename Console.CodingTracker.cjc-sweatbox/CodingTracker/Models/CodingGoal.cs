using CodingTracker.Data.Entities;

namespace CodingTracker.Models;

/// <summary>
/// Coding session data transformation object.
/// </summary>
public class CodingGoal
{
    #region Constructors

    public CodingGoal(CodingGoalEntity entity)
    {
        Id = entity.Id;
        WeeklyDurationInHours = entity.WeeklyDurationInHours;
    }

    public CodingGoal(double weeklyDurationInHours)
    {
        WeeklyDurationInHours = weeklyDurationInHours;
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public double WeeklyDurationInHours { get; init; }

    #endregion
}
