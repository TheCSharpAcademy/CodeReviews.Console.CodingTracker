using Patryk_MM.Console.CodingTracker.Models;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    public static class Validation {
        private static string _format = "dd.MM.yyyy HH:mm:ss";

        public static bool ValidateDateInput(string dateInput, out DateTime parsedDate) {
            return DateTime.TryParseExact(dateInput, _format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
        }

        public static bool ValidateDateOrder(DateTime startDate, DateTime endDate) {
            return endDate > startDate;
        }

        public static bool ValidateFutureDate(DateTime date) {
            return date > DateTime.Now;
        }

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
