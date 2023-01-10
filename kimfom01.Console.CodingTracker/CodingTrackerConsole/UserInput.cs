using System.Globalization;

namespace CodingTrackerConsole;

public class UserInput
{
    private readonly Validation _dateTimeValidation = new();

    public string GetDate()
    {
        Console.Write("Enter date (mm-dd-yyyy): ");
        var date = Console.ReadLine();

        while (!_dateTimeValidation.IsValidDate(date))
        {
            Console.WriteLine("Invalid date! Expected format (mm-dd-yyyy)");
            date = Console.ReadLine();
        }

        return date;
    }

    private string GetTime(string? type)
    {
        Console.Write($"Enter {type} time (hh:mm): ");
        var time = Console.ReadLine();

        while (!_dateTimeValidation.IsValidTime(time))
        {
            Console.Write("Invalid time! Expected format (hh:mm): ");
            time = Console.ReadLine();
        }

        return time;
    }

    public (string, string) GetTimes()
    {
        var startTime = GetTime("start");
        var endTime = GetTime("end");
        while (!_dateTimeValidation.IsValidTimeOrder(startTime, endTime))
        {
            Console.WriteLine("Start time cannot be greater than end time!");
            startTime = GetTime("start");
            endTime = GetTime("end");
        }

        return (startTime, endTime);
    }

    public string GetDuration(string startTime, string endTime)
    {
        DateTime parsedStartTime = DateTime.ParseExact(startTime, "HH:mm", null, DateTimeStyles.None);
        DateTime parsedEndTime = DateTime.ParseExact(endTime, "HH:mm", null, DateTimeStyles.None);

        TimeSpan duration = parsedEndTime.Subtract(parsedStartTime);

        if (duration < TimeSpan.Zero)
        {
            duration += TimeSpan.FromDays(1);
        }

        return duration.ToString();
    }

    public string GetChoice()
    {
        return Console.ReadLine().Trim().ToLower();
    }

    public string GetDay()
    {
        Console.Write("Enter day to filter with (Format: dd):");
        var day = Console.ReadLine();

        while (!_dateTimeValidation.IsValidDay(day))
        {
            Console.WriteLine("Invalid day! Expected format (dd)");
            day = Console.ReadLine();
        }

        return day;
    }

    public string GetMonth()
    {
        Console.Write("Enter month to filter with (Format: mm): ");
        var month = Console.ReadLine();

        while (!_dateTimeValidation.IsValidMonth(month))
        {
            Console.WriteLine("Invalid month! Expected format (mm): ");
            month = Console.ReadLine();
        }

        return month;
    }

    public string GetYear()
    {
        Console.Write("Enter year to filter with (Format: yyyy): ");
        var year = Console.ReadLine();

        while (!_dateTimeValidation.IsValidYear(year))
        {
            Console.WriteLine("Invalid month! Expected format (yyyy)");
            year = Console.ReadLine();
        }

        return year;
    }
}