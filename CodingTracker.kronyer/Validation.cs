using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class Validation
    {

        public static int ValidateInt(string input, string message)
        {
            int output = 0;
            while (!int.TryParse(input, out output) || Convert.ToInt32(input) < 0)
            {
                input = AnsiConsole.Ask<string>("Invalid number:" + message);
            }

            return output;
        }

        public static DateTime ValidateStartDate(string input)
        {
            DateTime date;

            while (!DateTime.TryParseExact(input, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                input = AnsiConsole.Ask<string>("\n Invalid date...");
            }
            return date;
        }

        internal static DateTime ValidateEndDate(DateTime startDate, string endDateInput)
        {
            DateTime endDate;
            while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                endDateInput = AnsiConsole.Ask<string>("\nInvalid date...");
            }

            while (startDate > endDate)
            {
                endDateInput = AnsiConsole.Ask<string>("\n\nEnd date can't be before start date...");

                while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                {
                    endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date...");
                }
            }

            return endDate;
        }
    }
}
