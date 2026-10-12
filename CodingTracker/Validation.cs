using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CodingTracker;

public static class Validation
{
    public const string DateFormat = "yyyy-MM-dd: HH-mm";
    public const string DateErrorMessage = "Invalid date format !";
    
    public static string DateToString(DateTime time)
    {
        return time.ToString(DateFormat);
    }

    public static bool DateTryParser(string input, out DateTime parsedDate)
    {
        return (DateTime.TryParseExact(input?.Trim(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out parsedDate));
    }
    
}