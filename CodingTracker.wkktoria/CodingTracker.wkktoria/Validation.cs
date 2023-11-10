using System.Globalization;

namespace CodingTracker.wkktoria;

public static class Validation
{
    public static bool ValidateDateTime(string dateTime)
    {
        return DateTime.TryParseExact($"{dateTime}:00", "dd-MM-yy HH:mm:ss", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out _);
    }

    public static bool ValidateTwoDates(DateTime startDateTime, DateTime endDateTime)
    {
        return startDateTime < endDateTime;
    }
}