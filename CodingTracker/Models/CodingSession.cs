using System.Globalization;
using System.Runtime.InteropServices.JavaScript;

namespace CodingTracker.Models;

public class CodingSession
{
    public CodingSession(string id, string start, string end)
    {
        Id = id;
        StartTime = start;
        EndTime = end;
    }

    public string Id { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    private string CalculateDuration()
    {
        DateTime start = ConvertToTime(StartTime);
        DateTime end = ConvertToTime(EndTime);
        TimeSpan duration = end - start;

        return $"{duration.Hours.ToString()};{duration.Minutes.ToString()}";
    }

    private DateTime ConvertToTime(string datetimeString)
    {
        return DateTime.ParseExact(datetimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
}