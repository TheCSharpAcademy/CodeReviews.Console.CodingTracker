using System.Globalization;

namespace ConConfig
{
    public class InputUtils
    {
        public static DateTime GetValidTime(string dateType = "start")
        {
            DateTime date;
            Console.WriteLine($"Please type your {dateType} date(dd-MM-yyyy):");
            string? dateStr = Console.ReadLine();
            while (!DateTime.TryParseExact(dateStr, "dd-MM-yyyy", new CultureInfo("en-US"),
                                   DateTimeStyles.None, out date))
            {
                Console.WriteLine("Sorry, your date is invalid. Please type a valid date(dd-MM-yyyy):");
                dateStr = Console.ReadLine();
            }
            return date;
        }
    }
}