using System.Data.SQLite;
using System.Configuration;
using Dapper;
using System;
using System.Data.SqlClient;
using Spectre.Console;
namespace CodingTracker
{
    public class DbController
    {
        private readonly string _connectionString;
        public SQLiteConnection _connection { get; private set; }
        public DbController()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"];
            _connection = new SQLiteConnection("Data Source=CodingSessions.db;");
            if(!File.Exists("./CodingSessions.db"))
            {
                SQLiteConnection.CreateFile("CodingSessions.db");
                AnsiConsole.MarkupLine("[underline bold]The database has been created![/]");
            }
            using (var connection = new SQLiteConnection(_connection))
            {
                connection.Open();
                var tableCommand = _connection.CreateCommand();
                tableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Sessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartDate TEXT
                EndDate TEXT,
                Duration TEXT
            )";
                tableCommand.ExecuteNonQuery();
                _connection.Close();
                AnsiConsole.MarkupLine("[yellow bold]The table has been created and is ready to use![/]");
            }
            
        }
        public void ReadRecords()
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                connection.Open();
                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM coding";

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
                        AnsiConsole.MarkupLine($"[bold]Id: {id}, Beginning date: {startDate}, Ending date: {endDate} Total duration: {duration}[/b]");
                    }
                    if (!hasRows)
                    {
                        AnsiConsole.MarkupLine("[bold]No records found.[/]");
                    }
                }
                connection.Close();
            }
        }
        public void InsertRecords()
        {
            AnsiConsole.WriteLine("[green]Please type in the date of the start below:[/]");
            string startDate = Console.ReadLine();
            Validation validation = new Validation();
            if (!String.IsNullOrEmpty(startDate))
            {
                validation.ValidString(startDate); //placeholder method, will check the input here.
            }
            AnsiConsole.WriteLine("[green]Please type in the date of the end below:[/]");
            string endDate = Console.ReadLine();
            if (!String.IsNullOrEmpty(endDate))
            {
                validation.ValidString(endDate); //placeholder method, will check the input here.
            }
            string duration = Console.ReadLine(); //placeholder for duration counting
            using (var connection = new SQLiteConnection(_connection))
            {
                connection.Open();
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"INSERT INTO Sessions (StartDate, EndDate, Duration)
                                              VALUES (@StartDate, @EndDate, @Duration)";
                insertCommand.Parameters.AddWithValue("@StartDate", startDate);
                insertCommand.Parameters.AddWithValue("@EndDate", endDate);
                insertCommand.Parameters.AddWithValue("@Duration", duration);
                insertCommand.ExecuteNonQuery();
                connection.Close();
                AnsiConsole.MarkupLine("[yellow]Record added![/]");
            }
        }


    }
}
