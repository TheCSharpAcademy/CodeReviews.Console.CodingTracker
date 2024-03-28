using Microsoft.Data.Sqlite;
using System.Configuration;
using Dapper;
using Spectre.Console;
using System.Globalization;
using System.Diagnostics;

namespace CodingTracker;

public class CodingController
{
    readonly string connectionString = ConfigurationManager.AppSettings.Get("connectionString")!;
    readonly private Stopwatch stopwatch;
    private TimeSpan pausedTime;
    bool timerIsPaused;

    public UserInput UserInput { get; set; }

    public CodingController()
    {
        stopwatch = new Stopwatch();
        pausedTime = TimeSpan.Zero;
        timerIsPaused = false;
    }

    internal List<Coding> Get()
    {
        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            string sql = "SELECT * FROM coding";
            var tableData = connection.Query<Coding>(sql).ToList();

            if (tableData.Any())
            {
                AnsiConsole.Clear();
                TableVisualisation.ShowTable(tableData);
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No rows found[/]");
            }

            return tableData;
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]An error occurred: {ex.Message}[/]");
            return new List<Coding>();
        }
    }

    internal Coding GetById(int id)
    {
        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            string sql = ($"SELECT * FROM coding Where Id = '{id}'");
            var tableData = connection.QueryFirstOrDefault<Coding>(sql, new { Id = id });

            if (tableData != null)
            {
                AnsiConsole.Clear();
            }

            return tableData!;
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]An error occurred: {ex.Message}[/]");
            return null;
        }
    }

    internal void Insert()
    {
        try
        {
            var date = GetDateInput();
            var duration = GetDurationInput();

            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(duration))
            {
                AnsiConsole.MarkupLine("[red]Invalid date or duration. Record not added.[/]");
            }

            Coding coding = new()
            {
                Date = date,
                Duration = duration
            };

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            string sql = "INSERT INTO coding (date, duration) VALUES (@Date, @Duration)";
            connection.Execute(sql, new { coding.Date, coding.Duration });

            AnsiConsole.MarkupLine("[green]Record was added![/]");
            Thread.Sleep(3000);
            AnsiConsole.Clear();
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]Database error: {ex.Message}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
        }
    }

    internal void Delete()
    {
        try
        {
            Get();
            int id = AnsiConsole.Ask<int>("Please enter the record Id you want to delete (or enter 0 to go back to Main Menu)):");

            while (id < 0)
            {
                AnsiConsole.MarkupLine("[yellow]You have to type a valid Id![/]");
                id = AnsiConsole.Ask<int>("Please enter the record Id you want to delete (or enter 0 to go back to Main Menu):");
            }

            if (id == 0)
            {
                AnsiConsole.Clear();
                UserInput.MainMenu();
                return;
            }

            var coding = GetById(id);

            while (coding.Id == 0)
            {
                AnsiConsole.MarkupLine($"[red]Record Id {id} does not exist![/]");
                return;
            }

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            string sql = "DELETE FROM coding WHERE Id = (@Id)";
            connection.Execute(sql, new { Id = id });

            AnsiConsole.MarkupLine($"[green]Record {id} was deleted![/]");
            Thread.Sleep(1000);
            AnsiConsole.Clear();

            Delete();
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]Database error: {ex.Message}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
        }
    }

    internal void Update()
    {
        try
        {
            bool updating = true;

            while (updating)
            {
                Get();
                int id = AnsiConsole.Ask<int>("Please enter the record Id you want to update (or enter 0 to go back to Main Menu):");

                while (id < 0)
                {
                    AnsiConsole.MarkupLine("[yellow]You have to type a valid Id![/]");
                    id = AnsiConsole.Ask<int>("Please enter the record Id you want to update (or enter 0 to go back to Main Menu):");
                }

                if (id == 0)
                {
                    AnsiConsole.Clear();
                    UserInput.MainMenu();
                    break;
                }

                var coding = GetById(id);

                if (coding == null)
                {
                    AnsiConsole.MarkupLine($"[red]Record with Id {id} does not exist![/]");
                    continue;
                }

                var updateInput = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Choose one of the following options:")
                    .PageSize(10)
                    .AddChoices(
                        "Date",
                        "Duration",
                        "Main Menu"
                    ));

                switch (updateInput)
                {
                    case "Date":
                        coding.Date = GetDateInput();
                        break;
                    case "Duration":
                        coding.Duration = GetDurationInput();
                        break;
                    case "Main Menu":
                        UserInput.MainMenu();
                        updating = false;
                        break;
                    default:
                        AnsiConsole.MarkupLine("Invalid input.");
                        break;
                }

                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                string sql =
                    @"UPDATE coding SET
                    Date = @Date,
                    Duration = @Duration
                WHERE
                    Id = @Id";
                connection.Execute(sql, new { coding.Date, coding.Duration, coding.Id });

                AnsiConsole.MarkupLine($"[green]Record with Id {coding.Id} was updated![/]");
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]Database error: {ex.Message}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
        }
    }

    internal void TimerStartPauseResume()
    {
        try
        {
            Console.Clear();

            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                AnsiConsole.MarkupLine("Timer paused.");
                timerIsPaused = true;
            }
            else if (timerIsPaused)
            {
                stopwatch.Start();
                AnsiConsole.MarkupLine("Timer resumed.");
                timerIsPaused = false;
            }
            else
            {
                stopwatch.Start();
                AnsiConsole.MarkupLine("Timer started.");
            }

            UserInput.TimerMenu();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
        }
    }

    internal void TimerStop()
    {
        try
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"Timer stopped. Elapsed time: {stopwatch.Elapsed - pausedTime}");

                TimeSpan elapsed = stopwatch.Elapsed - pausedTime;

                if (elapsed.TotalMinutes >= 1)
                {
                    string duration = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}";

                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO coding (date, duration) VALUES (@Date, @Duration)";
                        connection.Execute(sql, new { Date = DateTime.Now.ToString("dd-MM-yy"), Duration = duration });
                    }

                    AnsiConsole.MarkupLine("Record was added");
                }
                else
                {
                    AnsiConsole.MarkupLine("Elapsed time is less than 1 minute. No entry added to the database.");
                }

                stopwatch.Reset();
                pausedTime = TimeSpan.Zero;
                timerIsPaused = false;
            }
            else
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("Timer is not running.");
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]Database error: {ex.Message}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
        }
    }

    internal string GetDateInput()
    {
        try
        {
            string dateInput;
            bool isValidDate = false;

            do
            {
                dateInput = AnsiConsole.Ask<string>("Please enter the date (Format: dd-MM-yy):");

                if (dateInput == "0")
                {
                    AnsiConsole.Clear();
                    UserInput.MainMenu();
                    return string.Empty;
                }

                if (DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    isValidDate = true;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid date format. Please enter the date with the format 'dd-MM-yy'![/]");
                }

            } while (!isValidDate);

            return dateInput;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
            return string.Empty;
        }
    }

    internal string GetDurationInput()
    {
        try
        {
            string durationInput = AnsiConsole.Ask<string>("Please enter the duration (Format: hh:mm):");

            while (!TimeSpan.TryParseExact(durationInput, "h\\:mm", CultureInfo.InvariantCulture, out _) || durationInput == "0")
            {
                if (durationInput == "0")
                {
                    AnsiConsole.Clear();
                    UserInput.MainMenu();
                    return string.Empty;
                }
                AnsiConsole.MarkupLine("[red]Not a valid duration. Please enter the duration with the format 'hh:mm'![/]");
                durationInput = AnsiConsole.Ask<string>("Please enter the duration (Format: hh:mm):");
            }
            return durationInput;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
            return string.Empty;
        }
    }

    internal void GenerateReport()
    {
        try
        {
            DateTime today = DateTime.Today;

            int numberOfDays = AnsiConsole.Ask<int>("Enter the number of days for the report:");
            DateTime startDate = today.AddDays(-numberOfDays + 1);
            DateTime endDate = today;

            var reportData = GetReportData(startDate, endDate);
            TimeSpan totalTime = GetTotalTimeForPeriod(startDate, endDate);

            AnsiConsole.Clear();
            TableVisualisation.DisplayReport(reportData, totalTime);

            AnsiConsole.MarkupLine("Press any key to return to the main menu...");
            Console.ReadKey();
            AnsiConsole.Clear();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
        }
    }

    internal List<Coding> GetReportData(DateTime startDate, DateTime endDate)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM coding WHERE Date BETWEEN @StartDate AND @EndDate";
            return connection.Query<Coding>(sql, new { StartDate = startDate.ToString("dd-MM-yy"), EndDate = endDate.ToString("dd-MM-yy") }).ToList();
        }
    }

    internal TimeSpan GetTotalTimeForPeriod(DateTime startDate, DateTime endDate)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT Duration FROM coding WHERE Date BETWEEN @StartDate AND @EndDate";
            var durations = connection.Query<string>(sql, new { StartDate = startDate.ToString("dd-MM-yy"), EndDate = endDate.ToString("dd-MM-yy") }).ToList();

            TimeSpan totalTime = TimeSpan.Zero;
            foreach (var duration in durations)
            {
                if (TimeSpan.TryParse(duration, out TimeSpan sessionDuration))
                {
                    totalTime += sessionDuration;
                }
            }
            return totalTime;
        }
    }
}
