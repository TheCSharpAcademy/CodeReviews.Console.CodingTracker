using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi.ConsoleUI
{
    internal static class UserInput
    {
        internal static char SelectMenuOption(string OPTIONS)
        {
            Console.Write("Select: ");
            var selectedOption = Console.ReadKey().KeyChar;

            if (!OPTIONS.Contains(selectedOption))
            { 
                Console.WriteLine("\nInvalid option. Please select a valid option.\n");

                selectedOption = SelectMenuOption(OPTIONS);
            }

            return selectedOption;
        }

        internal static (DateTime, DateTime) GetDateRangeFromUser()
        {
            string dateFormat = "dd-MM-yy HH:mm";

            Console.WriteLine("Search by date: ");
            Console.WriteLine($"\n Please enter the dates in the format {dateFormat}");
            Console.Write("\nFROM: ");
            string from = Console.ReadLine()!.Trim().ValidateDate(dateFormat);
            Console.Write("TILL: ");
            string till = Console.ReadLine()!.Trim().ValidateDate(dateFormat);
            
            return (DateTime.Parse(from), DateTime.Parse(till));
        }

        private static string ValidateDate(this string input, string dateFormat)
        {
            DateTime output = DateTime.MinValue;
            bool isInvalid = true;

            while (isInvalid)
            {
                if (!DateTime.TryParseExact(input, 
                                            dateFormat, 
                                            CultureInfo.InvariantCulture, 
                                            DateTimeStyles.None, 
                                            out output))
                {
                    Console.Write("\nInvalid input. Please try again: ");
                    input = Console.ReadLine()!.Trim();
                }
                else
                    isInvalid = false;
            }

            return output.ToString(dateFormat);
        }
    }
}
