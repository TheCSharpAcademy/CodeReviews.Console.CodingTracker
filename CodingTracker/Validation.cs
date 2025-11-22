using System.Globalization;
using Spectre.Console;
namespace CodingTracker
{
    public class Validation
    {
        public static ValidationResult ValidateEndDate(string end, string start)
        {
            DateTime startDate = DateTime.ParseExact(start, DateFormats.DateStringFormat, CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(end, DateFormats.DateStringFormat, CultureInfo.InvariantCulture);
            return endDate <= startDate ? ValidationResult.Error("Must Be After Start Time") : ValidationResult.Success();
        }

        public static ValidationResult ValidateEndDate(DateTime end, DateTime start)
        {
            return end <= start ? ValidationResult.Error("Must Be After Start Time") : ValidationResult.Success();
        }

        public static ValidationResult ValidatePositiveInteger(int num)
        {
            return num > 0 ? ValidationResult.Success() : ValidationResult.Error("Must Be Bigger Than Zero");
        }

        public static ValidationResult ValidateDateString(string date)
        {
            return DateTime.TryParseExact(date, DateFormats.DateStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _) ? ValidationResult.Success() : ValidationResult.Error("Invalid Date Time Format");
        }

        public static ValidationResult ValidateDateOnlyString(string date)
        {
            return DateTime.TryParseExact(date, DateFormats.DateOnlyStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _) ? ValidationResult.Success() : ValidationResult.Error("Invalid Date Time Format");
        }
    }
}
