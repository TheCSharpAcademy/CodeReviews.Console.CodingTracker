using CodingTracker.Models;
using System.Globalization;

namespace CodingTracker.Services;

internal class Helpers
{
    public static string? GetDate(bool formatted)
    {
        return DateTimeOffset.Now.ToString(formatted ? "MM/dd/yyyy" : "MM/dd/yyyy hh:mm:ss");
    }

    public static string? GetTime()
    {
        return DateTimeOffset.Now.ToString(@"hh\:mm\:ss");
    }

    public static string? CalculateDuration(string? startTime, string? endTime)
    {
        TimeSpan start = TimeSpan.Parse(startTime);
        TimeSpan end = TimeSpan.Parse(endTime);

        string? difference = end.Subtract(start).ToString();
        return difference;
    }

    public static void PrintTimeSheet()
    {
        List<string?>? times = new()
        {
            "01:00 -> 1 AM    |   13:00 -> 1 PM",
            "02:00 -> 2 AM    |   14:00 -> 2 PM",
            "03:00 -> 3 AM    |   15:00 -> 3 PM",
            "04:00 -> 4 AM    |   16:00 -> 4 PM",
            "05:00 -> 5 AM    |   17:00 -> 5 PM",
            "06:00 -> 6 AM    |   18:00 -> 6 PM",
            "07:00 -> 7 AM    |   19:00 -> 7 PM",
            "08:00 -> 8 AM    |   20:00 -> 8 PM",
            "09:00 -> 9 AM    |   21:00 -> 9 PM",
            "10:00 -> 10 AM   |   22:00 -> 10 PM",
            "11:00 -> 11 AM   |   23:00 -> 11 PM",
            "12:00 -> 12 AM   |   00:00 -> 12 AM"
        };

        foreach (string? time in times)
        {
            Console.WriteLine(time);
        }
    }

    public static void Finished(CodeSession? session, Goal? goal, string? action)
    {
        if (session == null)
        {
            Console.WriteLine(" ------------------------ ");
            Console.WriteLine($"Goal Name: {goal.Name}");
            Console.WriteLine($"Start Date: {goal.StartDate}");
            Console.WriteLine($"End Date: {goal.EndDate}");
            Console.WriteLine($"Coding Hours Per Day: {goal.HoursPerDay}");
            Console.WriteLine($"Days Until Goal: {goal.DaysToGoal}");
            Console.WriteLine($"Achieved: {goal.Achieved}");
            Console.WriteLine(" ------------------------ ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Successfully {action} Session Data.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press Enter To Go To Main Menu");
            Console.ReadLine();
            Console.Clear();
            MainMenu.ShowMenu();
        }
        else
        {
            Console.WriteLine("------------------------ ");
            Console.WriteLine($"Start Date: {session.TodaysDate}");
            Console.WriteLine($"Start Time: {session.StartTime}");
            Console.WriteLine($"End Time: {session.EndTime}");
            Console.WriteLine($"Duration: {session.Duration}");
            Console.WriteLine("------------------------ ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Successfully {action} Session Data.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press Enter To Go To Main Menu");
            Console.ReadLine();
            Console.Clear();
            MainMenu.ShowMenu();
        }
    }

    public static int? DaysToGoal(string? startDate, string? endDate)
    {
        DateTimeOffset start = DateTimeOffset.Parse(startDate);
        DateTimeOffset end = DateTimeOffset.Parse(endDate);

        TimeSpan difference = end.Subtract(start);

        int? daysToGoal = Convert.ToInt32(difference.TotalDays);
        return daysToGoal;
    }

    #region CodingSession Average/Total Time
    public static double CalculateAverageSession(List<CodeSession?>? allSessions)
    {
        double totalSeconds = GetTotalSessionTime(allSessions);
        double averageTime = totalSeconds / allSessions.Count;
        return averageTime;
    }

    public static double GetTotalSessionTime(List<CodeSession?>? allSessions)
    {
        double totalSeconds = 0.00f;

        string format = @"hh\:mm\:ss";
        CultureInfo provider = CultureInfo.InvariantCulture;

        foreach (CodeSession session in allSessions)
        {
            TimeSpan start = TimeSpan.ParseExact(session.StartTime, format, provider);
            TimeSpan end = TimeSpan.ParseExact(session.EndTime, format, provider);

            TimeSpan difference = end.Subtract(start);

            double timeInSeconds = difference.TotalSeconds;

            totalSeconds = totalSeconds + timeInSeconds;
        }

        return totalSeconds;
    }
    #endregion

    #region GoalTotalTime
    public static int GetTotalGoalDays(List<Goal?>? allGoals)
    {
        int totalDays = 0;
        string format = "MM/dd/yyyy";
        CultureInfo provider = CultureInfo.InvariantCulture;

        foreach (Goal goal in allGoals)
        {
            DateTimeOffset start = DateTimeOffset.ParseExact(goal.StartDate, format, provider);
            DateTimeOffset end = DateTimeOffset.ParseExact(goal.EndDate, format, provider);
            int difference = Convert.ToInt32(end - start);
        }

        return totalDays;
    }

    public static int GetTotalGoalHours(List<Goal?>? allGoals)
    {
        int totalHours = 0;

        foreach (Goal goal in allGoals)
        {
            totalHours = (int)(totalHours + goal.HoursPerDay);
        }

        return totalHours;
    }

    public static double GetTotalDaysUntilAllGoalsMet(List<Goal?>? allGoals)
    {
        double totaldays = 0.00f;

        foreach (Goal goal in allGoals)
        {
            totaldays = (int)(totaldays + goal.DaysToGoal);
        }

        return totaldays;
    }
    #endregion
}
