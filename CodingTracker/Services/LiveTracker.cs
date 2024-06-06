using System;
using System.Diagnostics;

public class LiveTracker
{
    public readonly Stopwatch Stopwatch = new Stopwatch();

    public void Start()
    {
        Stopwatch.Start();
    }

    public CodingSession Save()
    {
        Stopwatch.Stop();

        CodingSession codingSession = new CodingSession();
        codingSession.Duration = Stopwatch.Elapsed;
        codingSession.StartTime = DateTime.Now - codingSession.Duration;
        codingSession.EndTime = DateTime.Now;

        return codingSession;
    }

    public void Reset()
    {
        Stopwatch.Reset();
    }

    public TimeSpan GetTime()
    {
        return new TimeSpan(Stopwatch.Elapsed.Hours, Stopwatch.Elapsed.Minutes, Stopwatch.Elapsed.Seconds);
    }
}