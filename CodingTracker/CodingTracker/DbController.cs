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

        public DbController()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"] ?? "Data Source=CodingSessions.db;";
            _connection = new SQLiteConnection("Data Source=CodingSessions.db;");
            InitializeDatabase();
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
                }
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
        }

        public void UpdateRecord()
        {
            AnsiConsole.MarkupLine("[bold]Enter the Id of the record you want to update: [/]");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                using (var connection = new SQLiteConnection(_connection))
                {
                    connection.Open();
                    var selectCommand = connection.CreateCommand();
                    selectCommand.CommandText = "SELECT * FROM Sessions WHERE Id = @Id";
                    selectCommand.Parameters.AddWithValue("@Id", id);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            AnsiConsole.MarkupLine($"Start Date: {reader.GetString(1)}");
                            AnsiConsole.MarkupLine($"End date: {reader.GetString(2)}");

                            AnsiConsole.MarkupLine("[green]Enter a new start date: [/]");
                            session.StartDate = Console.ReadLine();
                            AnsiConsole.MarkupLine("[green]Enter a new end date: [/]");
                            session.EndDate = Console.ReadLine();

                            if (!String.IsNullOrEmpty(session.StartDate) && !String.IsNullOrEmpty(session.EndDate))
                            {
                                var updateCommand = connection.CreateCommand();
                                updateCommand.CommandText = @"
                                    UPDATE Sessions
                                    SET StartDate = @StartDate, EndDate = @EndDate
                                    WHERE Id = @Id";
                                updateCommand.Parameters.AddWithValue("@StartDate", session.StartDate);
                                updateCommand.Parameters.AddWithValue("@EndDate", session.EndDate);
                                updateCommand.Parameters.AddWithValue("@Id", id);
                                updateCommand.ExecuteNonQuery();
                                AnsiConsole.MarkupLine("[yellow]Record updated.[/]");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]Invalid input for dates.[/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Record not found.[/]");
                        }
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid Id.[/]");
            }
        }
    }
}