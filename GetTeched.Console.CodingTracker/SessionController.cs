using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;

namespace coding_tracker;

public class SessionController
{
    public UserInput UserInput { get; set; }

    CodingSession session = new();

    readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
    TableVisualisationEngine tableGeneration = new();
    static InputValidation validation = new();

    internal List<CodingSession> ViewAllSessions()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM Coding_Session";
            //string sqlQuery = "SELECT strftime('%d-%m-%Y', Date) AS FormattedDate, strftime('%H:%M:%S', StartTime) AS FormattedStartTime, strftime('%H:%M:%S', EndTime) AS FormattedEndTime, * FROM Coding_Session";
            var tableData = connection.Query<CodingSession>(sqlQuery).ToList();

            if (tableData.Any())
            {
                AnsiConsole.Clear();
                TableVisualisationEngine.ShowTable(tableData);
            }
        }
        return new List<CodingSession>();
    }

    internal void AddNewManualSession()
    {
        string startDateTime;
        string endDateTime;

        startDateTime = $"{GetDateInput("Start")} {GetTimeInput("Start")}";
        session.StartTime = InputValidation.DateTimeParse(startDateTime);
        endDateTime = $"{GetDateInput("End")} {GetTimeInput("End")}";
        session.EndTime = InputValidation.DateTimeParse(endDateTime);
        session.Date = InputValidation.DateTimeParse(session.StartTime, true);
        session.Duration = validation.Duration(session.StartTime, session.EndTime);

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO Coding_Session (Date, StartTime, EndTime, Duration) VALUES (@Date, @StartTime, @EndTime, @Duration)";
            connection.Execute(sqlQuery, new { session.Date, session.StartTime, session.EndTime, session.Duration });
        }
    }

    internal string GetTimeInput(string dateType)
    {
        string? userInput = "";
        bool validTime = false;

        while (!validTime)
        {
            userInput = AnsiConsole.Ask<string>($"Please enter the {dateType} time of your coding session. Format:[green]HH:MM:SS[/]");
            validTime = validation.TimeValidation(userInput);
        }
        return userInput;
    }

    internal string GetDateInput(string dateType)
    {
        string? userInput = "";
        bool validDate = false;

        while (!validDate)
        {
            userInput = AnsiConsole.Ask<string>($"Please enter the {dateType} date and time of your coding session. Format:[green]DD-MM-YY[/]");
            validDate = validation.DateValidation(userInput);
        }
        return userInput;
    }
    internal void SessionStopwatch()
    {
        if (!AnsiConsole.Confirm("Start stopwatch now?"))
        {
            AnsiConsole.MarkupLine("Returning to Menu");
            Thread.Sleep(1000);
            AnsiConsole.Clear();
            return;
        }
        AnsiConsole.Clear();

        Stopwatch stopwatch = new();
        stopwatch.Start();
        session.StartTime = validation.GetTimeStamp();
        session.Date = InputValidation.DateTimeParse(session.StartTime, true, false);
        tableGeneration.StopwatchTable();
        while (true)
        {

            TimeSpan timeSpan = TimeSpan.FromSeconds(Convert.ToInt32(stopwatch.Elapsed.TotalSeconds));
            Console.SetCursorPosition(30, 1);
            AnsiConsole.Markup($"[teal]{timeSpan.ToString("c")}[/]");
            Console.WriteLine("\r");
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.F) break;

        }
        session.EndTime = validation.GetTimeStamp();
        int timer = Convert.ToInt32(stopwatch.Elapsed.TotalSeconds);
        session.Duration = timer.ToString();

        AnsiConsole.Clear();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO Coding_Session (Date, StartTime, EndTime, Duration) VALUES (@Date, @StartTime, @EndTime, @Duration)";
            connection.Execute(sqlQuery, new { session.Date, session.StartTime, session.EndTime, session.Duration });
        }

    }

    internal string SecondsConversion(string secondsDuration)
    {
        int totalseconds = Convert.ToInt32(secondsDuration);
        int seconds = totalseconds % 60;
        int minutes = (totalseconds % 3600) / 60;
        int hours = totalseconds / 3600;
        string totalDuration = "";

        if (hours > 0)
        {
            return totalDuration = String.Format("{0:00}h {1:00}m {2:00}s", hours, minutes, seconds);
        }
        else if (minutes > 0)
        {
            return totalDuration = String.Format("00h {0:00}m {1:00}s", minutes, seconds);
        }
        else
        {
            return totalDuration = String.Format("00h 00m {0:00}s", seconds);
        }
    }

    internal int[] GetIds()
    {
        List<int> rowData = new();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "SELECT * FROM Coding_Session";
            var codingSessions = connection.Query(sqlQuery);

            foreach (var codingSession in codingSessions)
            {
                rowData.Add(Convert.ToInt32(codingSession.Id));
            }
        }
        return rowData.ToArray();


    }

    internal void UpdateSession()
    {

        int[] sessionIds = GetIds();
        bool entryValid = false;
        int idSelection = 0;
        string startDateTime;
        string endDateTime;

        while (!entryValid)
        {
            ViewAllSessions();
            idSelection = AnsiConsole.Ask<int>("Please type the Id number you would like to edit.");
            AnsiConsole.Clear();
            entryValid = validation.SessionIdInRange(sessionIds, idSelection);
        }

        session.Id = idSelection;
        startDateTime = $"{GetDateInput("Start")} {GetTimeInput("Start")}";
        session.StartTime = InputValidation.DateTimeParse(startDateTime);
        endDateTime = $"{GetDateInput("End")} {GetTimeInput("End")}";
        session.EndTime = InputValidation.DateTimeParse(endDateTime);
        session.Date = InputValidation.DateTimeParse(session.StartTime, true, false);
        session.Duration = validation.Duration(session.StartTime, session.EndTime);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery =
                @"UPDATE Coding_Session SET
                Date = @Date,
                StartTime = @StartTime,
                EndTime = @EndTime,
                Duration = @Duration
                Where Id = @Id";

            connection.Execute(sqlQuery, new { session.Id, session.Date, session.StartTime, session.EndTime, session.Duration });
        }
    }

    internal void DeleteSession()
    {

        int[] sessionIds = GetIds();
        bool entryValid = false;
        int idSelection = 0;

        while (!entryValid)
        {
            ViewAllSessions();
            idSelection = AnsiConsole.Ask<int>("Please type the Id number you would like to delete.");
            AnsiConsole.Clear();
            entryValid = validation.SessionIdInRange(sessionIds, idSelection);
        }

        session.Id = idSelection;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = @"DELETE FROM Coding_Session WHERE Id = @Id";

            connection.Execute(sqlQuery, new { session.Id });
        }
    }

    internal void WeeklyCodingSessions()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = @"SELECT strftime('%W', StartTime) AS WeekNumber, SUM(Duration) AS Duration From Coding_Session GROUP BY strftime('%W', StartTime)";
            var codingSessions = connection.Query(sqlQuery);

            List<string> rowData = new();

            foreach (var codingSession in codingSessions)
            {
                string parsedTime = SecondsConversion(Convert.ToString(codingSession.Duration));
                rowData.Add(codingSession.WeekNumber);
                rowData.Add(parsedTime);

            }


            tableGeneration.WeekGenerator(rowData);
        }
    }

    internal void GetYearToDateRange()
    {
        string sortType; string sortRange;
        string[] sorting = new string[2];
        DateTime inputDate = DateTime.Now;
        string endDate = InputValidation.DateTimeParse(inputDate.ToString());
        string startDate = InputValidation.DateTimeParse(inputDate.AddDays(-365).ToString());
        var reportData = DateRangeReport(startDate, endDate);
        while(true)
        {
            TableVisualisationEngine.ReportDisplay(reportData);
            sorting = UserInput.Sorting();  
            sortRange = sorting[0];
            sortType = sorting[1];
            if (sortRange == null || sortType == null) break;

            reportData = DateRangeReport(startDate, endDate, sortRange, sortType);
        }
        

    }

    internal void GetBiweeklyRange()
    {
        string sortType; string sortRange;
        string[] sorting = new string[2];
        DateTime inputDate = DateTime.Now;
        string endDate = InputValidation.DateTimeParse(inputDate.AddDays(6).ToString());
        string startDate = InputValidation.DateTimeParse(inputDate.AddDays(-14).ToString());
        var reportData = DateRangeReport(startDate, endDate);

        while (true)
        {
            TableVisualisationEngine.ReportDisplay(reportData);
            sorting = UserInput.Sorting();
            sortRange = sorting[0];
            sortType = sorting[1];
            if (sortRange == null || sortType == null) break;

            reportData = DateRangeReport(startDate, endDate, sortRange, sortType);
        }
    }

    internal void GetDateRange()
    {
        string sortType; string sortRange;
        string[] sorting = new string[2];
        string startDate = InputValidation.DateTimeParse(GetDateInput("Start"), true);
        string endDate = InputValidation.DateTimeParse(GetDateInput("End"), true);
        var reportData = DateRangeReport(startDate, endDate);

        while (true)
        {
            TableVisualisationEngine.ReportDisplay(reportData);
            sorting = UserInput.Sorting();
            sortRange = sorting[0];
            sortType = sorting[1];
            if (sortRange == null || sortType == null) break;

            reportData = DateRangeReport(startDate, endDate, sortRange, sortType);
        }
    }

    internal List<CodingSession> DateRangeReport(string startDate, string endDate, string sortRange = "Date", string sortType = "ASC")
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = @$"SELECT * FROM Coding_Session WHERE Date BETWEEN @StartDate AND @EndDate ORDER BY {sortRange} {sortType}";
            return connection.Query<CodingSession>(sqlQuery, new { StartDate = startDate, EndDate = endDate }).ToList();
        }
    }



}