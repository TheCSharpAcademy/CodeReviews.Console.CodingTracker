using System.Diagnostics;

namespace CodingTracker;

class SessionStopWatch
{
    public DateTime StartDate;
    public DateTime EndDate;
    private readonly Stopwatch sessionTimer;
    public TimeSpan SessionTimer {get => sessionTimer.Elapsed;}
    public bool IsRunning {get => sessionTimer.IsRunning;}

    public SessionStopWatch()
    {
        sessionTimer = new Stopwatch();
    }

    public void StartSession()
    {
        if (!sessionTimer.IsRunning)
        {    
            StartDate = DateTime.Now;
            sessionTimer.Start();
        }
    }

    public void EndSession()
    {
        if (sessionTimer.IsRunning)
        {
            EndDate = DateTime.Now;
            sessionTimer.Stop();
        }
    }
}