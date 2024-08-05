using System.Globalization;

namespace CodingTracker;

public class Validate
{
    internal static bool ValidateDate(string? input)
    {
        bool valid = false;
        string format = "dd/MM/yy";

        if (input == null)
        {
            Views.ShowError("Input is null. Please input a date.");
        }
        else if (!DateTime.TryParseExact(input, format, new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Views.ShowError("Incorrect format. Please input a date following the specified format. (dd/mm/yy)");
        }
        else
        {
            valid = true;
        }


        return valid;
    }

    internal static bool ValidateTime(string? input)
    {
        bool valid = false;
        string format = "h\\:mm tt";

        if (input == null)
        {
            Views.ShowError("Input is null. Please input a time.");
        }
        else if (!DateTime.TryParseExact(input, format, new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Views.ShowError("Incorrect format. Please input a time following the specified format. (Example: 12:30 AM)");
        }
        else
        {
            valid = true;
        }

        return valid;
    }

    internal static bool ValidateNumber(string? input)
    {
        bool valid = false;

        if (input == null)
        {
            Views.ShowError("Input is null.");
        }
        else if (!int.TryParse(input, out _))
        {
            Views.ShowError("Please input a number.");
        }
        else
        {
            valid = true;
        }

        return valid;
    }
}