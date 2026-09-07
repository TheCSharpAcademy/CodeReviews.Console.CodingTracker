using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.DzemalKurtic.Models
{
    internal class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        private TimeSpan Duration => EndTime - StartTime;
        public CodingSession(int id, DateTime startTime, DateTime endTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
