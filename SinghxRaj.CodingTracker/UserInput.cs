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

    private static DateTime GetEndInfo(DateTime start)
    {
        // TODO
        string endDate = GetEndDateInfo();
        string endTime = GetEndTimeInfo();

        string endStr = endDate + endTime;

        DateTime.TryParseExact(endStr, TimeFormat.SessionTimeStampFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end);

        return end;
    }

    private static string GetEndTimeInfo()
    {
        // TODO
        throw new NotImplementedException();
    }

    private static string GetEndDateInfo()
    {
        // TODO
        throw new NotImplementedException();
    }

    private static DateTime GetStartInfo()
    {
        // TODO
        string startDate = GetStartDateInfo();
        string startTime = GetStartTimeInfo();

        string startStr = startDate + startTime;

        DateTime.TryParseExact(startStr, TimeFormat.SessionTimeStampFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start);


        return start;
    }

    private static string GetStartTimeInfo()
    {
        Console.WriteLine("Type the start time of the session (format hh:mm)");
        string? dateInput = Console.ReadLine();
        while (!Validator.ValidateStartTime(dateInput))
        {
            Console.WriteLine("Invalidate input for date.");
            Console.WriteLine("Type date again (format dd-mm-yy):");
            dateInput = Console.ReadLine();
        }
        return dateInput!;
    }

    private static string GetStartDateInfo()
    {
        // TODO
        Console.WriteLine("Type the start date of the session (format dd-mm-yy)");
        string? dateInput = Console.ReadLine();
        while (!Validator.ValidateStartDate(dateInput))
        {
            Console.WriteLine("Invalidate input for date.");
            Console.WriteLine("Type date again (format dd-mm-yy):");
            dateInput = Console.ReadLine();
        }
        return dateInput!;
    }

}