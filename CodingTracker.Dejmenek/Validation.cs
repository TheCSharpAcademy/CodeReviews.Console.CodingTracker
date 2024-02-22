using Spectre.Console;
using System.Globalization;

namespace CodingTracker.Dejmenek
{
    public class Validation
    {
        public static ValidationResult IsPositiveNumber(int userNumber)
        {
            return userNumber switch
            {
                <= 0 => ValidationResult.Error("[red]You must enter a positive number.[/]"),
                _ => ValidationResult.Success(),
            };
        }

        public static bool IsChronologicalOrder(DateTime startDate, DateTime endDate)
        {
            int result = DateTime.Compare(startDate, endDate);

            if (result < 0 || result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ValidationResult IsValidDateTimeFormat(string? userDate)
        {
            if (DateTime.TryParseExact(userDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return ValidationResult.Success();
            }
            else
            {
                return ValidationResult.Error("[red]You must enter a valid date time format: yyyy-MM-dd HH:mm[/]");
            }
        }

        public static ValidationResult IsValidDateFormat(string? userDate)
        {
            if (DateTime.TryParseExact(userDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return ValidationResult.Success();
            }
            else
            {
                return ValidationResult.Error("[red]You must enter a valid date format: yyyy-MM-dd[/]");
            }
        }
    }
}
