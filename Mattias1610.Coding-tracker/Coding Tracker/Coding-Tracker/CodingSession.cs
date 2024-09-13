using System;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Diagnostics;

namespace Coding_Tracker
{
    public class CodingSession
    {
        string connectionString = @"Data source=coding-tracker.db";

        public void ShowMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                AnsiConsole.Markup("[yellow]---------[/] WELCOME TO MY CODING TRACKER [yellow]---------[/] \n");
                AnsiConsole.Markup($"\t TYPE 1 TO ADD NEW RECORD \n");
                AnsiConsole.Markup($"\t TYPE 2 TO DELETE A RECORD \n");
                AnsiConsole.Markup($"\t TYPE 3 TO UPDATE A RECORD \n");
                AnsiConsole.Markup($"\t TYPE 4 TO SEE THE RECORDS \n");
                AnsiConsole.Markup($"\t TYPE 5 TO USE A STOPWATCH \n");
                AnsiConsole.Markup($"\t TYPE 6 TO ORDER THE RECORDS \n");
                AnsiConsole.Markup($"\t TYPE 7 TO GENERATE REPORTS \n");
                AnsiConsole.Markup($"\t TYPE 8 TO SET A GOAL\n");
                AnsiConsole.Markup($"\t TYPE 0 TO EXIT \n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        AddRecord();
                        break;
                    case "2":
                        DeleteRecord();
                        break;
                    case "3":
                        UpdateRecord();
                        break;
                    case "4":
                        ReadTable();
                        break;
                    case "5":
                        GetStopWatch();
                        break;
                    case "6":
                        Sorted();
                        break;
                    case "7":
                        GetReport();
                        break;
                    case "8":
                        GetGoal();
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        AnsiConsole.Markup("[bold red] ERROR [/]\n");
                        break;
                }
            }
        }

        public void GetGoal()
        {
            AnsiConsole.MarkupLine("[bold yellow] PLEASE ENTER YOUR CODING GOAL IN HOURS:[/]");
            string goal = Console.ReadLine();

            if (!int.TryParse(goal, out int goalInHours))
            {
                AnsiConsole.MarkupLine("[bold red] INVALID [/]");
                return;
            }

            int goalInMinutes = goalInHours * 60;

            int totalCoding = 0;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand("SELECT Duration FROM coding_tracker", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string durationStr = reader.GetString(0);

                            if (TimeSpan.TryParse(durationStr, out TimeSpan duration))
                            {
                                totalCoding += (int)duration.TotalMinutes;
                            }
                        }
                    }
                }
            }

            int remaining = goalInMinutes - totalCoding;
            TimeSpan remainingTime = TimeSpan.FromMinutes(remaining);

            AnsiConsole.MarkupLine($"[bold yellow]Total Coding Time:[/] {TimeSpan.FromMinutes(totalCoding):hh\\:mm}");

            if (remaining > 0)
            {
                AnsiConsole.MarkupLine($"[bold yellow]You need to code for another:[/] {remainingTime:hh\\:mm} hours to reach your goal.");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold green]Congratulations! You've reached or exceeded your goal![/]");
            }
        }

        public void Sorted()
        {
            AnsiConsole.MarkupLine("[bold yellow] Choose period[/]");
            AnsiConsole.MarkupLine("\t 1. TODAY");
            AnsiConsole.MarkupLine("\t 2. THIS WEEK");
            AnsiConsole.MarkupLine("\t 3. THIS MONTH");
            AnsiConsole.MarkupLine("\t 4. THIS YEAR");
            AnsiConsole.MarkupLine("\t 5. ALL TIME");

            string periodChoice = Console.ReadLine();

            AnsiConsole.MarkupLine("[bold yellow] Choose order[/]");
            AnsiConsole.MarkupLine("\t 1. ASCENDING");
            AnsiConsole.MarkupLine("\t 2. DESCENDING");

            string orderChoice = Console.ReadLine();

            string query = BuildFilterAndOrderQuery(periodChoice, orderChoice);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id = reader.GetInt32(0);
                            string date = reader.GetString(1);
                            string duration = reader.GetString(2);

                            AnsiConsole.MarkupLine($"[bold yellow]Id:[/] {Id}, [bold yellow]Date:[/] {date}, [bold yellow]Duration:[/] {duration}");
                        }
                    }
                }
            }
        }

        public string BuildFilterAndOrderQuery(string periodChoice, string orderChoice)
        {
            string dateCondition = "";
            string orderBy = orderChoice == "1" ? "ASC" : "DESC";

            switch (periodChoice)
            {
                case "1":
                    dateCondition = "WHERE Date = date('now')";
                    break;
                case "2":
                    dateCondition = "WHERE Date >= date('now', '-7 days')";
                    break;
                case "3":
                    dateCondition = "WHERE Date >= date('now', 'start of the month')";
                    break;
                case "4":
                    dateCondition = "WHERE Date >= date('now', 'start of the year')";
                    break;
                case "5":
                    dateCondition = "";
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red] ERROR [/]");
                    break;
            }

            return $"SELECT * FROM coding_tracker {dateCondition} ORDER BY Date {orderBy}";
        }

        public void GetReport()
        {
            AnsiConsole.MarkupLine("[bold yellow] Choose period for the report[/]");
            AnsiConsole.MarkupLine("\t 1. TODAY");
            AnsiConsole.MarkupLine("\t 2. THIS WEEK");
            AnsiConsole.MarkupLine("\t 3. THIS MONTH");
            AnsiConsole.MarkupLine("\t 4. THIS YEAR");
            AnsiConsole.MarkupLine("\t 5. ALL TIME");

            string periodChoice = Console.ReadLine();

            GenerateReport(periodChoice);
        }

        public string BuildReportQuery(string periodChoice)
        {
            string dateCondition = "";

            switch (periodChoice)
            {
                case "1":
                    dateCondition = "WHERE Date = date('now')";
                    break;
                case "2":
                    dateCondition = "WHERE Date >= date('now', '-7 days')";
                    break;
                case "3":
                    dateCondition = "WHERE Date >= date('now', 'start of the month')";
                    break;
                case "4":
                    dateCondition = "WHERE Date >= date('now', 'start of the year')";
                    break;
                case "5":
                    dateCondition = "";
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red] ERROR: Invalid Choice [/]");
                    break;
            }

            return $@"
                SELECT 
                    SUM(strftime('%s', Duration)) AS TotalDurationSeconds, 
                    AVG(strftime('%s', Duration)) AS AverageDurationSeconds
                FROM coding_tracker 
                {dateCondition};";
        }

        public void GenerateReport(string periodChoice)
        {
            string query = BuildReportQuery(periodChoice);

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long totalDurationSeconds = reader.GetInt64(0);
                            double averageDurationSeconds = reader.GetDouble(1);

                            TimeSpan totalDuration = TimeSpan.FromSeconds(totalDurationSeconds);
                            TimeSpan averageDuration = TimeSpan.FromSeconds(averageDurationSeconds);

                            AnsiConsole.MarkupLine($"[bold yellow]Total Duration:[/] {totalDuration:hh\\:mm\\:ss}");
                            AnsiConsole.MarkupLine($"[bold yellow]Average Duration:[/] {averageDuration:hh\\:mm\\:ss}");
                        }
                    }
                }
            }
        }

        public string GetDate()
        {
            while (true)
            {
                AnsiConsole.MarkupLine("Enter the date in this format [bold red]DD-MM-YY[/]:");
                string initialDate = Console.ReadLine();

                if (DateOnly.TryParseExact(initialDate, "dd-MM-yy", out DateOnly date))
                {
                    return date.ToString("yyyy-MM-dd");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid date format. Please try again.[/]");
                }
            }
        }

        public string GetStartTime()
        {
            while (true)
            {
                AnsiConsole.MarkupLine("Enter the time you started coding in this format [bold red]HH:mm[/]:");
                string initialTime = Console.ReadLine();

                if (TimeOnly.TryParseExact(initialTime, "HH:mm", out TimeOnly time))
                {
                    return time.ToString("HH:mm");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid time format. Please try again.[/]");
                }
            }
        }

        public string GetEndTime()
        {
            while (true)
            {
                AnsiConsole.MarkupLine("Enter the time you ended coding in this format [bold red]HH:mm[/]:");
                string finalTime = Console.ReadLine();

                if (TimeOnly.TryParseExact(finalTime, "HH:mm", out TimeOnly time))
                {
                    return time.ToString("HH:mm");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid time format. Please try again.[/]");
                }
            }
        }

        public void GetStopWatch()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            AnsiConsole.MarkupLine("[bold yellow]Stopwatch started. Press Enter to stop.[/]");
            Console.ReadLine();
            stopwatch.Start();
            Console.ReadLine();
            stopwatch.Stop();
            AnsiConsole.MarkupLine($"Elapsed time: {stopwatch.Elapsed}");
        }

        public void AddRecord()
        {
            string date = GetDate();
            string startTime = GetStartTime();
            string endTime = GetEndTime();

            TimeSpan duration = TimeSpan.Parse(endTime) - TimeSpan.Parse(startTime);

            using (var connection = new SqliteConnection(connectionString))
            {
                string query = "INSERT INTO coding_tracker (Date, Duration) VALUES (@Date, @Duration)";
                connection.Execute(query, new { Date = date, Duration = duration.ToString(@"hh\:mm") });
            }

            AnsiConsole.MarkupLine("[bold green]Record added successfully![/]");
        }

        public void DeleteRecord()
        {
            AnsiConsole.MarkupLine("Enter the [bold red]Id[/] of the record you want to delete:");
            int id = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SqliteConnection(connectionString))
            {
                string query = "DELETE FROM coding_tracker WHERE Id = @Id";
                connection.Execute(query, new { Id = id });
            }

            AnsiConsole.MarkupLine("[bold green]Record deleted successfully![/]");
        }

        public void UpdateRecord()
        {
            AnsiConsole.MarkupLine("Enter the [bold red]Id[/] of the record you want to update:");
            int id = Convert.ToInt32(Console.ReadLine());

            string date = GetDate();
            string startTime = GetStartTime();
            string endTime = GetEndTime();

            TimeSpan duration = TimeSpan.Parse(endTime) - TimeSpan.Parse(startTime);

            using (var connection = new SqliteConnection(connectionString))
            {
                string query = "UPDATE coding_tracker SET Date = @Date, Duration = @Duration WHERE Id = @Id";
                connection.Execute(query, new { Date = date, Duration = duration.ToString(@"hh\:mm"), Id = id });
            }

            AnsiConsole.MarkupLine("[bold green]Record updated successfully![/]");
        }

        public void ReadTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var table = connection.Query("SELECT * FROM coding_tracker");

                AnsiConsole.MarkupLine("[bold yellow]Here are the records:[/]");
                foreach (var row in table)
                {
                    AnsiConsole.MarkupLine($"Id: {row.Id}, Date: {row.Date}, Duration: {row.Duration}");
                }
            }
        }
    }

}
