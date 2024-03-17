using System.Globalization;
using Spectre.Console;

namespace CodingTracker.utils;

/// <summary>
/// Provides validation methods for various inputs.
/// </summary>
public static class Validation
{
    /// <summary>
    /// Represents an exception that is thrown when the user wants to return back to the main menu.
    /// </summary>
    public sealed class ReturnBackException(string message = "\nExiting to menu.") : Exception(message);

    /// <summary>
    /// Validates a date input provided by the user.
    /// </summary>
    /// <param name="message">The message to display when prompting for date input.</param>
    /// <returns>The validated DateTime object representing the date input.</returns>
    internal static DateTime ValidateDate(string message = "Enter the date in the format: dd-mm-yy hh:mm (24h clock).")
    {
        DateTime dateValue;
        bool isValid;

        do
        {
            var input = AnsiConsole.Ask<string>(message);

            try
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    CheckForZero(input);
                }

            }
            catch (ReturnBackException e)
            {
                AnsiConsole.WriteLine(e.Message);
                throw;
            }

            isValid = DateTime.TryParseExact(
                input, "dd-MM-yy HH:mm", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out dateValue
                ) && dateValue < DateTime.Now && dateValue > DateTime.Now.AddYears(-10);

            if (!isValid)
            {
                AnsiConsole.WriteLine("Invalid input or future date. Please enter a date in the past in the format:\n" +
                                      "dd-mm-yy hh:mm (24h clock) and no more than 10 year ago.\n" +
                                      "Enter 0 'zero' to exit to main menu.");
            }

        } while (!isValid);

        return dateValue;
    }

    /// <summary>
    /// Validates the user input to ensure it is a positive number within the specified limits.
    /// </summary>
    /// <param name="message">The message to display to the user when asking for input.</param>
    /// <param name="topLimit">The upper limit of the number.</param>
    /// <param name="bottomLimit">The lower limit of the number.</param>
    /// <returns>The validated positive number within the specified limits.</returns>
    internal static uint ValidateNumber(string message = "Enter a positive number.", uint topLimit = uint.MaxValue, uint bottomLimit = 0)
    {
        uint number;
        bool isValid;

        do
        {
            var input = AnsiConsole.Ask<string>(message);

            if (!string.IsNullOrWhiteSpace(input))
            {
                CheckForZero(input);
            }
            
            isValid = uint.TryParse(input, out number) && number <= topLimit && number >= bottomLimit;
            
            if (!isValid)
            {
                AnsiConsole.WriteLine("Invalid input. Please enter a positive number or 0 to exit to main menu.");
            }
            
        } while(!isValid);

        return number;
    }

    /// <summary>
    /// Check if the input string is equal to "0" and throw an exception if true.
    /// </summary>
    /// <param name="input">The input string to check</param>
    private static void CheckForZero(string input)
    {
        if (input.Equals("0"))
        {
            throw new ReturnBackException();
        }
    }
}