using Spectre.Console;

internal class UserInput
{
    public static string GetUserDate()
    {
        var endAsk = false;

        while (!endAsk)
        {
            var userDate = AnsiConsole.Ask<string>($"Enter a date ({Globals.DATE_FORMAT}): ");
            var validDate = Validation.CheckValidDate(userDate);

            if(validDate)
            {
                return userDate;
            }
            else
            {
                AnsiConsole.WriteLine($"Enter a valid date {Globals.DATE_FORMAT}", new Style(Color.Red));
            }
        }

        return string.Empty;
    }

    public static string GetUserTime()
    {
        var endAsk = false;
        var time = DateTime.MinValue;

        while (!endAsk)
        {
            var userTime = AnsiConsole.Ask<string>($"Enter a time ({Globals.TIME_FORMAT}): ");
            var validTime = Validation.CheckValidTime(userTime);

            if (userTime != null && validTime)
            {
                return userTime;
            }
            else
            {
                AnsiConsole.WriteLine($"Enter a valid Time {Globals.TIME_FORMAT}", new Style(Color.Red));
            }
        }

        return string.Empty;
    }
}