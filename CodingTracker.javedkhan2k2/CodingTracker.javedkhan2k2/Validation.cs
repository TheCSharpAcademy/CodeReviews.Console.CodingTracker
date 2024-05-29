
using System.Globalization;

namespace CodingTracker;

internal static class Validation
{
    internal static bool IsValidDateTimeInput(string? date)
    {
        if(date == null || !DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            return false;
        }
        return true;
    }
}