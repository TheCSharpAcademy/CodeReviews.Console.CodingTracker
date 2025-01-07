using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

internal class Goals
{
    internal static void SetNewGoal()
    {
        Console.Clear();
        var totalHours = InputHelper.GetPositiveNumberInput(
            "Enter a goal of total coding session hours (ie '10000'):");
        var averageHours = InputHelper.GetPositiveNumberInput(
            "Enter daily average session time in hours (ie '5'):");

        var goalDate = DateTime.Today.AddDays(totalHours / averageHours);
        UpdateGoalData(totalHours, averageHours, DateTime.Today);

        AnsiConsole.MarkupLine($"You can reach your goal of total {totalHours} coding hours " +
            $"on date {goalDate.ToString(Config.DateFormat)} if you code daily for {averageHours} hours.");
        AnsiConsole.MarkupLine("Good luck!");
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    internal static (bool, int, int, DateTime) GetGoalDataFromDb()
    {
        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();
            var result = connection.QueryFirstOrDefault<Goal>(@"
                SELECT
                    total_hours AS TotalHours,
                    average_hours AS AverageHours,
                    set_goal_date AS SetGoalDate
                FROM goals");

            if (result != null && !result.HasDefaultValues())
            {
                return (true, result.TotalHours, result.AverageHours, result.SetGoalDate);
            }
            else return (false, 0, 0, DateTime.Today);
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]An error occurred.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
            return (false, 0, 0, DateTime.Today);
        }
    }

    internal static void ShowProgress()
    {
        var (hasData, totalHours, averageHours, setGoalDate) = GetGoalDataFromDb();
        if (!hasData) return;
        var durationReport = ReportCrud.GetDurationInfo(setGoalDate, DateTime.Now);
        if (!durationReport.HasData) return;

        int actualProgressInHours = (int)durationReport.TotalDuration.TotalHours;
        int plannedProgressInHours = averageHours * durationReport.TotalDays;

        var goalItems = new List<GoalBarItem>
        {
            new($"Your average {(int)Math.Round(durationReport.AverageDuration.TotalHours)} " +
                $"hours daily session", actualProgressInHours, Color.Yellow),
            new($"Planned {averageHours} hours a day:", plannedProgressInHours, Color.Red),
            new($"Your goal of total {totalHours} hours:", totalHours, Color.Green),
        };

        AnsiConsole.MarkupLine($"Compare your actual and planned progress from {setGoalDate.ToString(Config.DateFormat)}:");
        AnsiConsole.Write(new BarChart()
            .Width(80)
            .AddItems(goalItems));
        AnsiConsole.WriteLine();

        if (actualProgressInHours >= totalHours)
        {
            AnsiConsole.MarkupLine($"[seagreen1]Congratulations on reaching your total {totalHours} hours of coding time goal![/]");
            AnsiConsole.MarkupLine("[seagreen1]Set a new goal![/]\n");
        }
    }

    internal static void UpdateGoalData(int totalDuration, int averageDuration,
        DateTime goalDate)
    {
        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@id", 1);
            parameters.Add("@total_hours", totalDuration);
            parameters.Add("@average_hours", averageDuration);
            parameters.Add("@set_goal_date", goalDate);
            connection.Execute(@"
                UPDATE goals
                SET total_hours = @total_hours, average_hours = @average_hours, set_goal_date = @set_goal_date
                WHERE id = @id",
                parameters);
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to add a goal to database.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]An error occurred.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
    }
}
