using System.Globalization;

namespace CodingTracker;

public class UserInput(string defaultDate, string defaultTime)
{
    public string NumberInput()
    {
        string? input;
        bool validNumber = false;

        do
        {
            input = Console.ReadLine();

            validNumber = int.TryParse(input, out _);

            if (string.IsNullOrWhiteSpace(input) || !validNumber)
            {
                Console.WriteLine("Invalid input.");
            }
            else
            {
                validNumber = true;
            }
        } while (!validNumber);
        return input!;
    }


    public TimeOnly HourInput()
    {
        string? input;

        TimeOnly time;

        while (true)
        {
            input = Console.ReadLine();

            if (TimeOnly.TryParseExact(input, defaultTime, new CultureInfo("en-US"), DateTimeStyles.None, out time))
            {
                return time;
            }
            else
            {
                Console.WriteLine($"Invalid input. Make sure the format is -> \"{defaultTime}\".");
            }
        }
    }

    public string DateInput()
    {
        string? date;
        bool validDate = false;

        do
        {
            date = Console.ReadLine();
            validDate = DateTime.TryParseExact(date, defaultDate, new CultureInfo("en-US"), DateTimeStyles.None, out _);

            if (!validDate)
            {
                Console.WriteLine($"Invalid input. Make sure the format is -> \"{defaultDate}\".");
            }
        } while (!validDate);

        return date!;
    }

    public int GetYearByUser()
    {
        Console.WriteLine("Enter the year you want to filter by (YYYY):");
        string? yearInput = NumberInput();
        int year = Convert.ToInt32(yearInput);
        return year;
    }

        public int GetMonthByUser()
    {
        Console.WriteLine("Enter the month you want to filter by (MM):");
        string? monthInput = NumberInput();
        int month = Convert.ToInt32(monthInput);
        return month;
    }
}
