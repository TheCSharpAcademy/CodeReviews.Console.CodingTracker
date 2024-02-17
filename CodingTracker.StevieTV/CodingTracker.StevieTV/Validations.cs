using System.Globalization;

namespace CodingTracker.StevieTV;

public class Validations
{
    public static bool IsValidTimeFormat(string input)
    {
        return TimeSpan.TryParseExact(input, "h\\:mm", CultureInfo.InvariantCulture, out _);
    }

    public static bool IsValidDateFormat(string input)
    {
        return DateTime.TryParseExact(input, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _);
    }

    public static bool IsValidEndTime(string timeInput, string startTime)
    {
        TimeSpan.TryParseExact(startTime, "h\\:mm", CultureInfo.InvariantCulture, out TimeSpan start);
        TimeSpan.TryParseExact(timeInput, "h\\:mm", CultureInfo.InvariantCulture, out TimeSpan end);
        return ((end.Ticks - start.Ticks) >= 0);
    }
}