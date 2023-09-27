using CodingTracker.Forser;
using System.Globalization;

internal class Helpers
{ 
    internal static DateTime GetDateInput(string dateMode)
    {
        Console.WriteLine($"\n\nPlease insert the {dateMode}: (Format: dd-mm-yy hh:mm). Type 0 to return to main menu.\n\n");

        string dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy hh:mm.\n\n");
            dateInput = Console.ReadLine();
        }

        return DateTime.Parse(dateInput);
    }

    internal static double CalculateDuration(DateTime startDate, DateTime endDate)
    {
        TimeSpan duration = endDate - startDate;
        return duration.TotalHours;
    }

    internal static Session GetNewSession()
    {
        Session session = new Session();
        Validation newValidator = new Validation("yy-MM-dd HH:mm");

        Console.WriteLine($"\n\nPlease enter your Start Date: - (Input must be in format: {newValidator.selectedFormat})");
        var startDate = Console.ReadLine();

        Console.WriteLine($"\n\nPlease enter your End Date: - (Input must be in format: {newValidator.selectedFormat})");
        var endDate = Console.ReadLine();

        if (newValidator.ValidateFormat(startDate, endDate))
        {
            session.StartDate = DateTime.Parse(startDate);
            session.EndDate = DateTime.Parse(endDate);
            session.TotalDuration = CalculateDuration(session.StartDate, session.EndDate);
            Console.WriteLine("\n\nSession added!");
            return session;
        }
        else
        {
            GetNewSession();
        }

        return session;
    }
}