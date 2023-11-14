using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.SamGannon
{
    
    public class Validation
    {
        public string CalculateSleepType(string duration)
        {
            TimeSpan sleepDuration = TimeSpan.ParseExact(duration, "h\\:mm", CultureInfo.InvariantCulture);

            if (sleepDuration.TotalHours > 4)
            {
                return "long";
            }
            else
            {
                return "Short";
            }
        }

        public string GetStartTime()
        {
            Console.WriteLine("Please enter the start time of your session in the following format: (HH:mm).");
            Console.WriteLine("Use 24-hour format (e.g., 14:30).");

            string startTime = Console.ReadLine();
            ValidateTimeFormat(startTime);

            return startTime;
        }

        public string GetEndTime()
        {
            Console.WriteLine("Please enter the ending time of your session in the following format: (HH:mm).");
            Console.WriteLine("Use 24-hour format (e.g., 17:45).");

            string endTime = Console.ReadLine();
            ValidateTimeFormat(endTime);

            return endTime;
        }

        private void ValidateTimeFormat(string time)
        {
            while (!IsValid24HourFormat(time))
            {
                Console.WriteLine("\n\nDuration invalid. Please insert the duration in 24-hour format HH:mm: (e.g., 17:45)\n\n");
                time = Console.ReadLine();
            }
        }

        private bool IsValid24HourFormat(string time)
        {
            bool isValid = TimeSpan.TryParseExact(time, "HH\\:mm", CultureInfo.InvariantCulture, out _);
            if (!isValid)
            {
                Console.WriteLine($"Invalid time format: {time}");
            }
            return isValid;
        }


        public string GetDateInput()
        {
            Console.WriteLine("Please enter the date in the following format: (dd-mm-yy).");

            string userDateInput = Console.ReadLine();

            while (!DateTime.TryParseExact(userDateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
                userDateInput = Console.ReadLine();
            }

            return userDateInput;
        }

        public string CalculateDuration(string startTime, string endTime, Action crudProcess)
        {
            TimeSpan start = TimeSpan.ParseExact(startTime, "HH\\:mm", CultureInfo.InvariantCulture);
            TimeSpan end = TimeSpan.ParseExact(endTime, "HH\\:mm", CultureInfo.InvariantCulture);

            TimeSpan duration = end - start;

            return $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}";
        }

        internal int ValidateIdInput(string? commandInput)
        {
            while (!int.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\n You have to type a valid Id\n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            return id;
        }
    }
}
