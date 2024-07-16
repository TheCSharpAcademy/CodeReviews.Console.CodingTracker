using CodingTracker.Data.Managers;
using CodingTracker.Models;

namespace CodingTracker.Controllers;

/// <summary>
/// Controller class for the Coding Session object and the database entity.
/// </summary>
public class CodingSessionController
{
    #region Fields

    private readonly SqliteDataManager _dataManager;

    #endregion
    #region Constructors

    public CodingSessionController(string databaseConnectionString)
    {
        _dataManager = new SqliteDataManager(databaseConnectionString);
    }

    #endregion
    #region Methods - Public

    public void AddCodingSession(DateTime startTime, DateTime endTime)
    {
        var duration = CalculateDuration(startTime, endTime);
        _dataManager.AddCodingSession(startTime, endTime, duration);
    }

    public void DeleteCodingSession(int codingSessionId)
    {
        _dataManager.DeleteCodingSession(codingSessionId);
    }

    public List<CodingSession> GetCodingSessions()
    {
        return _dataManager.GetCodingSessions().Select(x => new CodingSession(x)).ToList();
    }

    /// <summary>
    /// Seeds the database will 100 random CodingSession entries.
    /// </summary>
    public void SeedDatabase()
    {
        if (_dataManager.GetCodingSessions().Count == 0)
        {
            for (int i = 100; i > 0; i--)
            {
                var endDateTime = DateTime.Now.AddDays(-i).AddMinutes(-Random.Shared.Next(0, 120));
                var startDateTime = endDateTime.AddMinutes(-Random.Shared.Next(1, 120));
                var duration = (endDateTime - startDateTime).TotalHours;
                _dataManager.AddCodingSession(startDateTime, endDateTime, duration);
            }
        }
    }

    public void SetCodingSession(int codingSessionId, DateTime startTime, DateTime endTime)
    {
        var duration = CalculateDuration(startTime, endTime);
        _dataManager.SetCodingSession(codingSessionId, startTime, endTime, duration);
    }

    #endregion
    #region Methods - Private

    /// <summary>
    /// Requirement: Do not let user enter duration. Must be calculated in the CodingSessionController.
    /// </summary>
    /// <param name="startTime">The start time to calculate from.</param>
    /// <param name="endTime">The end time to calculate to.</param>
    /// <returns>The total hours between the start and the end times.</returns>
    private static double CalculateDuration(DateTime startTime, DateTime endTime)
    {
        return (endTime - startTime).TotalHours;
    }

    #endregion
}
