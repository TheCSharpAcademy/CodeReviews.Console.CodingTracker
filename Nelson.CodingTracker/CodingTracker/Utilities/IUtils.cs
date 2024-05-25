namespace CodingTracker.Utilities
{
    public interface IUtils
    {
        string GetDateInput(string message);
        DateTime ValidatedStartTime();
        DateTime ValidatedEndTime();
        List<DateTime> ValidatedTimes();
        string GetSessionDuration(DateTime startTime, DateTime endTime);
        int ConvertToInt(string input);
    }
}