using CodingTracker.matejadb.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CodingTracker.matejadb.Utils; 
public static class Validation {
    public static bool ValidateDateTime(string dateTime) {
        DateTime result;

        return DateTime.TryParseExact(dateTime, AppSettings.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }
}
