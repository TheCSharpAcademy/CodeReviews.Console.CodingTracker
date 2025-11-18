using Spectre.Console;
namespace CodingTracker
{
    public class Validation
    {
        public static ValidationResult ValidateEndDate(DateTime end, DateTime start)
        {
            return end <= start ? ValidationResult.Error("must be after start time") : ValidationResult.Success();
        }

        public static ValidationResult ValidatePositiveInteger(int num)
        {
            return num > 0 ? ValidationResult.Success() : ValidationResult.Error("must be bigger than zero");
        }

    }
}
