using System.Configuration;
using System.Data.SQLite;
using Spectre.Console;
using Dapper;

namespace CodingTracker
{
    public class DbController
    {
        private readonly string _connectionString;
        public SQLiteConnection _connection { get; private set; }
        CodingSession session = new CodingSession();
        private readonly Menu menu;

        public DbController(Menu _menu)
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"] ?? "Data Source=CodingSessions.db;";
            _connection = new SQLiteConnection("Data Source=CodingSessions.db;");
            InitializeDatabase();
            menu = _menu;
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
            using (var connection = new SQLiteConnection(_connection))
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
            using (var connection = new SQLiteConnection(_connection))
            {
                connection.Open();
                var records = connection.Query<CodingSession>("SELECT * FROM Sessions");

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
            
        }

        public void InsertRecords()
        {
            Validation validation = new Validation();
            AnsiConsole.MarkupLine("[green]Please type in the date of the start below:[/]");
            session.StartDate = Console.ReadLine();
            if (!String.IsNullOrEmpty(session.StartDate))
            {
                validation.ValidString(session.StartDate); // placeholder method, will check the input here.
            }
            AnsiConsole.MarkupLine("[green]Please type in the date of the end below:[/]");
            session.EndDate = Console.ReadLine();
            if (!String.IsNullOrEmpty(session.EndDate))
            {
                validation.ValidString(session.EndDate); // placeholder method, will check the input here.
            }
            AnsiConsole.MarkupLine("[green]Please type in the duration below:[/]");
            session.Duration = Console.ReadLine(); // placeholder for duration counting
            using (var connection = new SQLiteConnection(_connection))
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
                using (var connection = new SQLiteConnection(_connection))
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

                        if (!string.IsNullOrEmpty(newStartDate))
                        {
                            session.StartDate = newStartDate;
                        }
                        else
                        {
                            session.StartDate = existingRecord.StartDate;
                        }

                        if (!string.IsNullOrEmpty(newEndDate))
                        {
                            session.EndDate = newEndDate;
                        }
                        else
                        {
                            session.EndDate = existingRecord.EndDate;
                        }
                        string updateCommand = "UPDATE Sessions SET StartDate = @StartDate, EndDate = @EndDate WHERE Id = @Id";
                        connection.Execute(updateCommand, new { StartDate = session.StartDate, EndDate = session.EndDate, Id=id });
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
                using(var connection = new SQLiteConnection(_connection))
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
                            Thread.Sleep(1500);
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