using CodingTracker.Data.Managers;
using CodingTracker.Models;

namespace CodingTracker.Controllers;

/// <summary>
/// Controller class for the Coding Goal object and the database entity.
/// </summary>
public class CodingGoalController
{
    #region Fields

    private readonly SqliteDataManager _dataManager;

    #endregion
    #region Constructors

    public CodingGoalController(string databaseConnectionString)
    {
        _dataManager = new SqliteDataManager(databaseConnectionString);
    }

    #endregion
    #region Methods

    public CodingGoal GetCodingGoal()
    {
        var entity = _dataManager.GetCodingGoal();
        return new CodingGoal(entity);
    }

    public void SetCodingGoal(double weeklyDurationInHours)
    {
        _dataManager.SetCodingGoal(weeklyDurationInHours);
    }

    #endregion
}
