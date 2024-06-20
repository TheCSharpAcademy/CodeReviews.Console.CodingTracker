using System;
using Spectre.Console;
namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public TimeSpan duration { get; set; }
        public CodingSession(DateTime start, DateTime end, TimeSpan dur)
        {
            startTime = start;
            endTime = end;
            duration = dur;
        }
    }
}
