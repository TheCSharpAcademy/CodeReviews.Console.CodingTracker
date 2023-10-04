using System.Globalization;

namespace CodingTracker.Mo3ses.UserMenu
{
    public static class Validation
    {
        public static bool CheckInt(string input, out int inputParsed)
        {
            if (Int32.TryParse(input, out inputParsed))
            {
                return true;
            }
            else return false;
        }
        public static bool CheckDateFormat(string date, out DateTime dateParsed)
        {

            if (DateTime.TryParseExact(date, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateParsed))
            {
                return true;
            }
            else return false;

        }
        public static bool CheckIsValidDates(string startTime, string endTime)
        {
            DateTime startTimeParsed;
            DateTime endTimeParsed;

            if (CheckDateFormat(startTime, out startTimeParsed) && CheckDateFormat(endTime, out endTimeParsed))
            {
                if (endTimeParsed > startTimeParsed)
                {
                    return true;
                }
                else
                {
                    System.Console.WriteLine("End date must be greater than start date");
                    return false;
                }
            }
            else
            {
                System.Console.WriteLine("Invalid Dates, try again");
                return false;
            }
        }
    }
}