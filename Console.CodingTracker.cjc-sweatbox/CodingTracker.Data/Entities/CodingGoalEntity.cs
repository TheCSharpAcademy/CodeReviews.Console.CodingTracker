namespace CodingTracker.Data.Entities;

/// <summary>
/// Represents a row in the CodingGoal table.
/// </summary>
public class CodingGoalEntity
{
    #region Constructors

    public CodingGoalEntity()
    {
        // Required for Dapper.
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public double WeeklyDurationInHours { get; init; }

    #endregion
}
