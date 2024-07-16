using CodingTracker.Models;
using System.Globalization;

namespace CodingTracker.Services;

/// <summary>
/// Service to handle all user input validation.
/// </summary>
public static class UserInputValidationService
{
    #region Methods

    /// <summary>
    /// Performs validation on a CodingGoal user input .
    /// </summary>
    /// <param name="input">The CodingGoal user input.</param>
    /// <returns>The validation result and message.</returns>
    public static UserInputValidationResult IsValidCodingGoalDuration(double input)
    {
        if (input < 0)
        {
            return new UserInputValidationResult(false, "Coding goal must be greater than zero.");
        }
        
        return new UserInputValidationResult(true, "Validation successful");
    }

    /// <summary>
    /// Performs validation on a StartDate user input for a Report.
    /// </summary>
    /// <param name="input">The StartDate user input.</param>
    /// <param name="format">The format string that the date input should match.</param>
    /// <returns>The validation result and message.</returns>
    public static UserInputValidationResult IsValidReportStartDate(string input, string format)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new UserInputValidationResult(false, "Report start date cannot be empty.");
        }

        bool isCorrectDateFormat = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        if (!isCorrectDateFormat)
        {
            return new UserInputValidationResult(false, $"Report start date in wrong format. Format = {format}.");
        }
        
        return new UserInputValidationResult(true, "Validation successful");
    }

    /// <summary>
    /// Performs validation on an EndDate user input for a Report.
    /// </summary>
    /// <param name="input">The EndDate user input.</param>
    /// <param name="format">The format string that the date input should match.</param>
    /// <param name="startDate">The StartDate, as the EndDate cannot be before this date.</param>
    /// <returns>The validation result and message.</returns>
    public static UserInputValidationResult IsValidReportEndDate(string input, string format, DateTime startDate)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new UserInputValidationResult(false, "Report end date cannot be empty.");
        }

        bool isCorrectDateFormat = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate);
        if (!isCorrectDateFormat)
        {
            return new UserInputValidationResult(false, $"Report end date in wrong format. Format = {format}.");
        }

        // Strip off any time parts.
        startDate = startDate.Date;
        endDate = endDate.Date;

        if (endDate < startDate)
        {
            return new UserInputValidationResult(false, "End date cannot be before the start date.");
        }

        return new UserInputValidationResult(true, "Validation successful");
    }

    /// <summary>
    /// Performs validation on a StartDateTime user input for a CodingSession.
    /// </summary>
    /// <param name="input">The StartDateTime user input.</param>
    /// <param name="format">The format string that the date input should match.</param>
    /// <returns>The validation result and message.</returns>
    public static UserInputValidationResult IsValidCodingSessionStartDateTime(string input, string format)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new UserInputValidationResult(false, "Start time cannot be empty.");
        }

        bool isCorrectDateTimeFormat = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDateTime);
        if (!isCorrectDateTimeFormat)
        {
            return new UserInputValidationResult(false, $"Start time in wrong format. Format = {format}.");
        }
        else if (startDateTime > DateTime.Now)
        {
            return new UserInputValidationResult(false, "Start time cannot be in the future.");
        }

        return new UserInputValidationResult(true, "Validation successful");
    }

    /// <summary>
    /// Performs validation on an EndDateTime user input for a CodingSession.
    /// </summary>
    /// <param name="input">The EndDateTime user input.</param>
    /// <param name="format">The format string that the date input should match.</param>
    /// <param name="startDateTime">The StartDateTime, as the EndDateTime must be after this date.</param>
    /// <returns>The validation result and message.</returns>
    public static UserInputValidationResult IsValidCodingSessionEndDateTime(string input, string format, DateTime startDateTime)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new UserInputValidationResult(false, "End time cannot be empty.");
        }

        bool isCorrectDateTimeFormat = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDateTime);
        if (!isCorrectDateTimeFormat)
        {
            return new UserInputValidationResult(false, $"End time in wrong format. Format = {format}.");
        }
        else if (endDateTime > DateTime.Now)
        {
            return new UserInputValidationResult(false, "End time cannot be in the future.");
        }
        else if (endDateTime <= startDateTime)
        {
            return new UserInputValidationResult(false, "End time must be after the start time.");
        }

        return new UserInputValidationResult(true, "Validation successful");
    }

    #endregion
}
