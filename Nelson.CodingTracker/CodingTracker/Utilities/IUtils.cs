namespace CodingTracker.Utilities
{
    public interface IUtils
    {
        string GetDateInput(string message);
        DateTime ValidatedStartTime();
        DateTime ValidatedEndTime();
        bool ValidatedEndTime(DateTime startTime, DateTime endTime);
        List<DateTime> ValidatedTimes();
        string GetSessionDuration(DateTime startTime, DateTime endTime);
        int ConvertToInt(string input);
    }
}