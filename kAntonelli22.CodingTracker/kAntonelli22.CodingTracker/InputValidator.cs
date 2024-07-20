using System.Globalization;
using Spectre.Console;

namespace CodingTracker;
internal class InputValidator
{
    public static string CleanString(string? input)
    {
        while (true)
        {
            if (input == null)
                input = "";
            
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
    public static int CleanInt(string? input)
    {
        input = CleanString(input);
        Int32.TryParse(input, out int output);
        return output;
    } // end of CleanInt Method
    public static DateTime GetDate(string? input)
    {
        DateTime dateTime;
        bool firstTime = true;
        while (true)
        {
            try
            {
                input = firstTime ? CleanString(input) : CleanString(Console.ReadLine());
                dateTime = DateTime.ParseExact(input, "MM/dd/yy hh:mm tt", CultureInfo.InvariantCulture);
                break;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{ex.Message} Reenter in [blue](MM/dd/yy hh:mm tt)[/] format[/]");
                firstTime = false;
            }
        }
        return dateTime;
    } // end of GetDate Method
} // end of UserInput Class
