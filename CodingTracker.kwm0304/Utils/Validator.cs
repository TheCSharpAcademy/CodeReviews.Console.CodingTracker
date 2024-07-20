using CodingTracker.kwm0304.Enums;

namespace CodingTracker.kwm0304.Utils;

public class Validator
{
    internal static string ConvertDateTimeToString(DateTime time)
    {
        return time.ToString("yyyy-MM-dd HH:mm:ss");
    }

    internal static int ConvertTimeToInt(TimeSpan time)
    {
        return (int)time.TotalSeconds;
    }
    internal static DateTime ConvertTextToDateTime(string dateStr)
    {
        return DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", null);
    }

    internal static int ToDays(DateRange range)
    {
        return range switch
        {
            DateRange.Week => 7,
            DateRange.Month => 30,
            DateRange.Year => 365,
            _ => throw new ArgumentOutOfRangeException(nameof(range), $"Unexpected DateRange value: {range}")
        };
    }

    internal static DateRange ToDateRange(int range)
    {
        return range switch
        {
            7 => DateRange.Week,
            30 => DateRange.Month,
            365 => DateRange.Year,
            _ => throw new ArgumentOutOfRangeException(nameof(range), $"Unexpected DateRange value: {range}")
        };
    }
}
