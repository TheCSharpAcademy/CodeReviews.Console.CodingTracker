using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePortugueseMan.CodingTracker;

public class CodingSession
{
    public int Id;
    public DateTime StartDateTime;
    public DateTime EndDateTime;
    public TimeSpan Duration;

    public CodingSession (int id, DateTime startTime, DateTime endTime, TimeSpan duration)
    {
        Id = id;
        StartDateTime = startTime;
        EndDateTime = endTime;
        Duration = duration;
    }
    public CodingSession(DateTime startTime, DateTime endTime, TimeSpan duration)
    {
        StartDateTime = startTime;
        EndDateTime = endTime;
        Duration = duration;
    }

    public CodingSession() {}
}
