using Dapper;
using DbHelpers.HopelessCoding;
using Spectre.Console;
using System.Data.SQLite;
using ValidationChecks.HopelessCoding;

namespace CodingReports.HopelessCoding;

public class Reports
{
    public static void ReportsMenu()
    {
        Console.Clear();
        while (true)
        {
            AnsiConsole.Write(new Markup("[yellow1]REPORTS MENU\n\n[/]"));

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow1]What would you like to do?[/]")
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .HighlightStyle("olive")
                    .AddChoices(new[] {
                    "X - Report for specific time period", "0 - Exit to Main Menu"
                    }));

            switch (selection)
            {
                case "X - Report for specific time period":
                    TotalAndAverageReports();
                    break;
                case "0 - Exit to Main Menu":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("Invalid input, try again.");
                    Console.WriteLine("----------------------------");
                    break;
            }
        }
    }

    internal static void TotalAndAverageReports()
    {
        Console.Clear();
        string inputStartTime = Validations.ValidateTime("start");
        string inputEndTime = Validations.ValidateEndTimeNotBeforeStartTime(inputStartTime);
        string connectionString = DatabaseHelpers.connectionString;

        string viewQuery = @$"SELECT * 
                                    FROM coding_sessions 
                                    WHERE StartTime >= @StartTime AND EndTime <= @EndTime 
                                    ORDER BY StartTime DESC";

        var parameters = new { StartTime = inputStartTime, EndTime = inputEndTime };

        DatabaseHelpers.ListRecords(viewQuery, parameters, $"Records for period {inputStartTime} -> {inputEndTime}");

        string totalDuration = ReportDurations(connectionString, inputStartTime, inputEndTime, true);
        string averageDuration = ReportDurations(connectionString, inputStartTime, inputEndTime, false);

        PrintTotalAndAverage(totalDuration, averageDuration);

        Console.WriteLine("----------------------------");
    }

    internal static void PrintTotalAndAverage(string total, string average)
    {
        var totalPanel = GeneratePanel(total, "Total coding time for period");
        var averagePanel = GeneratePanel(average, "Average coding time for period");

        var averageTotalTable = new Table();
        averageTotalTable.Border = TableBorder.None;
        averageTotalTable.AddColumn(new TableColumn(totalPanel));
        averageTotalTable.AddColumn(new TableColumn(averagePanel));

        AnsiConsole.Write(averageTotalTable);
    }

    internal static Panel GeneratePanel(string content, string headerText)
    {
        var panel = new Panel(content).Padding(15, 0);
        panel.Border = BoxBorder.Double;
        panel.Header = new PanelHeader($"[khaki3]{headerText}[/]").Centered();
        return panel;
    }

    static string ReportDurations(string connectionString, string startTime, string endTime, bool isTotal)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {

            string calculationType = isTotal ? "SUM" : "AVG";

            string query = @$"SELECT 
                            printf('%02d:%02d', total_minutes / 60, total_minutes % 60) AS duration
                        FROM
                            (SELECT 
                                {calculationType}
                                    ((CAST(SUBSTR(duration, 1, 2) AS INTEGER) * 60) + CAST(SUBSTR(duration, 4, 2) AS INTEGER)) AS total_minutes
                            FROM 
                                coding_sessions
                            WHERE
                                StartTime >= @StartTime AND EndTime <= @EndTime) AS subquery";

            return connection.QueryFirstOrDefault<string>(query, new { StartTime = startTime, EndTime = endTime });
        }
    }
}