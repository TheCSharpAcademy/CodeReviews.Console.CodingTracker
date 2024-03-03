using System.Globalization;

namespace codingTracker.Fennikko.Models
{
    public class CodingSession
    {
        public int Id { get; set; }

        public string? StartTime { get; set; }

        public string? EndTime { get; set;}

        public string Duration => GetDuration(StartTime!,EndTime!);

        public double AverageMinutes => GetDurationMinutes(StartTime!, EndTime!);

        public string GetDuration(string startTime, string endTime)
        {
            var startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
            var endDateTime = DateTime.ParseExact(endTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));

            var differenceOfDates = endDateTime.Subtract(startDateTime);
            string stringDifference;

            switch (differenceOfDates.TotalMinutes)
            {
                case <= 1:
                    stringDifference = Convert.ToString($"{differenceOfDates.TotalSeconds:##.00} seconds");
                    break;
                case >=1 and <= 59:
                    stringDifference = Convert.ToString($"{differenceOfDates.TotalMinutes:##.00} minutes");
                    break;
                case >= 60 and <=1440 :
                    stringDifference = Convert.ToString($"{differenceOfDates.TotalHours:##.00} hours");
                    break;
                default:
                    stringDifference = Convert.ToString($"{differenceOfDates.TotalDays:##.00} days");
                    break;
            }
            return stringDifference;
        }

        public double GetDurationMinutes(string startTime, string endTime)
        {
            var startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
            var endDateTime = DateTime.ParseExact(endTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));

            var differenceOfDates = endDateTime.Subtract(startDateTime).TotalMinutes;

            return differenceOfDates;
        }
    }
}
