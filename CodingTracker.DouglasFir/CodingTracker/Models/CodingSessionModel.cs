using CodingTracker.Services;
using System.Security.Cryptography;

namespace CodingTracker.Models;

public class CodingSessionModel
{
    public int? Id { get; set; }
    public string DateCreated { get; set; } = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
    public string DateUpdated { get; set; } = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
    public string SessionDate { get; set; }= DateTime.UtcNow.ToString(ConfigSettings.DateFormatShort);
    public string Duration { get; set; } = TimeSpan.Zero.ToString(ConfigSettings.TimeFormatType);
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }

    public enum SessionProperties
    {
        Id,
        DateCreated,
        DateUpdated,
        SessionDate,
        Duration,
        StartTime,
        EndTime
    }

    public enum EditableProperties
    {
        SessionDate,
        StartTime,
        EndTime
    }

    public CodingSessionModel() { }

    public CodingSessionModel(DateTime startTime, DateTime endTime)
    {
        SessionDate = startTime.ToString(ConfigSettings.DateFormatShort);
        StartTime = startTime.ToString(ConfigSettings.DateFormatLong);
        EndTime = endTime.ToString(ConfigSettings.DateFormatLong);
        Duration = (endTime - startTime).ToString(ConfigSettings.TimeFormatType);  // Calculate duration based on start and end times
        DateCreated = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
        DateUpdated = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
    }

    public void SetDuration(DateTime startTime, DateTime endTime)
    {
        TimeSpan elapsedTime = endTime - startTime;
        Duration = elapsedTime.ToString(ConfigSettings.TimeFormatType);
        DateUpdated = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
    }

    public void SetEndTime(DateTime date)
    {
        EndTime = date.ToString(ConfigSettings.DateFormatLong);
        SetDuration(DateTime.Parse(StartTime), date);
        DateUpdated = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
    }
}
