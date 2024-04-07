using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace coding_tracker;

public class SessionController
{
    public UserInput UserInput { get; set; }

    CodingSession session = new();

    readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
    TableVisualisationEngine tableGeneration = new();
    static InputValidation validation = new();

    internal List<CodingSession> ViewAllSessions(bool showCaption = true)
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM Coding_Session";
                var tableData = connection.Query<CodingSession>(sqlQuery).ToList();

                if (tableData.Any())
                {
                    AnsiConsole.Clear();
                    TableVisualisationEngine.ShowTable(tableData, showCaption);
                }
                else
                {
                    AnsiConsole.Write(new Markup("[red]\nNo Data Found, Press any key to return to Main Menu[/]"));
                    Console.ReadLine();
                    UserInput.MainMenu();
                }
            }
            return new List<CodingSession>();
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A Database occurred: {ex.Message}[/]"));
            return new List<CodingSession>();
        }
    }

    internal void AddNewManualSession()
    {
        try
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
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }
    }

    internal string GetTimeInput(string dateType)
    {
        string? userInput = "";
        bool validTime = false;

        while (!validTime)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Enter Time").Color(Color.Teal));
            userInput = AnsiConsole.Ask<string>($"Please enter the {dateType} time of your coding session. Format:[green]HH:MM:SS[/]\n[red]Type F to return to Main menu[/]");
            if (userInput.ToLower() == "f") UserInput.MainMenu();
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
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Enter Date").Color(Color.Teal));
            userInput = AnsiConsole.Ask<string>($"Please enter the {dateType} date of your coding session. Format:[green]DD-MM-YY[/]\n[red]Type F to return to Main menu[/]");
            if (userInput.ToLower() == "f") UserInput.MainMenu();
            validDate = validation.DateValidation(userInput);
        }
        return userInput;
    }
    internal void SessionStopwatch()
    {
        try
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Stopwatch Tracker").Color(Color.Teal));
            if (!AnsiConsole.Confirm("Start stopwatch now?"))
            {
                AnsiConsole.MarkupLine("Returning to Menu");
                Thread.Sleep(600);
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
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }

    }

    internal int[] GetIds()
    {
        try
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
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
            return new int[] { 0 };
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
            return new int[] { 0 };
        }
    }

    internal void UpdateSession()
    {
        try
        {
            int[] sessionIds = GetIds();
            bool entryValid = false;
            int idSelection = 0;
            string startDateTime;
            string endDateTime;

            while (!entryValid)
            {
                ViewAllSessions(false);
                idSelection = AnsiConsole.Ask<int>("Please type the Id number you would like to edit.\n[red]Enter 0 to return to Main Menu[/]");
                if (idSelection == 0) UserInput.MainMenu();
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
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }
    }

    internal void DeleteSession()
    {
        try
        {
            int[] sessionIds = GetIds();
            bool entryValid = false;
            int idSelection = 0;

            while (!entryValid)
            {
                ViewAllSessions(false);
                idSelection = AnsiConsole.Ask<int>("Please type the Id number you would like to delete.\n[red]Enter 0 to return to Main Menu[/]");
                if (idSelection == 0) UserInput.MainMenu();
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
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }
    }

    internal void WeeklyCodingSessions()
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = @"SELECT strftime('%W', StartTime) AS WeekNumber, SUM(Duration) AS Duration From Coding_Session GROUP BY strftime('%W', StartTime)";
                var codingSessions = connection.Query(sqlQuery);
                List<string> rowData = new();

                foreach (var codingSession in codingSessions)
                {
                    string parsedTime = InputValidation.SecondsConversion(Convert.ToString(codingSession.Duration));
                    rowData.Add(codingSession.WeekNumber);
                    rowData.Add(parsedTime);
                }
                tableGeneration.WeekGenerator(rowData);
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }
    }

    internal void GetYearToDateRange()
    {
        string sortType; string sortRange;
        string[] sorting = new string[2];
        DateTime inputDate = DateTime.Now;
        string endDate = InputValidation.DateTimeParse(inputDate.ToString(), true);
        string startDate = InputValidation.DateTimeParse(inputDate.AddDays(-365).ToString(), true);
        var reportData = DateRangeReport(startDate, endDate);

        while (true)
        {
            AnsiConsole.Clear();
            TableVisualisationEngine.ReportDisplay(reportData);
            tableGeneration.TotalTable(DurationRangeReport(startDate, endDate));
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
        string endDate = InputValidation.DateTimeParse(inputDate.AddDays(6).ToString(), true);
        string startDate = InputValidation.DateTimeParse(inputDate.AddDays(-14).ToString(), true);
        var reportData = DateRangeReport(startDate, endDate);

        while (true)
        {
            AnsiConsole.Clear();
            TableVisualisationEngine.ReportDisplay(reportData);
            tableGeneration.TotalTable(DurationRangeReport(startDate, endDate));
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
            AnsiConsole.Clear();
            TableVisualisationEngine.ReportDisplay(reportData);
            tableGeneration.TotalTable(DurationRangeReport(startDate, endDate));
            sorting = UserInput.Sorting();
            sortRange = sorting[0];
            sortType = sorting[1];
            if (sortRange == null || sortType == null) break;
            reportData = DateRangeReport(startDate, endDate, sortRange, sortType);
        }
    }

    internal List<CodingSession> DateRangeReport(string startDate, string endDate, string sortRange = "Date", string sortType = "ASC")
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = @$"SELECT * FROM Coding_Session WHERE Date BETWEEN @StartDate AND @EndDate ORDER BY {sortRange} {sortType}";
                return connection.Query<CodingSession>(sqlQuery, new { StartDate = startDate, EndDate = endDate }).ToList();
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
            return new List<CodingSession>();
        }
    }

    internal string[] DurationRangeReport(string startDate, string endDate)
    {
        try
        {
            string[] durationCalculations = new string[2];
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = @"SELECT Duration FROM Coding_Session WHERE Date BETWEEN @StartDate AND @EndDate";
                var duration = connection.Query<int>(sqlQuery, new { StartDate = startDate, EndDate = endDate }).ToList();

                if (duration.Any())
                {
                    int totalTime = 0;
                    double averageTime;

                    foreach (int row in duration)
                    {
                        totalTime += row;
                    }

                    averageTime = duration.Average();
                    durationCalculations[0] = InputValidation.SecondsConversion(totalTime.ToString());
                    durationCalculations[1] = InputValidation.SecondsConversion(Convert.ToInt32(averageTime).ToString());
                }
                else
                {
                    AnsiConsole.Write(new Markup("[red]No Data Found, Press any key to return to Main Menu[/]"));
                    Console.ReadLine();
                    UserInput.MainMenu();
                    return new string[0];
                }
                return durationCalculations;
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
            return new string[0];
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
            return new string[0];
        }
    }

    internal bool CodingGoalCreated()
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM Coding_Goal";
                var codingGoal = connection.Query<CurrentCodingGoal>(sqlQuery).ToList();
                if (codingGoal.Count == 0) return true;
            }
            return false;
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
            return false;
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
            return false;
        }
    }

    internal void CodingGoalInsert()
    {
        try
        {
            string goalDate = GetDateInput("Goal End");
            DateTime today = DateTime.Now;
            string dateStamp = today.ToString("yyyy-MM-dd");
            int hours = AnsiConsole.Ask<int>($"Please enter the amount of hours you want to achieve by the end of your goal");
            int completed = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = @"INSERT INTO Coding_Goal (Date, Hours, Completed, Datestamp) VALUES (@Date, @Hours, @Completed, @Datestamp)";
                connection.Query<CurrentCodingGoal>(sqlQuery, new { Date = goalDate, Hours = hours, Completed = completed, Datestamp = dateStamp });
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }
    }

    internal void CodingGoalReview()
    {

        try
        {
            string goalDate;
            string dateStamp;
            int goalHours;
            int totalHours;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlGoal = "SELECT Date FROM Coding_Goal LIMIT 1";
                string sqlStamp = "SELECT Datestamp FROM Coding_Goal LIMIT 1";
                goalDate = connection.QueryFirstOrDefault<string>(sqlGoal);
                dateStamp = connection.QueryFirstOrDefault<string>(sqlStamp);

                string sqlSession = $"SELECT SUM(Duration) FROM Coding_Session WHERE Date >= {dateStamp}";
                string sqlTotal = "SELECT SUM(Hours) FROM Coding_Goal";
                goalHours = connection.QueryFirstOrDefault<int>(sqlTotal);
                totalHours = connection.ExecuteScalar<int>(sqlSession) / 3600;
            }

            int numberOfDays = validation.DaysRemaining(DateTime.Parse(goalDate));
            int remainingHours = goalHours - totalHours;
            int hoursPerDay = remainingHours / numberOfDays;

            if (remainingHours > 0 && numberOfDays >= 0)
            {
                tableGeneration.GoalBreakDown(remainingHours, totalHours, numberOfDays, hoursPerDay);
            }
            else if (remainingHours > 0 && numberOfDays < 0)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Oh No!!!\n").Color(Color.Red));
                AnsiConsole.Write(new FigletText("You Missed Your Goal :(").Color(Color.Red));
                AnsiConsole.Write(new FigletText("Press Any Key To Set New Goal").Color(Color.Green));
                Console.ReadLine();
                TransferGoal();
            }
            else if (remainingHours <= 0 && numberOfDays >= 0)
            {
                UpdateGoal();
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Well done!!!\n").Color(Color.Gold1));
                AnsiConsole.Write(new FigletText("You Reached Your Goal!!").Color(Color.Gold1));
                AnsiConsole.Write(new FigletText("Press Any Key To Set New Goal").Color(Color.Green));
                Console.ReadLine();
                TransferGoal();
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (DataException ex)
        {
            AnsiConsole.Write(new Markup("[red]You need to enter a goal to continue. Press any key to enter a goal[/]"));
            Console.ReadLine();
            CodingGoalInsert();
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }

    }

    internal void UpdateGoal()
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "UPDATE Coding_Goal SET Completed = 1 ";
                connection.Execute(sqlQuery);
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }
    }

    internal void TransferGoal()
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "INSERT INTO ALL_Coding_Goals (Date, Hours, Completed) SELECT Date, Hours, Completed FROM Coding_Goal";
                string sqlDeleteQuery = "DELETE FROM Coding_Goal";
                connection.Execute(sqlQuery);
                connection.Execute(sqlDeleteQuery);
            }
            AnsiConsole.Clear();
            CodingGoalInsert();
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]An error occurred: {ex.Message}[/]"));
        }
    }

    internal List<CodingGoal> ViewCodingGoals()
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM All_Coding_Goals";
                var tableData = connection.Query<CodingGoal>(sqlQuery).ToList();

                if (tableData.Any())
                {
                    AnsiConsole.Clear();
                    TableVisualisationEngine.ShowTable(tableData);
                }
                else
                {
                    AnsiConsole.Write(new Markup("[red]\nNo Data Found, Press any key to return to Main Menu[/]"));
                }
            }
            return new List<CodingGoal>();
        }
        catch (SqliteException ex)
        {
            AnsiConsole.Write(new Markup($"[red]A database occurred: {ex.Message}[/]"));
            return new List<CodingGoal>();
        }
    }
}