using System;

namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Duration { get; set; }
        
        public static int CalculateDuration(string StartTime, string EndTime)
        {
            string[] startList = StartTime.Split(':');
            int startH = int.Parse(startList[0]);
            int startM = int.Parse(startList[1]);

            string[] endList = EndTime.Split(':');
            int endH = int.Parse(endList[0]);
            int endM = int.Parse(endList[1]);

            return ((endH - startH) * 60) + (endM - startM);
        }
    }
}
