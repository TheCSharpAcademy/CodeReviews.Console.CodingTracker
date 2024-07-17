using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker;
internal class CodingSession
{
    public DateTime start;
    public DateTime end;
    public TimeSpan duration;

    public static List<CodingSession> sessions = new List<CodingSession>();

    public CodingSession(DateTime start, DateTime end)
    {
        this.start = start;
        this.end = end;
        this.duration = end - start;
        sessions.Add(this);
    } // end of CodingSession Construction

    
} // end of CodingSession Class
