
namespace codingTracker.Ibrahim.Models
{
    public abstract class CodingSessionReport
    {                              
       
        public TimeSpan Duration { get; set; }
    }

    public class WeeklyCodingSessionReport : CodingSessionReport
    {
        public string Weeks { get; set; }
    }

    public class MonthlyCodingSessionReport : CodingSessionReport
    {
        public string Months { get; set; }
    }

    public class YearlyCodingSessionReport : CodingSessionReport
    {
        public string Years { get; set; }
    }
}
