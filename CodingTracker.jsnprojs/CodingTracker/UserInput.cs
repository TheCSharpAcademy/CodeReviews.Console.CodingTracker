using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker;
internal class UserInput
{
    internal static string GetDateInput(string startOrEnd)
    {
        Console.WriteLine($"\n\nPlease insert the date for {startOrEnd}: (Format: hours(24 hours system)-day-month-year). Type 0 to return to main manu.\n\n");

        string dateInput = Console.ReadLine();

        if (dateInput == "0") { Menu.GetUserInput(); }

        while (!DateTime.TryParseExact(dateInput, "HH-dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: hours-day-month-year). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine();
            if (dateInput == "0") {Menu.GetUserInput(); }
        }

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") Menu.GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}
