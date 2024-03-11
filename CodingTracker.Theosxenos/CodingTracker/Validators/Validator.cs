namespace CodingTracker.Validators;

public static class Validator
{
    public static string DateFormat => "d-M-y";
    public static string TimeFormat => "HH:mm";

    public static bool ValidateStringAsDate(string date)
    {
        return DateOnly.TryParseExact(date, DateFormat, out _);
    }

    public static bool ValidateStringAsTime(string time)
    {
        return TimeOnly.TryParseExact(time, TimeFormat, out _);
    }
}