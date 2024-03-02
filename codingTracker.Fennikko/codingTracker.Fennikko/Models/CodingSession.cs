using System.Globalization;

namespace codingTracker.Fennikko.Models
{
    public class CodingSession
    {
        public int ID { get; set; }

        public string? StartTime { get; set; }

        public string? EndTime { get; set;}

        public string Duration {get{return GetDuration(StartTime,EndTime);}}



        public static string GetDuration(string startTime, string endTime)
        {
            var startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
            var endDateTime = DateTime.ParseExact(endTime, "dd-MM-yy HH:mm", new CultureInfo("en-US"));

            var differenceOfDates = endDateTime.Subtract(startDateTime);
            string stringDifference;

            switch (differenceOfDates.TotalHours)
            {
                case < 1:
                    stringDifference = Convert.ToString($"{differenceOfDates.TotalMinutes:##.00} minutes");
                    break;
                case >= 1 and <= 24:
                    stringDifference = Convert.ToString($"{differenceOfDates.TotalHours:##.00} hours");
                    break;
                default:
                    stringDifference = Convert.ToString($"{differenceOfDates.TotalDays:##.00} days");
                    break;
            }
            return stringDifference;
        }
    }
}
