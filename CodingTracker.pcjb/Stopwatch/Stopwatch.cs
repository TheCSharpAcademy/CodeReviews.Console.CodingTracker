namespace CodingTracker;

class Stopwatch
{
    private DateTime? start;
    private bool isRunning;

    public void Start()
    {
        start = CurrentDateTimeFullMinute();
        isRunning = true;
    }

    public CodingSession? Stop()
    {
        isRunning = false;
        if (!start.HasValue)
        {
            return null;
        }
   
        var end = CurrentDateTimeFullMinute();
        return new CodingSession(start.Value, end);
    }

    public DateTime? GetStart()
    {
        return start;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    private DateTime CurrentDateTimeFullMinute()
    {
        var now = DateTime.Now;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
    }
}