using System.Globalization;

namespace CodingTracker
{
    class Validation
    {
        public static void ValidateDate(string date, Func<string> Function, string format)
        {
            if (!DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid format");
                Function();
            }
        }

        public static void ValidateNumber(string numberInput, Func<int> retryFunction)
        {
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                retryFunction();
            }
        }

        public static void ValidateDuration(string startTime, string endTime, string format)
        {
            if (DateTime.ParseExact(endTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None) < DateTime.ParseExact(startTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None))
            {
                Console.WriteLine("Invalid format. The time in which your session ended, was before your session even started.");
                CodingController.Insert();
            }
        }
    }
}