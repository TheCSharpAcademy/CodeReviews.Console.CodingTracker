using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

internal class ReportCrud
{
    internal static (bool, DateTime) GetMinDateFromDb()
    {
        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();
            var result = connection.QueryFirstOrDefault<string>(@"
                SELECT MIN(start_time) AS start_time
                FROM sessions");

            if (result != null)
            {
                if (DateTime.TryParseExact(result, Config.DateParsingFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime minDate))
                {
                    return (true, minDate.Date);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Failed to parse date.[/]");
                    DisplayInfoHelpers.PressAnyKeyToContinue();
                    return (false, DateTime.MinValue);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No data found.[/]");
                DisplayInfoHelpers.PressAnyKeyToContinue();
                return (false, DateTime.MinValue);
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]An error occurred.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
            return (false, DateTime.MinValue);
        }
    }

    internal static DurationInfo GetDurationInfo(DateTime startDate, DateTime endDate)
    {
        var durationInfo = new DurationInfo();

        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@start_time", startDate);
            parameters.Add("@end_time", endDate);
            var results = connection.Query<TimeSpan>(@"
                SELECT duration
                FROM sessions
                WHERE DATETIME(start_time) BETWEEN @start_time AND @end_time",
                parameters);

            if (results.Any())
            {
                var totalDuration = new TimeSpan();
                foreach (var result in results)
                {
                    totalDuration += result;
                }
                var totalDays = (endDate - startDate + TimeSpan.FromDays(1)).Days;
                var averageDuration = totalDuration / totalDays;

                durationInfo.HasData = true;
                durationInfo.TotalDuration = totalDuration;
                durationInfo.AverageDuration = averageDuration;
                durationInfo.TotalDays = totalDays;
                return durationInfo;
            }
            else
            {
                return durationInfo;
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]An error occurred while reading from db![/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
            return durationInfo;
        }
    }
}
