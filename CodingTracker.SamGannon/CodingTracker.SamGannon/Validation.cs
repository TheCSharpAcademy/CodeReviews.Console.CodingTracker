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
        GetUserInput getUserInput = new();

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

        public string GetDuration()
        {
            Console.WriteLine("PLease enter the start time of your session in the following format: (hh:mm). Type 0 to return to main menu\n\n");
            string startTime = Console.ReadLine();
            if (startTime == "0") getUserInput.MainMenu();
            ValidateTimeFormat(startTime);

            Console.WriteLine("PLease enter the ending time of your session in the following format: (hh:mm). Type 0 to return to main menu\n\n");
            string endTime = Console.ReadLine();
            if (endTime == "0") getUserInput.MainMenu();
            ValidateTimeFormat(endTime);

            string duration = CalculateDuration(startTime, endTime);

            return duration;
        }

        public string GetDateInput()
        {
            Console.WriteLine("Please enter the date in the following format: (dd-mm-yy). Type 0 to return to the main menu.");

            string userDateInput = Console.ReadLine();

            if (userDateInput == "0") getUserInput.MainMenu();

            while (!DateTime.TryParseExact(userDateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
                userDateInput = Console.ReadLine();
            }

            return userDateInput;
        }

        private void ValidateTimeFormat(string time)
        {
            while (!TimeSpan.TryParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture, out _))
            {
                Console.WriteLine("\n\nDuration invalid. Please insert the duration: (Format: hh:mm) or type 0 to return to the main menu\n\n");
                time = Console.ReadLine();
                if (time == "0") getUserInput.MainMenu();
            }
        }

        private string CalculateDuration(string startTime, string endTime)
        {
            TimeSpan start = TimeSpan.ParseExact(startTime, "hh\\:mm", CultureInfo.InvariantCulture);
            TimeSpan end = TimeSpan.ParseExact(endTime, "hh\\:mm", CultureInfo.InvariantCulture);

            TimeSpan duration = end - start;

            return duration.ToString("hh:mm");
        }

        internal int ValidateIdInput(string? commandInput)
        {
            while (!int.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\n You have to type a valid Id (or 0 to return to the Main Menu). \n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) getUserInput.MainMenu();

            return id;
        }
    }
}
