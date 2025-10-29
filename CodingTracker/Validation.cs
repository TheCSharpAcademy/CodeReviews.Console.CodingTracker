using Spectre.Console;
namespace CodingTracker
{
    public class Validation
    {
        public static ValidationResult ValidateEndDate(DateTime end, DateTime start)
        {
            return end <= start ? ValidationResult.Error("must be after start time") : ValidationResult.Success();
        }

    }
}
