using System.ComponentModel.Design;
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
                Console.WriteLine("Invalid date (Format needed: dd-mm-yy)");
                dateInput = Console.ReadLine();
            }
            
        return dateInput;
    }

    internal static string GetTime()
    {
        Console.WriteLine("Please enter start time in format hh:mm (24hr clock)");
        string timeInput = Console.ReadLine();
        if(timeInput == "0") MainMenu.ShowMenu();

        while (!DateTime.TryParseExact(timeInput, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid time (Format needed hh:mm)");
            timeInput = Console.ReadLine();
        }
        return timeInput;
    }

    internal static string GetEndTime()
    {
        Console.WriteLine("Please enter end time in format hh:mm (24hr clock");
        string timeInputEnd = Console.ReadLine() ;
        if (timeInputEnd == "0") MainMenu.ShowMenu();

        while(!DateTime.TryParseExact(timeInputEnd, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid time (Format needed hh:mm");
            timeInputEnd= Console.ReadLine() ;
        }
        return timeInputEnd;
    }

    internal static string GetTimeDifference(string time)
    {
        var startTime = GetTime();
        var endTime = GetEndTime();
        var parsedStartTime = DateTime.Parse(startTime);
        var parsedEndTime = DateTime.Parse(endTime);
        TimeSpan timeSpan = parsedStartTime - parsedEndTime;
        string stringTimeSpan = timeSpan.ToString();
        return stringTimeSpan;       
    }
}
