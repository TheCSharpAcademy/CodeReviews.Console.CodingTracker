using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ThePortugueseMan.CodingTracker;

internal class Format
{
    string? dateDbFormat, dateDisplayFormat, timeSpanFormat;
    AppSettings appSettings = new();

    public Format()
    {
        this.dateDbFormat = appSettings.GetDateTimeDbFormat();
        this.dateDisplayFormat = appSettings.GetDateTimeDisplayFormat();
        this.timeSpanFormat = appSettings.GetTimeSpanFormatOfDB();
    }

    public string DateToDateString(DateTime dateTimeToFormat)
    {
        return dateTimeToFormat.ToString(dateDisplayFormat);
    }

    public string DateToTimeString(DateTime dateTimeToFormat) 
    {
        return dateTimeToFormat.ToString("HH:mm");    
    }

    public string TimeSpanToStringFormat(TimeSpan timeToFormat) 
    {
        string formatted = 
            $"{Math.Truncate(timeToFormat.TotalHours).ToString("00")}:{timeToFormat.ToString("mm")}";
        return formatted ;
    }

    public DateTime StringToDate(string inputString)
    {
        DateTime returnDate = 
            DateTime.ParseExact(inputString, dateDbFormat, new CultureInfo("en-US"));
        return returnDate ;
    }

    public TimeSpan StringToTimeSpan(string inputString) 
    {
        string[] durationSplit = inputString.Split(":");
        Int32.TryParse(durationSplit[0], out int fullHours);
        Int32.TryParse(durationSplit[1], out int fullMinutes);

        TimeSpan returnTime = TimeSpan.Zero.Add(TimeSpan.FromHours(fullHours)).Add(TimeSpan.FromMinutes(fullMinutes));

        return returnTime ;

    }
}
