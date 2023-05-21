namespace CodingTracker.Chad1082.Models
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Duration
        {
            get
            {
                if (EndTime < StartTime)
                {
                    return "0";
                }

                TimeSpan duration = EndTime - StartTime;

                string format = @"dd\ \d\a\y\s\,\ hh\ \H\o\u\r\s\,\ mm\ \m\i\n\u\t\e\s";
                return duration.ToString(format);
            }
        }
    }
}
