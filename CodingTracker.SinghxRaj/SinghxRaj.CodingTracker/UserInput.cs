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

        string startStr = $"{startDate} {startTime}";

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

        string endDate = GetEndDateInfo();
        string endTime = GetEndTimeInfo();

        string endStr = $"{endDate} {endTime}";

        DateTime.TryParseExact(endStr, TimeFormat.SessionTimeStampFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out end);

        while (!Validator.ValidateSessionDateTimes(start, end))
        {
            Console.WriteLine("Invalid end date-time for session. Try again.");
            endDate = GetEndDateInfo();
            endTime = GetEndTimeInfo();

            endStr = $"{endDate} {endTime}";

            DateTime.TryParseExact(endStr, TimeFormat.SessionTimeStampFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
        }

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

    internal static int GetId()
    {
        Console.WriteLine("Type the id of the coding session:");
        string? idInput = Console.ReadLine();
        int id;
        while(!int.TryParse(idInput, out id))
        {
            Console.WriteLine("Invalid input for id. Try again.");
            idInput = Console.ReadLine();
        }
        return id;
    }

    internal static CodingSession GetUpdatedCodingSessionInfo(int sessionId)
    {
        CodingSession previousSession = DatabaseManager.GetCodingSession(sessionId);

        if (previousSession == null)
        {
            return null!;
        }

        DateTime start = UpdateStartSession(previousSession); ;

        DateTime end = UpdateEndSession(previousSession, start);

        TimeSpan duration = CalculateDuration(start, end);  

        return new CodingSession(sessionId, start, end, duration);
    }

    private static DateTime UpdateEndSession(CodingSession previousSession, DateTime start)
    {
        if (!Validator.ValidateSessionDateTimes(start, previousSession.EndTime))
        {
            Console.WriteLine("Since start time is now later than the end time. End time will have to change.");
            return GetEndInfo(start);
        } else
        {
            Console.WriteLine("Would you like to change the end time of the session? (yes or no)");
            string? sessionStartResponse = Console.ReadLine();
            sessionStartResponse = sessionStartResponse?.Trim().ToLower();
            if (Validator.ValidateResponse(sessionStartResponse) && sessionStartResponse!.StartsWith("y"))
            {
                return GetEndInfo(start);
            }
            else
            {
                return previousSession.EndTime;
            }
        }
       
        
    }

    private static DateTime UpdateStartSession(CodingSession previousSession)
    {
        Console.WriteLine("Would you like to change the start time of the session? (yes or no)");
        string? sessionStartResponse = Console.ReadLine();
        sessionStartResponse = sessionStartResponse?.Trim().ToLower();
        if (sessionStartResponse != null && sessionStartResponse!.Length > 0
            && sessionStartResponse.StartsWith("y"))
        {
            return GetStartInfo();
        }
        else
        {
            return previousSession.StartTime;
        }
    }
}