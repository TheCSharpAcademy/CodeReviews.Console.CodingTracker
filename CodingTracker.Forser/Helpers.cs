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
}