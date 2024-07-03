using System.Data.SQLite;
using Spectre.Console;
using Dapper;

namespace CodingTracker
{
    public class DbController
    {
        private readonly string _connectionString;
        public SQLiteConnection Conn { get; private set; } //renamed from _connection because of codacy.
        CodingSession session = new CodingSession();
        private readonly Menu menu;
        Validation validation;
        UserInput userInput;

        public DbController(Menu _menu)
        {
            _connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"] ?? "Data Source=CodingSessions.db;";
            Conn = new SQLiteConnection("Data Source=CodingSessions.db;");
            InitializeDatabase();
            menu = _menu;
            validation = new Validation(string.Empty);
            userInput = new UserInput(session,this);
        }

        private void InitializeDatabase()
        {
            //Creating the database if it does not exist already
            if (!File.Exists("./CodingSessions.db"))
            {
                SQLiteConnection.CreateFile("CodingSessions.db");
                AnsiConsole.MarkupLine("[underline bold]The database has been created![/]");
            }
            //Creating the table if it does not exist already
            using (var connection = new SQLiteConnection(Conn))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Sessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDate TEXT,
                        EndDate TEXT,
                        Duration TEXT
                    )";
                tableCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ReadRecords()
        {
            using (var connection = new SQLiteConnection(Conn))
            {
                connection.Open();
                var records = connection.Query<CodingSession>("SELECT * FROM Sessions").ToList();

                if (records.Any())
                {
                    foreach (var record in records)
                    {
                        AnsiConsole.MarkupLine($"[bold]Id: {record.Id}, Beginning date: {record.StartDate}, Ending date: {record.EndDate}, Total duration: {record.Duration}[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold]No records found.[/]");
                    Thread.Sleep(1000);
                    menu.DisplayMenu();
                }
                connection.Close();
            }
            AnsiConsole.MarkupLine("[bold]Press any key to return to the main menu[/]");
            Console.ReadKey();
            menu.DisplayMenu();
        }

        public void InsertRecords()
        {
            userInput.DateOfStart();
            userInput.DateOfEnd();
            session.Duration = userInput.Duration(session.StartDate, session.EndDate);
            if(session.Duration=="")
            {
                AnsiConsole.MarkupLine("[bold red]The provided data was not correct, try again.[/]");
                Thread.Sleep(1000);
                InsertRecords();
            }
            using (var connection = new SQLiteConnection(Conn))
            {
                connection.Open();
                string sqlCommand = "INSERT INTO Sessions (StartDate, EndDate, Duration) VALUES (@StartDate, @EndDate, @Duration)";
                var parameters = new {session.StartDate, session.EndDate, session.Duration};
                connection.Execute(sqlCommand,parameters);
                connection.Close();
            }
            Thread.Sleep(1000);
            menu.DisplayMenu();
        }

        public void UpdateRecord()
        {
            AnsiConsole.MarkupLine("[bold]Enter the Id of the record you want to update: [/]");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                using (var connection = new SQLiteConnection(Conn))
                {
                    connection.Open();
                    var existingRecord = connection.QueryFirstOrDefault<CodingSession>("SELECT * FROM Sessions WHERE Id = @Id", new { Id = id });
                    if (existingRecord != null)
                    {
                        AnsiConsole.MarkupLine($"Start Date: {existingRecord.StartDate}");
                        AnsiConsole.MarkupLine($"End date: {existingRecord.EndDate}");

                        AnsiConsole.MarkupLine("[green]Enter a new start date (leave empty to keep the current value): [/]");
                        string newStartDate = Console.ReadLine();
                        AnsiConsole.MarkupLine("[green]Enter a new end date (leave empty to keep the current value): [/]");
                        string newEndDate = Console.ReadLine();

                        if (!string.IsNullOrEmpty(newStartDate)&&validation.ValidString(newEndDate))
                        {
                            session.StartDate = newStartDate;
                        }
                        else
                        {
                            session.StartDate = existingRecord.StartDate;
                            AnsiConsole.MarkupLine("[bold red]The record will not be changed, the given data was not correct![/]");
                        }

                        if (!string.IsNullOrEmpty(newEndDate))
                        {
                            session.EndDate = newEndDate;
                        }
                        else
                        {
                            session.EndDate = existingRecord.EndDate;
                            AnsiConsole.MarkupLine("[bold red]The record will not be changed, the given data was not correct![/]");
                        }
                        session.Duration = userInput.Duration(session.StartDate, session.EndDate);
                        if (session.Duration == "")
                        {
                            AnsiConsole.MarkupLine("[bold red]The provided data was not correct, try again.[/]");
                            Thread.Sleep(1000);
                            UpdateRecord();
                        }
                        string updateCommand = "UPDATE Sessions SET StartDate = @StartDate, EndDate = @EndDate, Duration = @Duration WHERE Id = @Id";
                        connection.Execute(updateCommand, new { StartDate = session.StartDate, EndDate = session.EndDate, Duration = session.Duration, Id=id });
                        AnsiConsole.MarkupLine("[yellow]Record updated.[/]");
                        connection.Close();

                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Record not found.[/]");
                    }
                }
            }
            else 
            {
                AnsiConsole.MarkupLine("[red]Invalid ID.[/]");
                Thread.Sleep(1000);
                menu.DisplayMenu();
            }
            Thread.Sleep(1000);
            menu.DisplayMenu();
        }
        public void DeleteRecord()
        {
            AnsiConsole.MarkupLine("[bold]Enter the Id of the record you want to delete: [/]");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                using(var connection = new SQLiteConnection(Conn))
                {
                    connection.Open();
                    var existingRecord = connection.Query<CodingSession>("SELECT * FROM Sessions WHERE Id=@Id",new { Id = id }).FirstOrDefault();
                    if (existingRecord != null)
                    {
                        AnsiConsole.MarkupLine("[bold]Record found! Are you sure you want to delete it from the database? Y/N[/]");
                        string confirmation = Console.ReadLine().ToLower();
                        if (confirmation == "y" || confirmation == "yes")
                        {
                            string deleteCommand = "DELETE FROM Sessions WHERE Id=@Id";
                            connection.Execute(deleteCommand, new { Id = id });
                            AnsiConsole.MarkupLine("[yellow bold]Record deleted[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Returning to the menu now.[/]");
                        }
                    }
                    connection.Close();
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No record found with the Id you've provided. Try again...[/]");
            }
            Thread.Sleep(1000);
            menu.DisplayMenu();
        }
    }
}