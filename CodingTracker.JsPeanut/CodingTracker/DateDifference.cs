namespace CodingTracker
{
    public class DateDifference
    {
        public int Years;
        public int Months;
        public int Days;
        public int Hours;
        public int Minutes;
        public long Seconds;
        public static TimeSpan Duration;
        public DateDifference(DateTime initialDate, DateTime finalDate)
        {
            var difference = finalDate - initialDate;
            Days = (int)difference.TotalDays;
            Minutes = (int)difference.TotalMinutes;
            Seconds = (int)difference.TotalSeconds;
            Hours = (int)difference.TotalHours;
            Years = Days / 365;
            Months = Days / 30;
            Duration = new TimeSpan(0, 0, 0, (int)Seconds);
        }
        public string PrintCodingSessionTime()
        {
            string message;

            if (Duration.Days > 1)
            {
                message = $"You have been coding for: {Duration.Days} days, {Duration.Hours} hours, {Duration.Minutes} minutes and {Duration.Seconds} seconds.";
            }
            else
            {
                message = $"You have been coding for: {Duration.Hours} hours, {Duration.Minutes} minutes and {Duration.Seconds} seconds.";
            }

            return message;
        }
    }
}
