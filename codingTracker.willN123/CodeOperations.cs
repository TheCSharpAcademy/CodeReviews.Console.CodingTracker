namespace CodingTracker
{
    internal class CodeOperations
    {
        public string GetTotalTime(string start, string end)
        {
            return CalculateTotal(
               ConvertToTime(ConvertHours(start),ConvertMinutes(start)),
               ConvertToTime(ConvertHours(end), ConvertMinutes(end)))
               .ToString(@"hh\:mm");
        }

        public bool CompareTime(string start, string end)
        {
            var startTime = ConvertToTime(ConvertHours(start), ConvertMinutes(start));
            var endTime = ConvertToTime(ConvertHours(end), ConvertMinutes(end));
            if(startTime < endTime)
            {
                return true;
            }

            Console.WriteLine("Invalid Input. Start time must be before end time.");
            return false;
        }

        public static TimeSpan CalculateTotal(TimeOnly start, TimeOnly end)
        {
            return end - start;
        }
        
        public static TimeOnly ConvertToTime(int hours, int minutes)
        {
            return new TimeOnly(hours, minutes);
        }

        public static int ConvertHours(string time)
        {
            string hours = time[..2];
            int convertedHours = Int32.Parse(hours);
            return convertedHours;
        }

        public static int ConvertMinutes(string time)
        {
            string minutes = time[3..];
            int convertedMinutes = Int32.Parse(minutes);
            return convertedMinutes;
        }
    }
}
