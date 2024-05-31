using Patryk_MM.Console.CodingTracker.Models;
using System;
using System.Collections.Generic;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    /// <summary>
    /// Provides utility methods for data validation.
    /// </summary>
    public static class Validation {
        private static string _format = "dd.MM.yyyy HH:mm:ss";

        /// <summary>
        /// Validates the input string as a date in the specified format.
        /// </summary>
        /// <param name="dateInput">The input string to validate as a date.</param>
        /// <param name="parsedDate">The parsed DateTime value if the validation is successful.</param>
        /// <returns>True if the input string is successfully parsed as a date, otherwise false.</returns>
        public static bool ValidateDateInput(string dateInput, out DateTime parsedDate) {
            return DateTime.TryParseExact(dateInput, _format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
        }

        /// <summary>
        /// Validates that the start date is before the end date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>True if the start date is before the end date, otherwise false.</returns>
        public static bool ValidateDateOrder(DateTime startDate, DateTime endDate) {
            return endDate > startDate;
        }

        /// <summary>
        /// Validates that a given date is not in the future.
        /// </summary>
        /// <param name="date">The date to validate.</param>
        /// <returns>True if the date is not in the future, otherwise false.</returns>
        public static bool ValidateFutureDate(DateTime date) {
            return date > DateTime.Now;
        }

        /// <summary>
        /// Validates if there is an overlap between a new session and existing sessions.
        /// </summary>
        /// <param name="existingSessions">The list of existing coding sessions.</param>
        /// <param name="newSession">The new coding session to validate for overlap.</param>
        /// <returns>True if there is an overlap, otherwise false.</returns>
        public static bool ValidateSessionOverlap(List<CodingSession> existingSessions, CodingSession newSession) {
            foreach (CodingSession existingSession in existingSessions) {
                // Skip checking overlap with the same session
                if (existingSession.Id == newSession.Id) {
                    continue;
                }

                // Check if the start time of the new session is between the start and end times of the existing session
                if (newSession.StartDate >= existingSession.StartDate && newSession.StartDate <= existingSession.EndDate) {
                    return true;
                }

                // Check if the end time of the new session is between the start and end times of the existing session
                if (newSession.EndDate >= existingSession.StartDate && newSession.EndDate <= existingSession.EndDate) {
                    return true;
                }

                // Check if the new session completely contains the existing session
                if (newSession.StartDate <= existingSession.StartDate && newSession.EndDate >= existingSession.EndDate) {
                    return true;
                }
            }

            return false;
        }
    }
}
