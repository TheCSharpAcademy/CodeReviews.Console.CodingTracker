using static weakiepedia.CodingTracker.ConfigurationHelper;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using Dapper;
using Spectre.Console;

namespace weakiepedia.CodingTracker;

internal class DatabaseManager
{
    internal static void CreateTableIfNotExists()
    {
        using (var connection = new SQLiteConnection(GetConnectionString()))
        {
            connection.Open();
            string query = "CREATE TABLE IF NOT EXISTS 'time' (Id INTEGER PRIMARY KEY AUTOINCREMENT, StartTime TEXT NOT NULL, EndTime TEXT NOT NULL, Duration TEXT NOT NULL)";
            connection.Execute(query);
            connection.Close();
        }
    }

    internal static void ViewAllRecords()
    {
        Console.Clear();
        try
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM time";

                var codingSessions = connection.Query<CodingSession>(query).ToList();

                var table = new Table();
                table = table.Title("[aquamarine3]Coding sessions[/]");
                table.Border(TableBorder.HeavyHead);
                table.AddColumn("ID").AddColumn("StartTime").AddColumn("EndTime").AddColumn("Duration");

                if (codingSessions.Any())
                {
                    foreach (var codingSession in codingSessions)
                    {
                        table.AddRow(codingSession.Id.ToString(), codingSession.StartTime, codingSession.EndTime, codingSession.Duration);
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    UserInterface.PressAnyKey();
                    UserInterface.Menu();
                }
                
                AnsiConsole.Write(table);
                
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(ex.Message);
        }
    }

    internal static void ViewSummary()
    {
        Console.Clear();
        
        try
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT Duration FROM time";

                var durationList = connection.Query<string>(query).ToList();

                float totalHours = 0;
                foreach (var duration in durationList)
                {
                    int extractHours = Convert.ToInt32(duration.Substring(0, 2));
                    int extractMinutes = Convert.ToInt32(duration.Substring(3, 2));
                    int totalMinutes = extractHours * 60 + extractMinutes;
                    totalHours += totalMinutes;
                }
                
                totalHours = totalHours / 60;
                float average = totalHours / durationList.Count;
                
                AnsiConsole.MarkupLine($"[aquamarine3]Summary[/]");
                AnsiConsole.MarkupLine($"Total time spent on coding: {totalHours.ToString("F2")} hours");
                AnsiConsole.MarkupLine($"Average time per session: {average.ToString("F2")} hours");
                
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(ex.Message);
        }
    }

    internal static void InsertRecord()
    {
        Console.Clear();

        DateTime startTime;
        DateTime endTime;
        string duration;
        
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What do you want to do?")
            .AddChoices("Enter time manually", "Get time automatically (stopwatch)")
            .HighlightStyle("aquamarine3"));

        if (choice == "Enter time manually")
        {
            bool validation = false;
            do
            {
                startTime = TimeHelper.GetUserTimeManually("Enter start time (HH:mm): ", 1);
                endTime = TimeHelper.GetUserTimeManually("Enter end time (HH:mm): ", 2);
                duration = TimeHelper.CalculateDuration(startTime, endTime);
                if (startTime < endTime)
                {
                    validation = true;
                }
                else
                {
                    AnsiConsole.MarkupLine("Start time can't be later than end time");
                    AnsiConsole.MarkupLine("");
                }
            } while (validation == false);
            
            try
            {
                using (var connection = new SQLiteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO time (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
                
                    connection.Execute(query, new { StartTime = startTime, EndTime = endTime, Duration = duration });
                
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine(ex.Message);
            }
        }
        else if (choice == "Get time automatically (stopwatch)")
        {
            Stopwatch stopwatch = new Stopwatch();
            
            startTime = DateTime.Now;
            string tempStartTime = startTime.ToString("HH:mm");
            stopwatch.Start();
            AnsiConsole.MarkupLine("[aquamarine1_1]Stopwatch is running, press any key to stop.[/]");
            AnsiConsole.MarkupLine("Start Time: " + startTime.ToString("HH:mm"));
            
            Console.ReadKey();
            
            stopwatch.Stop();
            endTime = DateTime.Now;
            string tempEndTime = startTime.ToString("HH:mm");
            AnsiConsole.MarkupLine("End Time: " + endTime.ToString("HH:mm"));
            
            DateTime properStartTime = DateTime.ParseExact(tempStartTime, "HH:mm", new CultureInfo("en-US"));
            DateTime properEndTime = DateTime.ParseExact(tempEndTime, "HH:mm", new CultureInfo("en-US"));
            duration = TimeHelper.CalculateDuration(startTime, endTime);
            
            try
            {
                using (var connection = new SQLiteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO time (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
                
                    connection.Execute(query, new { StartTime = properStartTime, EndTime = properEndTime, Duration = duration });
                
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine(ex.Message);
            }
        }
    }

    internal static void DeleteRecord()
    {
        Console.Clear();
        ViewAllRecords();
        
        AnsiConsole.Markup("Enter ID of the record you want to delete: ");
        string input = Console.ReadLine();
        int id;
                
        while (!int.TryParse(input, out id))
        {
            AnsiConsole.Markup("Can't parse the input, please try again: ");
            input = Console.ReadLine();
        }
        
        try
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                string query = "DELETE FROM time WHERE Id = @Id";
                connection.Open();

                connection.Execute(query, new { Id = id });
                
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(ex.Message);
        }
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        ViewAllRecords();
        
        AnsiConsole.Markup("Enter ID of the record you want to update: ");
        string input = Console.ReadLine();
        
        int id;
        while (!int.TryParse(input, out id))
        {
            AnsiConsole.Markup("Can't parse the input, please try again: ");
            input = Console.ReadLine();
        }
        
        DateTime startTime = TimeHelper.GetUserTimeManually("Enter start time (HH:mm): ", 1);
        DateTime endTime = TimeHelper.GetUserTimeManually("Enter end time (HH:mm): ", 2);
        string duration = TimeHelper.CalculateDuration(startTime, endTime);
        
        try
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                string query = "UPDATE time SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
                connection.Open();
                
                connection.Execute(query, new { StartTime = startTime, EndTime = endTime, Duration = duration, Id = id });
                
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine(ex.Message);
        }
    }
}
