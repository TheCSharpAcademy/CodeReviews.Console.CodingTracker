using System.Diagnostics;

class SessionStopWatch
{
    public DateTime StartDate;
    public DateTime EndDate;
    private readonly Stopwatch sessionTimer;

    public TimeSpan SessionTimer {get => sessionTimer.Elapsed;}

    public bool IsRunning {get => sessionTimer.IsRunning;}

    public SessionStopWatch()
    {
        StartDate = DateTime.Now;
        sessionTimer = new Stopwatch();
        sessionTimer.Start();
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