using CodingTracker.Controllers;

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

    public string Duration => CalculateDuration();

    private string CalculateDuration()
    {
        DateTime start = HelpersValidation.ConvertToTime(StartTime);
        DateTime end = HelpersValidation.ConvertToTime(EndTime);
        TimeSpan duration = end - start;

        return $"{duration.Hours.ToString().PadLeft(2, '0')}:{duration.Minutes.ToString().PadLeft(2, '0')}";
    }
}