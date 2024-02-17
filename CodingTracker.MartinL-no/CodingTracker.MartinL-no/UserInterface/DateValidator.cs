using System.Globalization;

namespace CodingTracker.MartinL_no.UserInterface;

internal class DateValidator
{
    public readonly string Format;

    public DateValidator(string dateTimeFormat)
	{
		Format = dateTimeFormat;
    }

    public bool IsCorrectFormat(string dateTimeString, out DateTime dateValue)
    {
        CultureInfo enUS = new CultureInfo("en-US");

        if (DateTime.TryParseExact(dateTimeString, Format, enUS, DateTimeStyles.None, out dateValue))
        {
            return true;
        }
        return false;
    }

    public bool AreValidDates(string startTimeString, string endTimeString)
    {
        DateTime startTime;
        DateTime endTime;

        if (IsCorrectFormat(startTimeString, out startTime) && IsCorrectFormat(endTimeString, out endTime) && startTime < endTime)
        {
            return true;
        }

        else return false;
    }
}
