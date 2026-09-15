public class SessionService
{
    public DateTime startTime;
    public void StartSession()
    {
        startTime = DateTime.Now;
    }

    public Session EndSession()
    {
        DateTime endTime = DateTime.Now;
        return new Session
        {
            StartTime = startTime,
            EndTime = endTime,
            Duration = endTime - startTime
        };
    }

    public TimeSpan GetElapsedTime()
    {
        return DateTime.Now - startTime;
    }
}