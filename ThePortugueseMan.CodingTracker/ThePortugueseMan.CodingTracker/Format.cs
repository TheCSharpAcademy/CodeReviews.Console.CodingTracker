using System.Globalization;

namespace ThePortugueseMan.CodingTracker;

internal class Format
{
    string? dateMainDbFormat, dateGoalsDbFormat, dateDisplayFormat, timeSpanFormat;
    AppSettings appSettings = new();

    public Format()
    {
        this.dateMainDbFormat = appSettings.GetDateTimeMainDbFormat();
        this.dateGoalsDbFormat = appSettings.GetDateTimeGoalsDbFormat();
        this.dateDisplayFormat = appSettings.GetDateTimeDisplayFormat();
        this.timeSpanFormat = appSettings.GetTimeSpanFormatOfDB();
    }

    public string DateToMainDbString(DateTime dateTimeToFormat) {return dateTimeToFormat.ToString(dateMainDbFormat);}

    public string DateToGoalsDbString(DateTime dateTimeToFormat) { return dateTimeToFormat.ToString(dateGoalsDbFormat); }

    public string DateToDisplayString(DateTime dateTimeToFormat) { return dateTimeToFormat.ToString(dateDisplayFormat); }
    
    public string DateToTimeString(DateTime dateTimeToFormat) { return dateTimeToFormat.ToString("HH:mm"); }

    public string TimeSpanToString(TimeSpan timeToFormat)
    { return $"{Math.Truncate(timeToFormat.TotalHours).ToString("00")}:{timeToFormat.ToString("mm")}"; }

    public DateTime StringToDate(string inputString)
    {
        DateTime returnDate;

        if (DateTime.TryParseExact(inputString, dateMainDbFormat,
            new CultureInfo("en-US"), DateTimeStyles.None, out returnDate))
        {
            return returnDate;
        }
        else if (DateTime.TryParseExact(inputString, dateGoalsDbFormat,
        new CultureInfo("en-US"), DateTimeStyles.None, out returnDate))
        {
            return returnDate;
        }
        else return DateTime.MinValue;
    }

    public TimeSpan StringToTimeSpan(string inputString)
    {
        string[] durationSplit = inputString.Split(":");
        if (Int32.TryParse(durationSplit[0], out int fullHours))
        {
            if (Int32.TryParse(durationSplit[1], out int fullMinutes))
            {
                return TimeSpan.Zero.Add(TimeSpan.FromHours(fullHours)).Add(TimeSpan.FromMinutes(fullMinutes));
            }
        }
        return TimeSpan.MinValue ;
    }
}
