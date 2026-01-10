using System.Globalization;

internal class Validation
{
    public static bool CheckValidDate(string userDate)
    {
        var validDate = DateTime.MinValue;
        var isValid = DateTime.TryParseExact(
            userDate,
            Globals.DATE_FORMAT,
            Globals.CULTURE_INFO,
            DateTimeStyles.None,
            out validDate);

        return isValid;
    }

    public static bool CheckValidTime(string userTime)
    {
        var validTime = DateTime.MinValue;
        var isValid = DateTime.TryParseExact(
            userTime,
            Globals.TIME_FORMAT,
            Globals.CULTURE_INFO,
            DateTimeStyles.None,
            out validTime);

        return isValid;
    }

    public static bool CheckValidEndTime(string start, string end)
    {
        var startTime = DateTime.Parse(start);
        var endTime = DateTime.Parse(end);

        if (startTime <= endTime)
        {
            return true;
        }
        return false;
    }
}
