using System.Globalization;

namespace CodingTrackerConsole;

public class Validation
{
    public bool IsValidDate(string date)
    {
        return DateOnly.TryParseExact(date, "MM-dd-yyyy", out _);
    }

    public bool IsValidTime(string time)
    {
        return TimeOnly.TryParseExact(time, "HH:mm", null, DateTimeStyles.None, out _);
    }

    public bool IsValidTimeOrder(string startTime, string endTime)
    {
        return DateTime.ParseExact(startTime, "HH:mm", null, DateTimeStyles.None) <
               DateTime.ParseExact(endTime, "HH:mm", null, DateTimeStyles.None);
    }

    public bool IsValidDay(string day)
    {
        int.TryParse(day, out int parsedDay);
        return day.Length == 2 && (parsedDay > 0 && parsedDay <= 31);
    }

    public bool IsValidMonth(string month)
    {
        int.TryParse(month, out int parsedMonth);
        return month.Length == 2 && (parsedMonth > 0 && parsedMonth <= 12);
    }

    public bool IsValidYear(string year)
    {
        int.TryParse(year, out int parsedYear);
        return year.Length == 4 && (parsedYear > 1000 && parsedYear <= 9999);
    }
}
