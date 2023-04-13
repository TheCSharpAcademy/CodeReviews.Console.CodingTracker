using System.Globalization;

namespace CodingTracker;

internal class Helpers
{
    internal static string GetDate()
    {
        Console.WriteLine("Please enter date in format dd-mm-yy");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") MainMenu.ShowMenu();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date (Format needed: dd-mm-yy");
            GetDate();
        }

        return dateInput;
    }

    internal static string GetTime()
    {
        Console.WriteLine("Please enter start time in format mm-HH (24hr clock)");
        string timeInput = Console.ReadLine();
        if(timeInput == "0") MainMenu.ShowMenu();

        while (!DateTime.TryParseExact(timeInput, "mm-HH", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid time (Format needed mm-hh");
            MainMenu.ShowMenu();
        }
        return timeInput;
    }

    internal static string GetEndTime()
    {
        Console.WriteLine("Please enter end time in format mm-hh (24hr clock");
        string timeInput = Console.ReadLine() ;
        if (timeInput == "0") MainMenu.ShowMenu();

        while(!DateTime.TryParseExact(timeInput, "mm-HH", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid time (Format needed mm-hh");
            MainMenu.ShowMenu();
        }
        return timeInput;
    }
}
