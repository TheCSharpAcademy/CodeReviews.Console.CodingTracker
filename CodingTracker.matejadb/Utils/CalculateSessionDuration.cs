using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.matejadb.Utils; 
public static class CalculateSessionDuration {
    public static string SessionDuration(string startTime, string endTime) {
        TimeSpan duration = DateTime.Parse(endTime) - DateTime.Parse(startTime);

        var durationString = $"{duration.TotalMinutes} minutes";

        return durationString;
    }
}
