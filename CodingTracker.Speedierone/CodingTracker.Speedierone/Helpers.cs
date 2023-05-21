using System.Globalization;

namespace CodeTracker;
internal class Helpers
{
    internal static string GetDate()
    {
        Console.WriteLine("Please enter date in format dd-mm-yy");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") MainMenu.ShowMenu();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date (Format needed: dd-mm-yy)");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }
    internal static string GetStartTime()
    {
        Console.WriteLine("Please enter start time in format HH:mm (24hr clock)");
        var startTime = Console.ReadLine();
        if (startTime == "0") MainMenu.ShowMenu();
        while (!DateTime.TryParseExact(startTime, "HH:mm", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid input. Please enter time in format hh:mm");
            startTime = Console.ReadLine();
        }
        return startTime;
    }
    internal static string GetEndTime()
    {
        Console.WriteLine("Please enter end time in format hh:mm (24hr clock)");
        var endTime = Console.ReadLine();
        if (endTime == "0") MainMenu.ShowMenu();
        while (!DateTime.TryParseExact(endTime, "HH:mm", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid input. Please enter time in format hh-mm");
            endTime = Console.ReadLine();
        }   
        return endTime;
    }
    internal static string CodingTime(string timeStart, string timeEnd)
    {
        var parsedTimeStart = DateTime.Parse(timeStart);
        var parsedTimeEnd = DateTime.Parse(timeEnd);
        TimeSpan codingTime = parsedTimeEnd - parsedTimeStart;
        var stringCodingTime = codingTime.ToString();
        return stringCodingTime;
    }
}
