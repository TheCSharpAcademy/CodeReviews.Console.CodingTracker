using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;
using System.Data;
using System.Globalization;


namespace CodingTracker;
internal class Controller
{
    static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
    static string tableString = ConfigurationManager.AppSettings.Get("connectionString");

    internal static void Insert()
    {
        Console.Clear();
        ShowTable();

        string startDate = UserInput.GetDateInput("StartDate");
        string endDate = UserInput.GetDateInput("EndDate");
        

        DateTime start;
        DateTime.TryParseExact(startDate, "HH-dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out start);
        DateTime end;
        DateTime.TryParseExact(endDate, "HH-dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out end);

        if(end < start)
        {
            Console.WriteLine("End date must be later than the startDate, please enter again.");
            endDate = UserInput.GetDateInput("EndDate");
        }
        double duration = (end - start).TotalHours;

        string? sql;
        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            sql = $"INSERT INTO CodingSessions(StartTime, EndTime, Duration) VALUES('{startDate}', '{endDate}', {duration})";
            connection.Execute(sql);

            connection.Close();
        }
    }

    internal static void ShowTable()
    {
        Console.Clear();

        string? sql;

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            sql = $"SELECT * FROM CodingSessions";
            var tables = connection.Query<CodingSessions>(sql);


            var table = new Table();
            table.Border = TableBorder.MinimalDoubleHead;
            table.BorderColor<Table>(Color.Yellow);

            table.AddColumn("[green]ID[/]");
            table.AddColumn("[green]StartDate[/]");
            table.AddColumn("[green]EndDate[/]");
            table.AddColumn("[green]Duration[/]");

            DateTime start;
            DateTime end;

            foreach (var row in tables)
            {
                // Add some rows
                DateTime.TryParseExact(row.StartTime, "HH-dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out start);
                DateTime.TryParseExact(row.EndTime, "HH-dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out end);
                table.AddRow($"{row.ID}", $"{start.Hour} O'Clock on {start.Date}/{start.Month}/{start.Year}", $"{end.Hour} O' Clock on {end.Date}/{end.Month}/{end.Year}", $"{row.Duration} hours");
            }
            // Render the table to the console
            AnsiConsole.Write(table);
            
            connection.Close();
        }
    }

    internal static void Delete()
    {
        Console.Clear();
        ShowTable();

        string? sql;
        int id = UserInput.GetNumberInput("Enter the id to delete that row");

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            sql = $"DELETE FROM CodingSessions WHERE ID='{id}'";

            if (0 == connection.Execute(sql))
            {
                Console.WriteLine("the id does not exist in this table, please try again");
                Delete();
            }

            connection.Close();
        }
    }

    internal static void Update()
    {
        Console.Clear();
        ShowTable();

        int id = UserInput.GetNumberInput("Enter the id to update that row");

        using (IDbConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string startDate = UserInput.GetDateInput("StartDate");
            string endDate   = UserInput.GetDateInput("EndDate");

            DateTime start;
            DateTime.TryParseExact(startDate, "HH-dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out start);
            DateTime end;
            DateTime.TryParseExact(endDate, "HH-dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out end);

            if (end < start)
            {
                Console.WriteLine("End date must be later than the startDate, please enter again.");
                endDate = UserInput.GetDateInput("EndDate");
            }

            double duration = (end - start).TotalHours;

            var sql = $"UPDATE CodingSessions SET StartTime = '{startDate}', EndTime = '{endDate}', Duration = {duration} WHERE Id = {id}";

            if (connection.Execute(sql) == 0)
            {
                Console.WriteLine("Row does not exist, try again");
                Update();
            }

            connection.Close();
        }
        Console.WriteLine("Updated, press to go back to main menu");
        Console.ReadLine();

    }

    internal static void Timer()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Press to start/end timer");
        Console.Read();
        int seconds = 0;
        while (true)
        {
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // Read the key to clear it from the buffer
                break;
            }

            AnsiConsole.Progress()
                .HideCompleted(true)
                .StartAsync(async ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask($"[green]Timer started... {seconds++}[/]");
                    AnsiConsole.WriteLine("Press to start timer");

                    while (!ctx.IsFinished)
                    {
                        task1.Increment(1.0);
                        await Task.Delay(10); // Control the speed of the progress bar
                    }
                }).GetAwaiter().GetResult();
            
            // Clear the console to refresh the progress bar in the same line
            AnsiConsole.Clear();          
        }

        AnsiConsole.MarkupLine($"You've been coding for [green]{seconds}[/] seconds!");
        Console.WriteLine("Press to go back to menu");
        Console.ReadLine();
        Console.ReadLine();



    }
}
