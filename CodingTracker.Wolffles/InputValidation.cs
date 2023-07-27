using System;
using System.Globalization;
using System.Linq;

namespace CodingTracker.Wolffles;

internal class InputValidation
{
    static public string GetUserInputAsDate()
    {
        string input;
        string format = "M/d/yyyy h:mm:ss tt";
        bool isDate;
        DateTime date;
        do
        {
            Console.WriteLine($"Please enter date in {format} format.");
            input = Console.ReadLine();
            isDate = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
        while (!isDate);

        return date.ToString(format);
    }
    static public int GetUserInputAsInt()
    {
        int userInput;
        bool isNumerical;

        do
        {
            Console.WriteLine("Please enter an integer: ");
            isNumerical = int.TryParse(Console.ReadLine(), out userInput);
        }
        while (!isNumerical);

        return userInput;
    }

}
