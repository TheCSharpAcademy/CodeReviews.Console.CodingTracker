using System.Configuration;
using System.Data.SQLite;
using Spectre.Console;

namespace CodingTracker
{
    public class DbController
    {
        private readonly string _connectionString;

        public DbController()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"] ?? "Data Source=CodingSessions.db;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists("./CodingSessions.db"))
            {
                SQLiteConnection.CreateFile("CodingSessions.db");
                AnsiConsole.MarkupLine("[underline bold]The database has been created![/]");
            }

            using (var connection = new SQLiteConnection(_connectionString))
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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Sessions";

                using (var reader = selectCommand.ExecuteReader())
                {
                    bool hasRows = false;
                    while (reader.Read())
                    {
                        hasRows = true;
                        var id = reader.GetInt32(0);
                        var startDate = reader.GetString(1);
                        var endDate = reader.GetString(2);
                        var duration = reader.GetString(3);
                        Console.WriteLine($"Id: {id}, Beginning date: {startDate}, Ending date: {endDate}, Total duration: {duration}");
                    }

                    if (!hasRows)
                    {
                        AnsiConsole.MarkupLine("[bold]No records found.[/]");
                    }
                }
            }
        }

        public void InsertRecords()
        {
            AnsiConsole.MarkupLine("[green]Please type in the date of the start below:[/]");
            string startDate = Console.ReadLine();
            Validation validation = new Validation();

            if (!String.IsNullOrEmpty(startDate))
            {
                validation.ValidString(startDate); // placeholder method, will check the input here.
            }

            AnsiConsole.MarkupLine("[green]Please type in the date of the end below:[/]");
            string endDate = Console.ReadLine();
            if (!String.IsNullOrEmpty(endDate))
            {
                validation.ValidString(endDate); // placeholder method, will check the input here.
            }

            AnsiConsole.MarkupLine("[green]Please type in the duration below:[/]");
            string duration = Console.ReadLine(); // placeholder for duration counting

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"INSERT INTO Sessions (StartDate, EndDate, Duration)
                                              VALUES (@StartDate, @EndDate, @Duration)";
                insertCommand.Parameters.AddWithValue("@StartDate", startDate);
                insertCommand.Parameters.AddWithValue("@EndDate", endDate);
                insertCommand.Parameters.AddWithValue("@Duration", duration);
                insertCommand.ExecuteNonQuery();
                AnsiConsole.MarkupLine("[yellow]Record added![/]");
            }
        }

        public void UpdateRecord()
        {
            AnsiConsole.MarkupLine("[bold]Enter the Id of the record you want to update: [/]");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                using (var connection = new SQLiteConnection(_connectionString))
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
                            string startDate = Console.ReadLine();
                            AnsiConsole.MarkupLine("[green]Enter a new end date: [/]");
                            string endDate = Console.ReadLine();

                            if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                            {
                                var updateCommand = connection.CreateCommand();
                                updateCommand.CommandText = @"
                                    UPDATE Sessions
                                    SET StartDate = @StartDate, EndDate = @EndDate
                                    WHERE Id = @Id";
                                updateCommand.Parameters.AddWithValue("@StartDate", startDate);
                                updateCommand.Parameters.AddWithValue("@EndDate", endDate);
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