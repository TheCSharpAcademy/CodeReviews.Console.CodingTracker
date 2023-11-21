using System.Diagnostics.Contracts;

class CodingSession
{
    private string StartDate;
    private string StartTime;
    private string EndDate;
    private string EndTime;

    public CodingSession(string _startDate, string _startTime, string _endDate, string _endTime)
    {
        StartDate = _startDate;
        StartTime = _startTime;
        EndDate = _endDate;
        EndTime = _endTime;
    }

    public string SessionTime()
    {
        TimeOnly startDateTime;
        TimeOnly endDateTime;
        TimeSpan elapsedTime;
        string elpasedTimeString;

        TimeOnly.TryParseExact(StartTime,"HH:mm", out startDateTime);
        TimeOnly.TryParseExact(EndTime,"HH:mm",out endDateTime);
        
        elapsedTime = endDateTime - startDateTime;
        elpasedTimeString = elapsedTime.Hours.ToString()+":"+elapsedTime.Minutes.ToString();
        
        return elpasedTimeString;
    }
}