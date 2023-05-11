using System.Globalization;

namespace CodingTracker.barakisbrown;

public static class Input
{
    private readonly static string _validDateFormat = "mm-dd-yyyy";
    private readonly static string _validTimeFormat = "hh:mm";
    private readonly static string _dateInputString = $"Enter the date in the following format only [{_validDateFormat}] or Enter to use today";
    private readonly static string _timeInputString = $"Enter the time in the following format only [{_validTimeFormat}] or Enter to use current time";
    
    public static bool GetYesNo()
    {
        ConsoleKeyInfo input = Console.ReadKey(true);
        if (input.Key == ConsoleKey.Y)
            return true;
        else
            return false;
    }

    public static DateOnly GetDate()
    {
        Console.WriteLine(_dateInputString);
        string? result = Console.ReadLine();

        while (true)
        {
            if (result == string.Empty)
                return DateOnly.FromDateTime(DateTime.Now);
            try
            {
                DateTime.TryParseExact(result, _validDateFormat, new CultureInfo("en-us"), DateTimeStyles.None, out DateTime date);
                return DateOnly.FromDateTime(date);
            }
            catch (FormatException _)
            {
                Console.WriteLine($"Date has to in the following format: {_validDateFormat} ");
                Console.WriteLine("Please try again.");
                Console.WriteLine(_dateInputString);
                result = Console.ReadLine();
            }
        }
    }

    public static TimeOnly GetTime()
    {
        Console.WriteLine(_timeInputString);
        string? result = Console.ReadLine();

        while (true)
        {
            if (result == string.Empty)
                return TimeOnly.FromDateTime(DateTime.Now);
            else
            {
                var parse = result?.Split(":");
                int hourInt = int.Parse(parse[0]);
                int minuteInt = int.Parse(parse[1]);

                if (hourInt >= 24)
                {
                    Console.WriteLine("Invalid Hour : Hour can not greater to be less than 24");
                    continue;
                }
                if (minuteInt >= 60)
                {
                    Console.WriteLine("Invalid time.");
                }

                var time = new TimeOnly();
                time.AddHours(hourInt);
                time.AddMinutes(minuteInt);

                return time;
            }
        }
    }
}
