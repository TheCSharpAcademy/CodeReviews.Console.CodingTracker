using CodingTracker.Services;

namespace CodingTracker.Models;

public class CodingGoalModel
{
    public int Id { get; set; }
    public string DateCreated { get; set; } = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
    public string? DateCompleted { get; set; }
    public string? TargetDuration { get; set; }
    public string CurrentProgress { get; set; } = TimeSpan.Zero.ToString(ConfigSettings.TimeFormatType);
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }

    public CodingGoalModel() { }

    public CodingGoalModel(TimeSpan targetDuration, string description) 
    {
        TargetDuration = targetDuration.ToString(ConfigSettings.TimeFormatType);
        Description = description;
    }

    public void UpdateProgress(TimeSpan sessionDuration)
    {
        var newDuration = TimeSpan.Parse(CurrentProgress) + sessionDuration;
        CurrentProgress = newDuration.ToString(ConfigSettings.TimeFormatType);

        if (newDuration > TimeSpan.Parse(TargetDuration!))
        {
            IsCompleted = true;
            DateCompleted = DateTime.UtcNow.ToString(ConfigSettings.DateFormatLong);
        }
        
    }

    public void GetProgressAsIntervals(out float currentProgress, out float targetHours)
    {
        var currentDuration = TimeSpan.Parse(CurrentProgress);
        var targetDuration = TimeSpan.Parse(TargetDuration!);
        currentProgress = (float)currentDuration.TotalHours;
        targetHours = (float)targetDuration.TotalHours;
    }
}

