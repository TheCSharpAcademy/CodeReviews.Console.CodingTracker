using System;
using System.Linq;

namespace CodingTracker.Wolffles;

public class Timer
{
    DateTime startDate;
    DateTime endDate;


    public ISession TimedSession()
    {
        StartTimer();
        return new CodingSession(0,startDate,endDate);
    }
    private void StartTimer()
    {
        startDate = DateTime.Now;
        Console.WriteLine("Timer started. Press any key to stop.");
        Console.ReadKey();
        EndTimer();
    }
    private void EndTimer()
    {
        endDate = DateTime.Now;
        Console.WriteLine("Timer ended.");
    }


}
