namespace SinghxRaj.CodingTracker;

public class Validator
{
    public static bool ValidateOption(int option)
    {
        return option >= 0 && option <= 4;
    }

    public static bool ValidateSessionDateTimes(DateTime start, DateTime end)
    {
        return start <= end;
    }

    public static bool ValidateDate(string? dateInput)
    {
        return DateOnly.TryParseExact(dateInput, TimeFormat.SessionDateOnlyFormat, out var date);
    }

    public static bool ValidateTime(string? dateInput)
    {
        return TimeOnly.TryParseExact(dateInput, TimeFormat.SessionTimeOnlyFormat, out var time);
    }

    internal static bool ValidateResponse(string? sessionStartResponse)
    {
        return sessionStartResponse != null && sessionStartResponse!.Length > 0;
    }
}