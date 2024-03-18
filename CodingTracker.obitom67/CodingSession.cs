using System;
using System.Linq;


namespace CodingTracker.obitom67
{
    internal class CodingSession
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Duration { get; set; }
        public int Quantity { get; set; }


        public string GetDuration(DateTime startTime, DateTime endTime)
        {
            TimeSpan duration = endTime.Subtract(startTime);
            return duration.ToString();
        }

        public bool CheckDates(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
