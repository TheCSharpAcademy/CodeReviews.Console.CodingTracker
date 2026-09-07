using Spectre.Console;
using System.Globalization;

namespace CodingTracker.DzemalKurtic;

internal static class Validation
{
    internal static DateTime ValidateDate(string dateInput, string dateName)
    {
        string date = dateInput;

        while (!DateTime.TryParseExact(date, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
             AnsiConsole.WriteLine("Invalid date. (Format: dd-mm-yy hh:mm). Try again:");
            date = AnsiConsole.Ask<string>($"Enter the {dateName} date of the Coding Session: (Format: dd-mm-yy hh:mm)");
        }

        return DateTime.ParseExact(date, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture);
    }


    internal static int ValidateId(int idInput)
    {
        int id = idInput;
        while (Convert.ToInt32(id) < 0)
        {
            AnsiConsole.WriteLine("Number can't be negative. Try again.");
            id = AnsiConsole.Ask<int>("Please typt the Id od the item you want to update");
        }

        return Convert.ToInt32(id);
    }

    internal static bool ValidateTimespan(DateTime start, DateTime end)
    {
        bool isBigger = end <= start;
        if (isBigger) AnsiConsole.WriteLine("End date can not be before start date");
        return isBigger;
    }
}
