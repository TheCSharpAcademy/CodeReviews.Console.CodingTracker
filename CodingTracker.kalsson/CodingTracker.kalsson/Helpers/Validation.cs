namespace CodingTracker.kalsson;

public static class Validation
{
    /// <summary>
    /// Validates if the end time is later than the start time.
    /// </summary>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>Returns true if the end time is later than the start time; otherwise, returns false.</returns>
    public static bool ValidateDateTimeRange(DateTime startTime, DateTime endTime)
    {
        return endTime > startTime;
    }
}