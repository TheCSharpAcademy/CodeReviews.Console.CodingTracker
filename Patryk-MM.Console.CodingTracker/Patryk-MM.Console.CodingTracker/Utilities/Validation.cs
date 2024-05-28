using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patryk_MM.Console.CodingTracker.Utilities {
    public static class Validation {
        private static string _format = "dd.MM.yyyy HH:mm";

        public static bool ValidateDateInput(string dateInput, out DateTime parsedDate) {
            return DateTime.TryParseExact(dateInput, _format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
        }

        public static bool ValidateDateOrder(DateTime startDate, DateTime endDate) {
            return startDate < endDate;
        }
    }
}
