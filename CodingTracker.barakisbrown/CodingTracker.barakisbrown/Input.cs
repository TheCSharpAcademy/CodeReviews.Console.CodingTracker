using Serilog;
using System.Globalization;

namespace CodingTracker.barakisbrown;

public static class Input
{
    private readonly static string _validDateFormat = "mm-dd-yyyy";
    private readonly static string _validTimeFormat = "hh:mm";
    private readonly static string _dateInputString = $"Enter the date in the following format [{_validDateFormat}] or Enter to use today";
    private readonly static string _timeInputString = $"Enter the time in the following format [{_validTimeFormat}] \nIE: 23:59 is 11:59pm\nEnter to use current time";
    
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
                Log.Error("F> GetDate() raised exception and caught. Exception Message : {0}", _.Message);
                Console.WriteLine($"Date has to in the following format: {_validDateFormat} ");
                Console.WriteLine("Please try again.");
                Console.WriteLine(_dateInputString);
                result = Console.ReadLine();
            }
        }
    }

    public static TimeOnly GetTime()
    {       
        while (true)
        {
            Console.WriteLine(_timeInputString);
            string? result = Console.ReadLine();

            if (result == string.Empty)
                return TimeOnly.FromDateTime(DateTime.Now);
            else if (!result.Contains(':'))
            {
                Log.Debug("F> GetTime() -- User entered wrong information. Will be told to reenter.");
                Console.WriteLine("Invalid Time Entered. It must be HH:MM");                
            }
            else
            {
                var parse = result?.Split(":");
                int hourInt, minuteInt;
                try
                {
                    hourInt = int.Parse(parse[0]);
                    if (hourInt >= 24)
                    {
                        Log.Debug("F> GetTime() -- User entered wrong information. Will be told to reenter.");
                        Console.WriteLine("Invalid Hour : Hour should be between 0 and 24");
                        continue;
                    }
                }
                catch (FormatException _)
                {
                    Log.Error("F> GetTime() raised an exception and caught. Exception message is {0}", _.Message);
                    Console.WriteLine("Invalid Information. Please make sure it is numerical.");
                    continue;
                }
                try
                {
                    minuteInt = int.Parse(parse[1]);
                    if (minuteInt >= 60)
                    {
                        Log.Debug("F> GetTime() -- User entered wrong information. Will be told to reenter.");
                        Console.WriteLine("Invalid time. Time should be between 0 and 60.");
                        continue;
                    }
                }
                catch (FormatException _)
                {
                    Log.Error("F> GetTime() raised an exception and caught. Exception message is {0}", _.Message);
                    Console.WriteLine("Invalid Information. Please make sure it is numerical.");
                    continue;
                }

                return new TimeOnly(hourInt, minuteInt);
            }
        }
    }
}
