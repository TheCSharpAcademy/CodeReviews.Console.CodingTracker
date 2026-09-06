using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CodingTracker.Model
{
    internal class CodingSession
    {
        public int? Id { get; set; } // NULL before Sql autoincrement
        public DateTime StartTime {  get; set; } 
        public DateTime EndTime {  get; set; }
        public TimeSpan Duration => EndTime - StartTime;

        // String Properties
        public string StartTimeString { get; set; } = string.Empty;
        public string EndTimeString { get; set; } = string.Empty;
        public string DurationString => Duration.TotalHours >= 24
            ? $"{(int)Duration.TotalHours}:{Duration:mm\\:ss}"
            : Duration.ToString(@"hh\:mm\:ss");

        public CodingSession() { } //without parameter

        public CodingSession(DateTime startDate, DateTime endDate, int? id = null)
        {
            Id = id;
            StartTime = startDate;
            EndTime = endDate;
            StartTimeString = startDate.ToString("dd-MM-yyyy HH:mm:ss");
            EndTimeString = endDate.ToString("dd-MM-yyyy HH:mm:ss");
        }
        public CodingSession(string startDate, string endDate, int? id = null)
        {
            Id = id;
            StartTimeString = startDate;
            EndTimeString = endDate;
            StartTime = DateTime.ParseExact(Regex.Replace(StartTimeString?.Trim() ?? "", (@"\s+"), (" ")).Replace(".", "-"), 
                                    "dd-MM-yyyy HH:mm:ss",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None);
            EndTime = DateTime.ParseExact(Regex.Replace(EndTimeString?.Trim() ?? "", (@"\s+"), (" ")).Replace(".", "-"), 
                                    "dd-MM-yyyy HH:mm:ss",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None);
        }

    }

    
}
