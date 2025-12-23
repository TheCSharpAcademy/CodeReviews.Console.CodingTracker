using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class TimeController
    {
        private SqliteConnection OpenConnection()
        {
            string? ConnectionString = ConfigurationManager.AppSettings["connectionString"];
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        internal void CreateDB()
        {
            var connection = OpenConnection();
            var sql = @"CREATE TABLE IF NOT EXISTS CodingSessions(
                                                    Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                                                    StartTime TEXT,
                                                    EndTime   TEXT,
                                                    Duration  TEXT);";
            connection.Execute(sql);
            connection.Close();
        }

        internal void ViewHistory()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Enums.SessionViewChoice>()
                .Title("[bold green]How would you like to see the sessions?[/]")
                .AddChoices(Enum.GetValues<Enums.SessionViewChoice>())
                );

            var connection = OpenConnection();
            var table = HelperFunctions.CreateTable();
            bool Custom = false;
            List<CodingSession> History = new();

            string sql = "";
            switch (choice) {
                case Enums.SessionViewChoice.Normal:
                    sql = "SELECT * FROM CodingSessions";
                    break;
                case Enums.SessionViewChoice.Chronologically:
                    sql = "SELECT * FROM CodingSessions ORDER BY StartTime ASC";
                    break;
                case Enums.SessionViewChoice.ReversedChronologically:
                    sql = "SELECT * FROM CodingSessions ORDER BY StartTime DESC";
                    break;
                case Enums.SessionViewChoice.SortedByDurationAscending:
                    sql = "SELECT * FROM CodingSessions ORDER BY Duration ASC";
                    break;
                case Enums.SessionViewChoice.SortedByDurationDescending:
                    sql = "SELECT * FROM CodingSessions ORDER BY Duration DESC";
                    break;
                case Enums.SessionViewChoice.Custom:
                    var start= AnsiConsole.Prompt(
                        new TextPrompt<DateOnly>("From this start date: (yyyy-MM-dd)"));
                    var end = AnsiConsole.Prompt(
                        new TextPrompt<DateOnly>("To this start date (yyyy-MM-dd)"));
                    
                    //Automatically include the whole day because im too lazy to type the time and i doubt anyone has several code sessions a day
                    DateTime startT = start.ToDateTime(new TimeOnly(00,00,00));
                    DateTime endT = end.ToDateTime(new TimeOnly(23,59,59));
                    string startDate = startT.ToString("yyyy-MM-dd HH:mm:ss");
                    string endDate = endT.ToString("yyyy-MM-dd HH:mm:ss");

                    
                    var parameters = new
                    {
                        start = startDate,
                        end = endDate
                    };
                    sql = @$"SELECT * FROM CodingSessions 
                            WHERE StartTime>=@start AND EndTime <=@end
                            ORDER BY StartTime ASC";
                    History = connection.Query<CodingSession>(sql,parameters).ToList();
                    Custom = true;
                    break;
            }

            if(!Custom) History = connection.Query<CodingSession>(sql).ToList();

            if (History.Any() == false) AnsiConsole.Write(new Markup("[bold red]The list is empty [/]\n"));
            else
            {
                foreach (var session in History)     
                {
                    table.AddRow($"[bold blue]{session.Id}[/]",$"[bold blue]{session.Duration}[/]",$"[bold blue]{session.StartTime}[/]",$"[bold blue]{session.EndTime}[/]");
                }
                AnsiConsole.Write(table);
            }
            connection.Close();
            HelperFunctions.ContinueToMainMenu();
        }
    

        internal void InsertTime()
        {
            var start = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("When did you start the Coding Session? (yyyy-MM-dd HH:mm:ss)"));
            var end = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("When did you finish the Coding Session? (yyyy-MM-dd HH:mm:ss)"));

            if(DateTime.Compare(start, end) >= 0) 
            {
                AnsiConsole.Write(new Markup("[bold red]\nError: Start time can't be equal to or later than end time. Returning to Main Menu.[/]"));
                Console.ReadKey();
                Console.Clear();
                UserInterface.MainMenu();
            }
            string startTime = start.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = end.ToString("yyyy-MM-dd HH:mm:ss");
            CodingSession UserInput = new CodingSession(startTime, endTime);
            
            var parameters = new 
            {
                ParametrisedStartTime = UserInput.StartTime,
                ParametrisedEndTime = UserInput.EndTime,
                ParametrisedDuration = UserInput.Duration
            };

            var connection = OpenConnection();
            var sql = @$"INSERT INTO CodingSessions(StartTime, EndTime, Duration)
                         VALUES(@ParametrisedStartTime, @ParametrisedEndTime, @ParametrisedDuration)";
            var InsertStatus = connection.Execute(sql, parameters);
            if(InsertStatus > 0) AnsiConsole.Write(new Markup("\n Session inserted successfully\n", UserInterface.MenuStyle));
            connection.Close();
            HelperFunctions.ContinueToMainMenu();
        }

        internal void InsertTime(DateTime start, DateTime end)
        {
            if (DateTime.Compare(start, end) >= 0)
            {
                AnsiConsole.Write(new Markup("[bold red]\nError: Start time can't be equal to or later than end time. Returning to Main Menu.[/]"));
                Console.ReadKey();
                Console.Clear();
                UserInterface.MainMenu();
            }
            string startTime = start.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = end.ToString("yyyy-MM-dd HH:mm:ss");
            CodingSession UserInput = new CodingSession(startTime, endTime);

            var parameters = new
            {
                ParametrisedStartTime = UserInput.StartTime,
                ParametrisedEndTime = UserInput.EndTime,
                ParametrisedDuration = UserInput.Duration
            };

            var connection = OpenConnection();
            var sql = @$"INSERT INTO CodingSessions(StartTime, EndTime, Duration)
                         VALUES(@ParametrisedStartTime, @ParametrisedEndTime, @ParametrisedDuration)";
            var InsertStatus = connection.Execute(sql, parameters);
            if (InsertStatus > 0) AnsiConsole.Write(new Markup("\n Session inserted successfully\n", UserInterface.MenuStyle));
            connection.Close();
            HelperFunctions.ContinueToMainMenu();
        }

        internal void DeleteSession()
        {
            var connection = OpenConnection();
            var sql = "SELECT * FROM CodingSessions";
            var SessionList = connection.Query<CodingSession>(sql).ToList();

            if (!SessionList.Any())
            {
                AnsiConsole.Write(new Markup("List is currently empty, returning to Main Menu", UserInterface.MenuStyle));
                HelperFunctions.ContinueToMainMenu();
            }

            var Session = AnsiConsole.Prompt(
                new SelectionPrompt<CodingSession>()
                .UseConverter(sessionDisplay=> sessionDisplay.DisplaySession())
                .AddChoices(SessionList));
            var SessionId = Session.Id;

            var confirm = AnsiConsole.Prompt(
                (new TextPrompt<bool>($"[red]Are you sure you want to delete session with ID:[/][green]{SessionId}[/]")
                .AddChoice(true)
                .AddChoice(false)
                .WithConverter(choice => choice? "y" : "n")));

            if (confirm)
            {
                var sqlDelete = $"DELETE FROM CodingSessions WHERE Id = {SessionId}";
                connection.Execute(sqlDelete);
                AnsiConsole.Write(new Markup($"[red]Session with Id: [/][green]{SessionId}[/][red] deleted[/]"));
                HelperFunctions.ContinueToMainMenu();
            }
            else
            {
                AnsiConsole.Write(new Markup("Session [red]NOT[/] deleted", UserInterface.MenuStyle));
                HelperFunctions.ContinueToMainMenu();
            }

        }
        internal void DeleteHistory()
        {
            var confirm = AnsiConsole.Prompt(
                new TextPrompt<bool>("[red]Are you sure you want to delete the history (Wipes the table)?[/]")
                .AddChoice(true)
                .AddChoice(false)
                .WithConverter(choice => choice? "y" : "n"));

            if (confirm)
            {
                var connection = OpenConnection();
                var sql = "DROP TABLE CodingSessions";
                connection.Execute(sql);
                AnsiConsole.Write(new Markup("History deleted", UserInterface.MenuStyle));
                connection.Close();
            }
            else
            {
                AnsiConsole.Write(new Markup("History [red]NOT[/] deleted", UserInterface.MenuStyle));
            }
            HelperFunctions.ContinueToMainMenu();
        }

        internal void StartTime()
        {
            Stopwatch stopwatch = new();
            string elapsedTime = "";
            DateTime start = DateTime.Now;

            stopwatch.Start();
            AnsiConsole.Write(new Markup("\nPress [blue]Enter[/] to start the stopwatch!\n", UserInterface.MenuStyle));
            var stopper = Console.ReadKey();

            Console.Clear();
            AnsiConsole.Write(new Markup("\nCoding session started! Press [blue]q[/] to stop the stopwatch!\n", UserInterface.MenuStyle));
            while(stopper.Key != ConsoleKey.Q)
            {
                Thread.Sleep(10);
                var elapsed = stopwatch.Elapsed;
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10);
                AnsiConsole.Write(new Markup($"[bold green]Time Elapsed:[/] [blue]{elapsedTime}[/] \r"));
                if (Console.KeyAvailable) stopper = Console.ReadKey(); //This triggers if a user presses a button, did this in order to not interrupt the while loop until neccessary
            }
            DateTime end = DateTime.Now;
            stopwatch.Stop();
            Console.Clear();

            AnsiConsole.Write(new Markup($"Press y to add Session with Duration: [blue]{elapsedTime}[/] to Database. Press anything else to cancel.", UserInterface.MenuStyle));
            var confirm = Console.ReadLine();
            if(confirm == "y")
            {
                Console.Clear();
                InsertTime(start, end);
            }
            else
            {
                AnsiConsole.Write(new Markup("Session Cancelled!", UserInterface.MenuStyle));
                HelperFunctions.ContinueToMainMenu();
            }
        }
    }
}