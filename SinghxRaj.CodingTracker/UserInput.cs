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

        while (!Validator.ValidateEnd(end))
        {
            Console.WriteLine("Invalid end time. Try again.");
            end = GetEndInfo(start);
        }

        TimeSpan duration = CalculateDuration(start, end);

        return new CodingSession(start, end, duration);

    }

    private static TimeSpan CalculateDuration(DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    private static DateTime GetEndInfo(DateTime start)
    {
        string endDate = GetEndDateInfo();
        string endTime = GetEndTimeInfo();

        string endStr = endDate + endTime;

        DateTime.TryParseExact(endStr, "dd-MM-yy hh-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end);

        return end;
    }

    private static string GetEndTimeInfo()
    {
        throw new NotImplementedException();
    }

    private static string GetEndDateInfo()
    {
        throw new NotImplementedException();
    }

    private static DateTime GetStartInfo()
    {
        string startDate = GetStartDateInfo();
        string startTime = GetStartTimeInfo();

        string startStr = startDate + startTime;

        DateTime.TryParseExact(startStr, "dd-MM-yy hh-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start);

        return start;
    }

    private static string GetStartTimeInfo()
    {
        throw new NotImplementedException();
    }

    private static string GetStartDateInfo()
    {
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