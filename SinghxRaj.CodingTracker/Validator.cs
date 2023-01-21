namespace SinghxRaj.CodingTracker
{
    internal class Validator
    {
        internal static bool ValidateOption(int option)
        {
            return option >= 0 && option <= 2;
        }

        internal static bool ValidateSessionDateTimes(DateTime start, DateTime end)
        {
            return start <= end;
        }

        internal static bool ValidateDate(string? dateInput)
        {
            return DateOnly.TryParseExact(dateInput, TimeFormat.SessionDateOnlyFormat, out var date);
        }

        internal static bool ValidateTime(string? dateInput)
        {
            return TimeOnly.TryParseExact(dateInput, TimeFormat.SessionTimeOnlyFormat, out var time);
        }
    }
}