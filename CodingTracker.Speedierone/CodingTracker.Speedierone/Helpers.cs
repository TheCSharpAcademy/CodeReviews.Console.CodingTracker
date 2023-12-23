using System.Globalization;

namespace CodeTracker;
public class Helpers
{
    public static bool IsValidDate(string dateInput)
    {
        return DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-GB"), DateTimeStyles.None, out _);
    }
    public static bool IsValidTime(string timeInput)
    {
        return DateTime.TryParseExact(timeInput, "HH-mm", new CultureInfo("en-GB"), DateTimeStyles.None, out _);
    }
    public static string GetDate()
    {
        Console.WriteLine("Please enter date in format dd-mm-yy");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") 
        { 
            MainMenu.ShowMenu(); 
            return null;
        }

        while (!IsValidDate(dateInput))
        {
            Console.WriteLine("Invalid date (Format needed: dd-mm-yy)");
            dateInput = Console.ReadLine();

            if(dateInput == "0")
            {
                MainMenu.ShowMenu();
                return null;
            }
        }
        return dateInput;
    }
    public static string GetStartTime()
    {
        Console.WriteLine("Please enter start time in format HH:mm (24hr clock)");
        var startTime = Console.ReadLine();
        if (startTime == "0") MainMenu.ShowMenu();
        while (IsValidTime(startTime))
        {
            Console.WriteLine("Invalid input. Please enter time in format hh:mm");
            startTime = Console.ReadLine();
        }
        return startTime;
    }
    public static string GetEndTime()
    {
        Console.WriteLine("Please enter end time in format hh:mm (24hr clock)");
        var endTime = Console.ReadLine();
        if (endTime == "0") MainMenu.ShowMenu();
        while (IsValidTime(endTime))
        {
            Console.WriteLine("Invalid input. Please enter time in format hh-mm");
            endTime = Console.ReadLine();
        }   
        return endTime;
    }
    public static string CodingTime(string timeStart, string timeEnd)
    {
        var parsedTimeStart = DateTime.Parse(timeStart);
        var parsedTimeEnd = DateTime.Parse(timeEnd);
        TimeSpan codingTime = parsedTimeEnd - parsedTimeStart;
        var stringCodingTime = codingTime.ToString();
        return stringCodingTime;
    }
}
