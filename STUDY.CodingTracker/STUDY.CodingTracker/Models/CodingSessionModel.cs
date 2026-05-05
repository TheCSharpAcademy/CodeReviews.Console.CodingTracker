namespace STUDY.CodingTracker.Models;

internal class CodingSessionModel
{
    public Int64 id { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public TimeSpan duration { get; }

    public CodingSessionModel (string start, string end, Int64 newId, string newDuration)
    {
        id = newId;

        startTime = ConvertStringToDateTime(start);

        endTime = ConvertStringToDateTime(end);

        duration = ConvertStringToTimeSpan(newDuration);
    }

    public CodingSessionModel (DateTime start, DateTime end, TimeSpan newDuration)
    {
        startTime = start;
        endTime = end;
        duration = newDuration;
    }

    public CodingSessionModel (DateTime start, DateTime end)
    {
        startTime = start;
        endTime = end;
        duration = endTime.Subtract(startTime);
    }

    private DateTime ConvertStringToDateTime(string date)
    {
        DateTime convertedDate;

        DateTime.TryParse(date, out convertedDate);
        return convertedDate;
    }

    private TimeSpan ConvertStringToTimeSpan(string time)
    {
        TimeSpan convertedTime;

        TimeSpan.TryParse(time, out convertedTime);

        return convertedTime;
    }

}
