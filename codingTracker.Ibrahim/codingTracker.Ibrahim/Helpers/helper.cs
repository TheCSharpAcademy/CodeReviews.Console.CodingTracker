using System.Globalization;
using codingTracker.Ibrahim.data;

namespace codingTracker.Ibrahim.Helpers
{
    public class helper
    {

        public static string CalculateDuration(string startTime, string endTime)
        {
            string duration="";
            bool Valid = false;
            do
            {
                DateTime et = DateTime.Parse(endTime);
                DateTime st = DateTime.Parse(startTime);

                if (et > st)
                {
                    Valid = true;
                    duration = Convert.ToString(et - st);
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
            return duration;
        }
        public static string ValidateDateTimeFormat(string dateTime)
        {
            string format = "MM-dd-yyyy HH:mm:ss";
            DateTime result;

            while (!DateTime.TryParseExact(dateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                Console.WriteLine("Invalid format. Please enter a date and time in the format 'MM-dd-yyyy HH:mm:ss'");
                Console.Write("Enter Here: ");
                dateTime = Console.ReadLine();
            }

            return dateTime;
        }
        public static string ValidateDateTime(int Id, string? StartTime,string? EndTime)
        {
            string validDateTime = "";
            if(StartTime!= null && EndTime ==null)
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
                        Console.WriteLine("start date can't be after end date please enter Start Date again in the format 'MM-dd-yyyy HH:mm:ss': \n");
                        Console.Write($"start date = ");
                        StartTime = Console.ReadLine();
                    }
                }
                while (!Valid);
                validDateTime = StartTime;
            }

            else if (StartTime==null && EndTime!=null)
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
                            Console.WriteLine("start date can't be after end date please enter End Date again in the format 'MM-dd-yyyy HH:mm:ss': \n");
                            Console.Write($"start date = ");
                            EndTime = Console.ReadLine();
                        }
                    }
                    while (!Valid);
                    validDateTime = EndTime;
                }
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
                    Console.WriteLine("start date can't be after end date please enter dates again in the format 'MM-dd-yyyy HH:mm:ss': \n");
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

            if(checkingForExists) // we want it to be there, if not prompt user for an existing date and set dateExists to that
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
                    if (number <= 5 && number >= 0)
                    {
                        retVal = menuOptionSelected;
                    }
                    else
                    {
                        result = false;
                        Console.Write($"please enter a number between 0 and 5");
                        menuOptionSelected = Console.ReadLine();
                    }
                }
                else //not a number
                {
                    result = false;
                    Console.Write($"please enter a number between 0 and 5");
                    menuOptionSelected = Console.ReadLine();
                }
            }
            while (!result);

            return retVal;
        }
    }
}
