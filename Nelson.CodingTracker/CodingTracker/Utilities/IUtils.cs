namespace CodingTracker.Utilities
{
    public interface IUtils
    {
        string GetDateInput(string message);
        DateTime ValidatedStartTime();
        DateTime ValidatedEndTime();
        List<DateTime> ValidatedTimes();
        TimeSpan GetSessionDuration(DateTime startTime, DateTime endTime);
    }
}