using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codingTracker.Ibrahim.UI
{
    public class SessionTracker
    {
        public DateTime StartTime {get; set;}
        public DateTime EndTime { get; set;}

        public Stopwatch stopwatch = new Stopwatch();
        public SessionTracker() {
            
        }

        public void StartTimer()
        {
            StartTime = DateTime.Now;
            stopwatch.Start();
        }

        public void EndTimer()
        {
            stopwatch.Stop();
            EndTime = DateTime.Now;
        }

        public (string startTime, string endTime) GetTime()
        {
            string startTime = StartTime.ToString("MM-dd-yyyy h:mm tt");
            string endTime = EndTime.ToString("MM-dd-yyyy h:mm tt");

            return (startTime, endTime);
        }

    }
}
