using System.Globalization;

class DataValidation
{
    public static bool ValidateInteger(string? input)
    {
        bool validInput;
        input = input?.Trim();

        validInput = int.TryParse(input, out int validInputInt);
        return validInput;
    }

    public static bool ValidateTime(string? startTime)
    {
        bool validInput;

        validInput = TimeOnly.TryParseExact(startTime,"HH:mm", out TimeOnly validStartTime);
        return validInput;
    }

    public static bool ValidateTime(string? startDate, string? startTime, string? endDate, string? endTime)
    {
        bool validInput;

        DateTime.TryParseExact(startDate+" "+startTime,"yyyy/MM/dd HH:mm",CultureInfo.InvariantCulture,
        DateTimeStyles.None ,out DateTime validStartTime);
        DateTime.TryParseExact(endDate+" "+endTime,"yyyy/MM/dd HH:mm",CultureInfo.InvariantCulture,
        DateTimeStyles.None, out DateTime validEndTime);
        
        validInput = validEndTime > validStartTime;
        return validInput;
    }

    public static bool ValidateDate(string? startDate)
    {
        bool validInput;
        
        validInput = DateOnly.TryParseExact(startDate, "yyyy/MM/dd",CultureInfo.InvariantCulture, 
        DateTimeStyles.None, out DateOnly validStartDate);
        return validInput;
    }

    public static bool ValidateDate(string? startDate, string? endDate)
    {
        bool validInput;
        
        DateOnly.TryParseExact(startDate, "yyyy/MM/dd",CultureInfo.InvariantCulture, 
        DateTimeStyles.None, out DateOnly validStartDate);
        DateOnly.TryParseExact(endDate, "yyyy/MM/dd",CultureInfo.InvariantCulture, 
        DateTimeStyles.None, out DateOnly validEndDate);
        
        validInput = validEndDate >= validStartDate;
        return validInput;
    }

    public static bool ValidateYesNoQuestion(string? input)
    {
        bool validInput = false;
        input = input?.ToLower().Trim();

        if (input == "y" || input == "n")
        {
            validInput = true;
        }
        return validInput;
    }
}