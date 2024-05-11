namespace CodingTracker.kalsson;

public static class Validation
{
    public static bool ValidateDateTimeRange(DateTime startTime, DateTime endTime)
    {
        return endTime > startTime;
    }
}