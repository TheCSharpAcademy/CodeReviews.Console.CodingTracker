using CodingTracker.Arashi256.Models;

namespace CodingTracker.Arashi256.Classes
{
    internal class Utility
    {
        public static string CalculateDuration(DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan duration = endDateTime - startDateTime;
            string formattedDuration = $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
            return formattedDuration;
        }

        public static double CalculateGoalHours(DateTime startDateTime, DateTime endDateTime, int desiredHours)
        {
            int totalDays = (endDateTime - startDateTime).Days + 1;
            double hoursPerDay = desiredHours / totalDays;
            return hoursPerDay;
        }

        public static double ConvertTimeStringToHours(string timeString)
        {
            var timeParts = timeString.Split(':');
            if (timeParts.Length != 3)
            {
                throw new ArgumentException("Time string must be in the format hh:mm:ss");
            }
            int hours = int.Parse(timeParts[0]);
            int minutes = int.Parse(timeParts[1]);
            int seconds = int.Parse(timeParts[2]);
            double totalHours = hours + (minutes / 60.0) + (seconds / 3600.0);
            return totalHours;
        }

        public static string ConvertHoursToTimeString(double totalHours)
        {
            int hours = (int)totalHours;
            double fractionalHours = totalHours - hours;
            int minutes = (int)(fractionalHours * 60);
            double fractionalMinutes = fractionalHours * 60 - minutes;
            int seconds = (int)(fractionalMinutes * 60);
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        public static double CalculateRemainingHoursPerDay(DateTime start, DateTime deadline, double targetHours, string completedTimeString)
        {
            TimeSpan completedTime;
            if (!TimeSpan.TryParse(completedTimeString, out completedTime))
            {
                return -1;
            }
            DateTime now = DateTime.Now;
            if (deadline <= now || completedTime.TotalHours > targetHours)
            {
                return -1;
            }
            double remainingHours = targetHours - completedTime.TotalHours;
            double totalDaysRemaining = (deadline - now).TotalDays;
            double hoursPerDay = remainingHours / totalDaysRemaining;
            return Math.Round(hoursPerDay, 2);
        }

        public static string SumCodingSessions(CodingSession[] codingSessions)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (CodingSession codingSession in codingSessions)
            {
                TimeSpan duration = ParseTimeSpan(codingSession.Duration);
                totalDuration += duration;
            }
            string formattedTotalDuration = $"{(int)totalDuration.TotalHours:D2}:{totalDuration.Minutes:D2}:{totalDuration.Seconds:D2}";
            return formattedTotalDuration;
        }

        private static TimeSpan ParseTimeSpan(string durationString)
        {
            var parts = durationString.Split(':');
            if (parts.Length != 3)
            {
                throw new FormatException("Utility.ParseTimeSpan(): Invalid duration format.");
            }
            if (!int.TryParse(parts[0], out int hours))
            {
                throw new FormatException("Utility.ParseTimeSpan(): Invalid hours format.");
            }
            if (!int.TryParse(parts[1], out int minutes))
            {
                throw new FormatException("Utility.ParseTimeSpan(): Invalid minutes format.");
            }
            if (!int.TryParse(parts[2], out int seconds))
            {
                throw new FormatException("Invalid seconds format.");
            }
            return new TimeSpan(hours / 24, hours % 24, minutes, seconds);
        }

        public static string AverageCodingSessions(CodingSession[] sessions)
        {
            if (sessions == null || sessions.Length == 0) return "00:00:00";
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (CodingSession codingSession in sessions)
            {
                TimeSpan duration = ParseTimeSpan(codingSession.Duration);
                totalDuration += duration;
            }
            double averageTicks = totalDuration.Ticks / (double)sessions.Length;
            TimeSpan averageTimeSpan = TimeSpan.FromTicks((long)averageTicks);
            return $"{(int)averageTimeSpan.TotalHours:D2}:{averageTimeSpan.Minutes:D2}:{averageTimeSpan.Seconds:D2}";
        }

        public static string TranslateSortOrderToString(SortOrder sortOrder)
        {
            return sortOrder == SortOrder.ASC ? "ASC" : "DESC";
        }

        public static bool GetValidCodingHoursGoal(DateTime start, DateTime? end, int hours)
        {
            if (end == null) return false;
            int totalDays = (end.Value - start).Days + 1;
            int maxHours = totalDays * 24;
            return hours <= maxHours;
        }

        public static double CalculateRequiredCodingHoursPerDay(DateTime start, DateTime end, double projectedHours)
        {
            int totalDays = (end - start).Days + 1;
            double hoursPerDay = projectedHours / totalDays;
            return hoursPerDay;
        }
        public static double RoundToZero(double value)
        {
            return (value < 0) ? 0 : value;
        }

        public static string ColorizeProgressBar(string progressBar)
        {
            int totalLength = progressBar.Length;
            int sectionLength = totalLength / 3;
            string colorizedProgressBar = Colorize(progressBar.Substring(0, sectionLength), "red") +
                Colorize(progressBar.Substring(sectionLength, sectionLength), "yellow") +
                Colorize(progressBar.Substring(2 * sectionLength), "lime");
            return $"{colorizedProgressBar}";
        }

        private static string Colorize(string section, string color)
        {
            return $"[{color}]{section}[/]";
        }

        public enum SortOrder
        {
            ASC, DESC
        }
    }
}