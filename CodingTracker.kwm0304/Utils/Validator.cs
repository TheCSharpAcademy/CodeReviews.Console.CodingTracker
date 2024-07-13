using CodingTracker.kwm0304.Models;

namespace CodingTracker.kwm0304.Utils;

public class Validator
{
    internal static void ConvertDateTimeToString(DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    internal static void ConvertTimeToString(TimeSpan sessionLength)
    {
        throw new NotImplementedException();
    }
    internal static DateTime ConvertTextToDateTime(string dateStr)
    {
        return DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", null);
    }
    internal static TimeSpan ConvertTextToTimeSpan(string timeStr)
    {
        return TimeSpan.ParseExact(timeStr, @"hh\:mm\:ss", null);
    }

    internal static bool IsIdValid(int id)
    {
        throw new NotImplementedException();
    }


    internal static bool IsListValid(List<CodingSession>? allSessions)
    {
        throw new NotImplementedException();
    }

    internal static bool IsValidTime(DateTime startTime)
    {
        throw new NotImplementedException();
    }
}
