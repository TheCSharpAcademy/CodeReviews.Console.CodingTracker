using System.Globalization;
using System.Runtime.InteropServices;

namespace CodingTracker.Validation
{
    public static class InputValidator
    {

        public static bool ValidateDateInput(string input)
        {
            return DateTime.TryParseExact(input, "MM/dd/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var result);
        }

        public static bool ValidateTimeInput(string input)
        {
            return DateTime.TryParseExact(input, "HH:mm", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var result);
        }
    }
}