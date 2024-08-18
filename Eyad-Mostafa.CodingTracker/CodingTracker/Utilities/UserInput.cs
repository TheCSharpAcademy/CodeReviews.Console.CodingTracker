using System.Globalization;

namespace CodingTracker.Utilities;

internal static class UserInput
{
    internal static string GetSessionTime(string message)
    {
        Console.WriteLine(message);
        string? time = Console.ReadLine()?.Trim();

        if (time == "0") return "0";

        while (String.IsNullOrEmpty(time) || !TimeOnly.TryParse(time, out _))
        {
            Console.WriteLine("Please Enter a Valid Format");
            time = Console.ReadLine()?.Trim();

            if (time == "0")
                return "";
        }
        return TimeOnly.Parse(time).ToString("H:mm");
    }

    internal static int CalculateDuration(string StartTime, string EndTime)
    {
        var start = TimeSpan.Parse(StartTime);
        var end = TimeSpan.Parse(EndTime);
        return ((int)(end - start).TotalMinutes);
    }

    internal static string GetDateInput(string message)
    {
        Console.WriteLine(message);

        string? dateInput = Console.ReadLine()?.Trim();

        if (dateInput == "0") return "0";

        while (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nInvalid date format, Please try again or type 0 to return to main menu");
            dateInput = Console.ReadLine()?.Trim();

            if (dateInput == "0") return "0";
        }

        return DateTime.Parse(dateInput).ToString("dd/MMM/yyyy");
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string? value = Console.ReadLine()?.Trim();

        if (value == "0") return 0;

        while (String.IsNullOrEmpty(value) || !int.TryParse(value, out _))
        {
            Console.WriteLine("Please Enter a valid numeric value. Or type 0 to return to main menu");
            value = Console.ReadLine()?.Trim();

            if (value == "0") return 0;
        }
        return int.Parse(value);
    }
}
