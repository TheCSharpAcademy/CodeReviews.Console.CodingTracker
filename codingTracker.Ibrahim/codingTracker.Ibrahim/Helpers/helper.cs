using System.Globalization;
using codingTracker.Ibrahim.data;

namespace codingTracker.Ibrahim.Helpers
{
    public class helper
    {

        public static string CalculateDuration(string startTime, string endTime)
        {
            TimeSpan duration = TimeSpan.Zero;
            bool Valid = false;
            do
            {
                DateTime et = DateTime.Parse(endTime);
                DateTime st = DateTime.Parse(startTime);

                if (et > st)
                {
                    Valid = true;
                    duration = et - st;
                }
                else
                {
                    Console.WriteLine("start date can't be after end date please enter dates again in the format 'MM-dd-yyyy HH:mm:ss': \n");
                    Console.Write($"start date = ");
                    startTime = Console.ReadLine();
                    Console.Write($"end date = ");
                    endTime = Console.ReadLine();
                }
            }
            while (!Valid);
            return $"{duration.Hours} Hours {duration.Minutes} minutes";
        }
        public static string ValidateDateTimeFormat(string dateTime)
        {
            string format = "MM-dd-yyyy h:mm tt";
            DateTime result;

            while (!DateTime.TryParseExact(dateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                Console.WriteLine("Invalid format. Please enter the date and time in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM): ");
                Console.Write("Enter Here: ");
                dateTime = Console.ReadLine();
            }

            return dateTime;
        }
        public static (string startTime, string endTime) ValidateDateTime(int Id, string? StartTime, string? EndTime)
        {
            var validDateTime = ("", "");
            if (StartTime != null && EndTime == null)
            {
                StartTime = ValidateDateTimeFormat(StartTime);
                bool Valid = false;
                do
                {
                    DateTime et = DateTime.Parse(DatabaseManager.GetOne(Id).EndTime);
                    DateTime st = DateTime.Parse(StartTime);

                    if (et > st)
                    {
                        Valid = true;
                    }
                    else
                    {
                        Console.WriteLine("start date can't be after end date please enter Start Date again in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM):\" \n");
                        Console.Write($"start date = ");
                        StartTime = Console.ReadLine();
                    }
                }
                while (!Valid);
                validDateTime = (StartTime, null);
            }

            else if (StartTime == null && EndTime != null)
            {
                EndTime = ValidateDateTimeFormat(EndTime);
                if (StartTime == null && EndTime != null)
                {
                    EndTime = ValidateDateTimeFormat(EndTime);
                    bool Valid = false;
                    do
                    {
                        DateTime st = DateTime.Parse(DatabaseManager.GetOne(Id).StartTime);
                        DateTime et = DateTime.Parse(EndTime);

                        if (et > st)
                        {
                            Valid = true;
                        }
                        else
                        {
                            Console.WriteLine("start date can't be after end date please enter End Date again in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM):\" \n");
                            Console.Write($"start date = ");
                            EndTime = Console.ReadLine();
                        }
                    }
                    while (!Valid);
                    validDateTime = (null, EndTime);
                }
            }

            else if (StartTime != null && EndTime != null)
            {
                StartTime = ValidateDateTimeFormat(StartTime);
                EndTime = ValidateDateTimeFormat(EndTime);
                bool Valid = false;
                do
                {
                    DateTime et = DateTime.Parse(EndTime);
                    DateTime st = DateTime.Parse(StartTime);

                    if (et > st)
                    {
                        Valid = true;
                    }
                    else
                    {
                        Console.WriteLine("start date can't be after end date please enter Start Date again in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM):\" \n");
                        Console.Write($"start date = ");
                        StartTime = Console.ReadLine();
                        Console.WriteLine("start date can't be after end date please enter End Date again in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM):\" \n");
                        Console.Write($"end date = ");
                        EndTime = Console.ReadLine();
                    }
                }
                while (!Valid);
                validDateTime = (StartTime, EndTime);

            }
            return validDateTime;
        }
        public static (string startTime, string EndTime) ValidateDateTimes(string StartTime, string EndTime)
        {
            StartTime = ValidateDateTimeFormat(StartTime);
            EndTime = ValidateDateTimeFormat(EndTime);
            bool Valid = false;
            do
            {
                DateTime et = DateTime.Parse(EndTime);
                DateTime st = DateTime.Parse(StartTime);

                if (et > st)
                {
                    Valid = true;
                }
                else
                {
                    Console.WriteLine("start date can't be after end date please enter dates again in the format MM-DD-YYYY HH:MM AM/PM (e.g., 04-26-2001 1:00 PM):\" \n");
                    Console.Write($"start date = ");
                    StartTime = Console.ReadLine();
                    Console.WriteLine($"end date = ");
                    EndTime = Console.ReadLine();
                }
            }
            while (!Valid);

            return (StartTime, EndTime);
        }
        public static int SessionExists(int Id, bool checkingForExists)
        {
            int sessionExists = -1;

            bool databaseCheck = DatabaseManager.SessionExists(Id);

            if (checkingForExists)
            {
                do
                {
                    databaseCheck = DatabaseManager.SessionExists(Id);

                    if (databaseCheck)
                    {
                        sessionExists = Id;
                    }
                    else
                    {
                        Console.Write("that session does not exist please enter a valid session number: ");
                        Id = validateInt(Console.ReadLine());
                    }
                }
                while (!databaseCheck);
            }

            return sessionExists;
        }
        public static int validateInt(string input)
        {
            int result;
            while (!int.TryParse(input, out result))
            {
                Console.Write("Invalid input. Please enter a valid session number:");
                input = Console.ReadLine();
            }
            return result;
        }

        internal static string ValidateUserChoice(string menuOptionSelected)
        {

            string retVal = "";
            int number;
            bool result;

            do
            {
                result = int.TryParse(menuOptionSelected, out number);
                if (result)
                {
                    if (number <= 6 && number >= 0)
                    {
                        retVal = menuOptionSelected;
                    }
                    else
                    {
                        result = false;
                        Console.Write($"please enter a number between 0 and 6");
                        menuOptionSelected = Console.ReadLine();
                    }
                }
                else //not a number
                {
                    result = false;
                    Console.Write($"please enter a number between 0 and 6");
                    menuOptionSelected = Console.ReadLine();
                }
            }
            while (!result);

            return retVal;
        }
    }
}
