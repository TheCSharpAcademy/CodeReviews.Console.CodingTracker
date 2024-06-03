
namespace CodingTracker;

internal static class Helpers
{
    internal static long CalculateDuration(string startDate, string endDate)  
    {
        if (!Validation.IsValidDateTimeInputs(startDate, endDate))
        {
            return 0;
        }
        DateTime startDateTime = DateTime.Parse(startDate);
        DateTime endDateTime = DateTime.Parse(endDate);
        return (long)endDateTime.Subtract(startDateTime).Duration().TotalSeconds;
    }
}