using System.Globalization;

internal class DateTimeHelper
{
    internal static DateTime ConcatenateDateAndTime(DateTime date, string timeInput)
    {
        DateTime.TryParseExact(timeInput, Config.TimeFormat, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime time);
        return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
    }
}
