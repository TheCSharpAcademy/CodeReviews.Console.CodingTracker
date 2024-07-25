using System.Globalization;
using Spectre.Console;

namespace CodingTracker;
internal class InputValidator
{
    public static string CleanString()
    {
        string? input = Console.ReadLine();
        while (true)
        {
            input ??= "";
            input = input.Trim();

            if (input.Length < 0)
                AnsiConsole.MarkupLine("[red]Please provide a valid response[/]");
            else if (input == "0")
                Environment.Exit(0);
            else
                break;
        }
        return input;
    } // end of CleanString Method
    public static int CleanInt()
    {
        if (int.TryParse(CleanString(), out int output))
            return output;
        else
            return -1;
    } // end of CleanInt Method
    public static DateTime GetDate()
    {
        DateTime dateTime;
        while (true)
        {
            try
            {
                dateTime = DateTime.ParseExact(CleanString(), "MM/dd/yy hh:mm tt", CultureInfo.InvariantCulture);
                break;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{ex.Message} Reenter in [blue](MM/dd/yy hh:mm tt)[/] format[/]");
            }
        }
        return dateTime;
    } // end of GetDate Method
} // end of UserInput Class
