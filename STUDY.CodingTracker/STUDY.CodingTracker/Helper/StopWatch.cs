using Spectre.Console;
using System.Timers;

namespace STUDY.CodingTracker.Helper;

internal class StopWatch
{
    private static System.Timers.Timer _timer;
    private TimeSpan duration = new TimeSpan(0, 0, 0);

    public StopWatch()
    {
        duration = new TimeSpan(0, 0, 0);
    }

    public TimeSpan LaunchTimer()
    {
        SetTimer();

        Console.ReadLine();

        _timer.Stop();
        _timer.Dispose();

        return duration;
    }

    private void SetTimer()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        duration = duration.Add(new TimeSpan(0, 0, 1));
        Console.Clear();
        AnsiConsole.MarkupLine("Press Any Key to stop the timer.");
        AnsiConsole.MarkupLine($"Duration: {duration}");
    }
}
