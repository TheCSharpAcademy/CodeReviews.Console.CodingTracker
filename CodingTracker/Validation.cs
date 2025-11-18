using Spectre.Console;
namespace CodingTracker
{
    public class Validation
    {
        public static ValidationResult ValidateEndDate(DateTime end, DateTime start)
        {
            return end <= start ? ValidationResult.Error("Must Be After Start Time") : ValidationResult.Success();
        }

        public static ValidationResult ValidatePositiveInteger(int num)
        {
            return num > 0 ? ValidationResult.Success() : ValidationResult.Error("Must Be Bigger Than Zero");
        }

    }
}
