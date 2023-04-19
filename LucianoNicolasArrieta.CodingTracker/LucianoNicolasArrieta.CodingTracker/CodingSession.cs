using static System.Runtime.InteropServices.JavaScript.JSType;

namespace coding_tracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        public CodingSession(string startTime, string endTime)
        {
            StartTime = DateTime.ParseExact(startTime, "d/M/yyyy H:m", System.Globalization.CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(endTime, "d/M/yyyy H:m", System.Globalization.CultureInfo.InvariantCulture);
            Duration = EndTime - StartTime;
        }
    }
}