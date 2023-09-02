using System;
using System.Globalization;

namespace CodingTrackerConsoleUI
{

    public static class Validation
    {
        public static string CheckValidTime(string timeInput) 
        {          
            while (!DateTime.TryParseExact(timeInput, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nInvalid time. (Format: HH:mm). Type 0 to return to main manu or try again:");
                timeInput = Console.ReadLine();
            }          
            return timeInput;
        }

        public static string CheckValidNumber(string numberInput) 
        {
            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\nInvalid number. Try again.");
                numberInput = Console.ReadLine();
            }
            return numberInput;
        }
    }
}