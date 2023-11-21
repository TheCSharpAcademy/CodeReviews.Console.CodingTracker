using System.Globalization;

class UserInput
{
    public static bool ValidateInteger(string? input)
    {
        bool validInput;

        validInput = int.TryParse(input, out int validInputInt);
        return validInput;
    }

    public static bool ValidateTime(string? input)
    {
        bool validInput;

        validInput = TimeOnly.TryParseExact(input,"HH:mm", out TimeOnly validInputTime);
        return validInput;
    }

    public static bool ValidateDate(string? input)
    {
        bool validInput;
        
        validInput = DateOnly.TryParseExact(input, "yyyy/MM/dd",CultureInfo.InvariantCulture, 
        DateTimeStyles.None, out DateOnly validInputDate);
        return validInput;
    }
}