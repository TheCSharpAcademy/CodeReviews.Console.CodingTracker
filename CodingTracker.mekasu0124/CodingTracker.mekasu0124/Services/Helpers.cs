using CodingTracker.Models;

namespace CodingTracker.Services;

internal class Helpers
{
    public static string GetDateTime()
    {
        DateTime startTimeFormat = DateTime.UtcNow;
        TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;
        DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(startTimeFormat, systemTimeZone);

        return localDateTime.ToString();
    }

    public static string GetFormattedDate()
    {
        DateTime now = DateTime.UtcNow;
        TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;
        DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(now, systemTimeZone);

        return localDateTime.ToString("MM/dd/yyyy");
    }

    public static string CalculateDuration(string startTime, string endTime)
    {
        TimeSpan start = TimeSpan.Parse(startTime);
        TimeSpan end = TimeSpan.Parse(endTime);

        string difference = end.Subtract(start).ToString();
        return difference;
    }

    public static void PrintTimeSheet()
    {
        List<string> times = new()
        {
            "01:00 -> 1 AM    |   13:00 -> 1 PM",
            "02:00 -> 2 AM    |   14:00 -> 2 PM",
            "03:00 -> 3 AM    |   15:00 -> 3 PM",
            "04:00 -> 4 AM    |   16:00 -> 4 PM",
            "05:00 -> 5 AM    |   17:00 -> 5 PM",
            "06:00 -> 6 AM    |   18:00 -> 6 PM",
            "07:00 -> 7 AM    |   19:00 -> 7 PM",
            "08:00 -> 8 AM    |   20:00 -> 8 PM",
            "09:00 -> 9 AM    |   21:00 -> 9 PM",
            "10:00 -> 10 AM   |   22:00 -> 10 PM",
            "11:00 -> 11 AM   |   23:00 -> 11 PM",
            "12:00 -> 12 AM   |   00:00 -> 12 AM"
        };

        foreach (string time in times)
        {
            Console.WriteLine(time);
        }
    }

    public static void FinishedCodingSession(CodeSession session, string action)
    {
        Console.WriteLine($"Start Date: {session.TodaysDate}");
        Console.WriteLine($"Start Time: {session.StartTime}");
        Console.WriteLine($"End Time: {session.EndTime}");
        Console.WriteLine($"Duration: {session.Duration}");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Successfully {action} Session Data.");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Press Enter To Go To Main Menu");
        Console.ReadLine();
        Console.Clear();
        MainMenu.ShowMenu();
    }
}
