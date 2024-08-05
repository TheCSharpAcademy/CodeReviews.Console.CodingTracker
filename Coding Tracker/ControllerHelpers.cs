using System.Globalization;
using Spectre.Console;

namespace CodingTracker;

public class Helpers
{
    internal static string ParseTimeWithSeconds(string timeString)
    {
        DateTime parsedTime = DateTime.ParseExact(timeString, "h:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);

        try
        {
            // Check if the parsed time includes seconds
            if (!timeString.Contains(':'))
            {
                // Append ":00" seconds if they are not included in the input
                parsedTime = parsedTime.AddSeconds(0);
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Views.ShowError(ex.Message);
        }

        return parsedTime.ToString("h:mm:ss tt", new CultureInfo("en-US"));
    }

    internal static string CalculateDuration(string start, string end)
    {
        TimeSpan duration = TimeSpan.Zero;

        try
        {
            var startTime = DateTime.Parse(start);
            var endTime = DateTime.Parse(end);
            duration = endTime - startTime;
        }
        catch (Exception ex)
        {
            Views.ShowError(ex.Message);
        }

        return duration.ToString();
    }
    
    internal static bool CheckTime(string column, string newTime, string unchangedTime)
    {
        bool isValid = false;

        if (column == "StartTime")
        {
            DateTime startTime = DateTime.Parse(newTime);
            DateTime endTime = DateTime.Parse(unchangedTime);

            isValid = (startTime >= endTime) ? false : true;
        }
        else if (column == "EndTime")
        {
            DateTime startTime = DateTime.Parse(unchangedTime);
            DateTime endTime = DateTime.Parse(newTime);

            isValid = (endTime <= startTime) ? false : true;
        }

        return isValid;
    }

    // Recalculates duration when udpating session start or end times    
    internal static string RecalculateDuration(string column, string newTime, string unchangedTime)
    {
        DateTime startTime;
        DateTime endTime;

        TimeSpan duration = TimeSpan.Zero;

        try
        {
            if (column == "StartTime")
            {
                startTime = DateTime.Parse(newTime);
                endTime = DateTime.Parse(unchangedTime);
            }
            else
            {
                startTime = DateTime.Parse(unchangedTime);
                endTime = DateTime.Parse(newTime);
            }
            duration = endTime - startTime;
        }
        catch (Exception ex)
        {
            Views.ShowError(ex.Message);
        }

        return duration.ToString();
    }

    internal static void CalculateGoalStatus(List<CodingGoal> goals)
    {
        foreach (var goal in goals)
        {
            TimeSpan sum = TimeSpan.Zero;
            TimeSpan totalHours;
            double percentage;

            List<CodingSession> filteredList = GoalsModel.GetFilteredLanguageSessions(goal);

            foreach (var session in filteredList)
            {
                sum += session.Duration;
            }

            totalHours = TimeSpan.FromHours(goal.TotalHours);
            percentage = sum / totalHours * 100;

            TimeSpan hoursLeft = totalHours - sum;

            string convertedPercentage = percentage.ToString("0.#");
            string convertedHoursLeft = hoursLeft.TotalHours.ToString("0.##");

            GoalsModel.UpdateGoalPercentage(goal, convertedPercentage, convertedHoursLeft);
        }
    }

    internal static double CalculatePercentage(TimeSpan languageCodingTime, TimeSpan totalCodingTime)
    {
        double percentage;

        percentage = languageCodingTime / totalCodingTime * 100;

        return percentage;
    }

    internal static TimeSpan SumTimes(List<CodingSession> sessionList)
    {
        TimeSpan languageCodingTime = TimeSpan.Zero;

        foreach (var session in sessionList)
        {
            languageCodingTime += session.Duration;
        }

        return languageCodingTime;
    }

    internal static DateTime StartOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    internal static Color SelectLanguageColor(CodingSession language)
    {
        string? convertedLanguage = Convert.ToString(language.Language);
        Color setColor = Color.Red;

        switch (convertedLanguage)
        {
            case "C":
                setColor = Color.Red;
                break;
            case "C+":
                setColor = Color.DeepPink3;
                break;
            case "C#":
                setColor = Color.DarkViolet_1;
                break;
            case "Python":
                setColor = Color.Blue;
                break;
            case "Java":
                setColor = Color.LightGoldenrod1;
                break;
            case "JavaScript":
                setColor = Color.LightSteelBlue;
                break;
            case "SQL":
                setColor = Color.Aqua;
                break;
            case "Go":
                setColor = Color.Green3_1;
                break;
            case "Rust":
                setColor = Color.OrangeRed1;
                break;
            default:
                break;
        }

        return setColor;
    }
}

