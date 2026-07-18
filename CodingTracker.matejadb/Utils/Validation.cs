using CodingTracker.matejadb.Config;
using Spectre.Console;
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

    public static bool ValidateEndDate(string startDate, string endDate) {
        DateTime startDateParsed = DateTime.ParseExact(startDate, AppSettings.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
        DateTime endDateParsed = DateTime.ParseExact(endDate, AppSettings.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);

        if (endDateParsed < startDateParsed) return false;

        return true;
    }
}
