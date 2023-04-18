using static System.Runtime.InteropServices.JavaScript.JSType;

namespace coding_tracker
{
    public class CodingSession
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        public CodingSession(string startTime, string endTime)
        {
            StartTime = DateTime.Parse(startTime, System.Globalization.CultureInfo.InvariantCulture);
            EndTime = DateTime.Parse(endTime, System.Globalization.CultureInfo.InvariantCulture);
            Duration = EndTime - StartTime;
        }
    }
}