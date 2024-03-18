using Spectre.Console;
using static CodingTracker.utils.Validation;

namespace CodingTracker.utils;

/// <summary>
/// Represents a class for handling user input.
/// </summary>
public class UserInput
{
    /// <summary>
    /// Get the user input for dates.
    /// </summary>
    /// <param name="singleDate">Flag indicating if only a single date is required.</param>
    /// <returns>An array of DateTime objects representing the start and end dates.</returns>
    internal DateTime[] GetDateInputs(bool singleDate = false)
    {
        AnsiConsole.WriteLine(singleDate ? "Enter the date." : "Enter the start date.");
        var startDate = ValidateDate();

        if (singleDate)
        {
            return [startDate];
        }

        AnsiConsole.WriteLine("Enter the end date.");
        var endDate = ValidateDate();

        while (startDate > endDate)
        {
            AnsiConsole.WriteLine("The end date must be after the start date.");
            endDate = ValidateDate();
        }

        return [startDate, endDate];
    }

    internal int GetIdInput()
    {
        var id = ValidateNumber("Enter the ID of the record.");
        
        return (int)id;
    }
}