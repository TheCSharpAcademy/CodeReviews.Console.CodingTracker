using System.ComponentModel.DataAnnotations;

namespace CodingTracker.iGoodw1n;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class IsDateAfterAttribute : ValidationAttribute
{
    private readonly string _testedPropertyName;

    public IsDateAfterAttribute(string testedPropertyName)
    {
        _testedPropertyName = testedPropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var propertyTestedInfo = validationContext.ObjectType.GetProperty(_testedPropertyName);
        if (propertyTestedInfo == null)
        {
            return new ValidationResult(string.Format("unknown property {0}", _testedPropertyName));
        }

        var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

        if (value is not DateTime || propertyTestedValue is not DateTime)
        {
            return ValidationResult.Success;
        }

        // Compare values
        if ((DateTime)value > (DateTime)propertyTestedValue)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("End date should be bigger than start date");
    }
}