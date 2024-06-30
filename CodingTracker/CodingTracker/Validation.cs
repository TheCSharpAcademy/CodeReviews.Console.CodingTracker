using Spectre.Console;
using System.Globalization;
namespace CodingTracker
{
    public class Validation
    {
        public Validation(string value)
        {
            bool result = ValidString(value); //2024.06.29.
            if (result)
            {
                AnsiConsole.MarkupLine("[yellow bold]Date passed the test.[/]");
            }
        }

        public bool ValidString(string value)
        {
            return DateTime.TryParseExact(value, "yyyy.MM.dd.", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _);
        }
    }
}
