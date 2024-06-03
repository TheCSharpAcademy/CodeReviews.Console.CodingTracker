
using System.Globalization;

namespace CodingTracker;

internal static class Validation
{
    internal static bool IsEmptyOrZero(string startDate, string endDate)
    {
        if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate)
            || startDate == "0" || endDate == "0")
        {
            return true;
        }
        return false;
    }

    internal static bool IsValidDateTimeInput(string? date)
    {
        if (date != null && date == "0")
        {
            return true;
        }
        if (date == null || !DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            return false;
        }
        return true;
    }

    internal static bool IsValidDateInput(string? date, string? format)
    {
        if (date != null && date == "0")
        {
            return true;
        }
        if (date == null || !DateTime.TryParseExact(date, format, new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            return false;
        }
        return true;
    }

    internal static bool IsValidDateTime(string startDate, string endDate)
    {
        DateTime startDateTime = DateTime.Parse(startDate);
        DateTime endDateTime = DateTime.Parse(endDate);
        return DateTime.Compare(endDateTime, startDateTime) > 0 ? true : false;
    }

    internal static bool IsValidDateTimeInputs(string startDate, string endDate)
    {
        if(IsEmptyOrZero(startDate, endDate))
        {
            return false;
        }
        return IsValidDateTime(startDate, endDate);
    }

    internal static bool IsValidYearInputs(string startDate, string endDate)
    {
        if(IsEmptyOrZero(startDate, endDate))
        {
            return false;
        }
        return Int32.Parse(endDate) >= Int32.Parse(startDate) ? true : false;
    }

    internal static bool IsValidIntegerInput(string? userInput)
    {
        if (userInput != null && userInput == "0")
        {
            return true;
        }
        if (userInput == null || !Int32.TryParse(userInput, out _))
        {
            return false;
        }
        return true;
        
    }
}