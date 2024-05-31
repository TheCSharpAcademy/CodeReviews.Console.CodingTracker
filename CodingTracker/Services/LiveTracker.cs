using System.Diagnostics;

public class LiveTracker
{
    public readonly Stopwatch Stopwatch = new Stopwatch();

    public void Start()
    {
        Stopwatch.Start();
    }

    public void Stop()
    {
        Stopwatch.Stop();
    }

    public void Reset()
    {
        Stopwatch.Reset();
    }
}