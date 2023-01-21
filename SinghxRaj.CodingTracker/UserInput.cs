using System.Globalization;

namespace SinghxRaj.CodingTracker;

internal class UserInput
{
    public static int GetOption()
    {
        Console.WriteLine("Type your option:");

        int option;
        string? input = Console.ReadLine();
        while (!int.TryParse(input, out option) || !Validator.ValidateOption(option))
        {
            Console.WriteLine("Invalid option. Type option again:");
            input = Console.ReadLine();
        }
        return option;
    }

    internal static CodingSession GetCodingSessionInfo()
    {
        DateTime start = GetStartInfo();
        DateTime end = GetEndInfo(start);

        TimeSpan duration = CalculateDuration(start, end);

        return new CodingSession(start, end, duration);

    }

    private static TimeSpan CalculateDuration(DateTime start, DateTime end)
    {
        return end - start;
    }

    private static DateTime GetStartInfo()
    {
        string startDate = GetStartDateInfo();
        string startTime = GetStartTimeInfo();

        string startStr = startDate + startTime;

        DateTime.TryParseExact(startStr, TimeFormat.SessionTimeStampFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start);


        return start;
    }

    private static string GetStartDateInfo()
    {
        Console.WriteLine("Type the start date of the session (format mm-dd-yy)");
        string? dateInput = Console.ReadLine();
        while (!Validator.ValidateDate(dateInput))
        {
            Console.WriteLine("Invalidate input for date.");
            Console.WriteLine("Type date again (format mm-dd-yy):");
            dateInput = Console.ReadLine();
        }
        return dateInput!;
    }

    private static string GetStartTimeInfo()
    {
        Console.WriteLine("Type the start time of the session (format hh:mm [military time]): ");
        string? dateInput = Console.ReadLine();
        while (!Validator.ValidateTime(dateInput))
        {
            Console.WriteLine("Invalidate input for time.");
            Console.WriteLine("Type time again (format hh:mm [military time]):");
            dateInput = Console.ReadLine();
        }
        return dateInput!;
    }

    // `start` is used to validate that end is after the start
    private static DateTime GetEndInfo(DateTime start)
    {
        DateTime end;
        do {
            string endDate = GetEndDateInfo();
            string endTime = GetEndTimeInfo();

            string endStr = $"{endDate} {endTime}";

            DateTime.TryParseExact(endStr, TimeFormat.SessionTimeStampFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
        } while (!Validator.ValidateSessionDateTimes(start, end));

        return end;
    }

    private static string GetEndDateInfo()
    {
        Console.WriteLine("Type the end date of the session (format mm-dd-yy)");
        string? dateInput = Console.ReadLine();
        while (!Validator.ValidateDate(dateInput))
        {
            Console.WriteLine("Invalidate input for date.");
            Console.WriteLine("Type date again (format mm-dd-yy):");
            dateInput = Console.ReadLine();
        }
        return dateInput!;
    }

    private static string GetEndTimeInfo()
    {
        Console.WriteLine("Type the end time of the session (format hh:mm [military time]): ");
        string? dateInput = Console.ReadLine();
        while (!Validator.ValidateTime(dateInput))
        {
            Console.WriteLine("Invalidate input for time.");
            Console.WriteLine("Type time again (format hh:mm [military time]):");
            dateInput = Console.ReadLine();
        }
        return dateInput!;
    }
}