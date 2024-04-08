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
        DateTime start = ConvertStartTime();
        DateTime end = ConvertEndTime();
        TimeSpan duration = end - start;

        return $"{duration.Hours.ToString()};{duration.Minutes.ToString()}";
    }

    private DateTime ConvertStartTime()
    {
        return DateTime.ParseExact(StartTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
    
    private DateTime ConvertEndTime()
    {
        return DateTime.ParseExact(EndTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
}