using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace CodingTracker.Hacker_735
{
    internal class TableController
    {
        private static readonly string connectionString = AppConfig.ConnectionString;
        static internal void CreateTable()
        {
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask("[yellow]Setting up database[/]");

                    using var connection = new SqliteConnection(connectionString);
                    connection.Open();
                    task.Increment(40);

                    string createTableSql =
                        @"CREATE TABLE IF NOT EXISTS coding_sessions (
                id INTEGER PRIMARY KEY,
                Date TEXT,
                StartTime TEXT,
                EndTime TEXT,
                Time TEXT
                )";

                    connection.Execute(createTableSql);
                    task.Increment(60);
                });

            AnsiConsole.MarkupLine("[green]Table ready.[/]");
        }
        static internal void Insert()
        {
            string date = TimeInputController.GetDateInput();
            var timeResult = TimeInputController.GetTimeInput();

            Validation.SafeExecute("Inserting session", () =>
            {

                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                connection.Execute(
                    "INSERT INTO coding_sessions(Date, StartTime, EndTime, Time) VALUES(@Date, @StartTime, @EndTime, @Time)",
                    new
                    {
                        Date = date,
                        StartTime = timeResult.StartTime.ToString("HH:mm"),
                        EndTime = timeResult.EndTime.ToString("HH:mm"),
                        Time = timeResult.FinalTime
                    });
            });
        }

        static internal void Display()
        {

            Console.Clear();

            Validation.SafeExecute("Displaying sessions", () =>
            {

                using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var sessions = connection.Query<CodingSession>(
                "SELECT id AS Id, Date, StartTime, EndTime, Time FROM coding_sessions ORDER BY id"
            ).ToList();

            if (!sessions.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
                return;
            }

            var table = new Table();


            table.AddColumn("Id");
            table.AddColumn("Date");
            table.AddColumn("Start");
            table.AddColumn("End");
            table.AddColumn("Duration");

            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.Date,
                    session.StartTime,
                    session.EndTime,
                    session.Time
                );
            }

            AnsiConsole.Write(table);
            });
        }

        static internal void Delete()
        {
            string action = "delete";

            int id = SelectSessionId(action);

            Validation.SafeExecute("deleting session", () =>
            {

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                int rowsAffected = connection.Execute(
                    "DELETE FROM coding_sessions WHERE id = @Id",
                    new { Id = id }
                );

                if (rowsAffected > 0)
                    AnsiConsole.MarkupLine($"[green]Deleted session {id}.[/]");
                else
                    AnsiConsole.MarkupLine("[yellow]Nothing was deleted.[/]");
            });
        }

        static private List<int> GetAllSessionIds()
        {
            return Validation.SafeExecute("fetching session ids", () =>
            {
                using var connection = new SqliteConnection(connectionString);
            connection.Open();

            return connection.Query<int>("SELECT id FROM coding_sessions ORDER BY id").ToList();
            }, new List<int>());
        }

        static internal void Update()
        {
            string action = "update";

            int id = SelectSessionId(action);

            string date = TimeInputController.GetDateInput();

            var timeResult = TimeInputController.GetTimeInput();
            string time = timeResult.FinalTime;
            DateTime startingTime = timeResult.StartTime;
            DateTime endingTime = timeResult.EndTime;

            Validation.SafeExecute("Displaying sessions", () =>
            {

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                int rowsAffected = connection.Execute(
                    @"UPDATE coding_sessions
          SET Date = @Date, StartTime = @StartTime, EndTime = @EndTime, Time = @Time
          WHERE id = @Id",
                    new
                    {
                        Date = date,
                        StartTime = startingTime.ToString("HH:mm"),
                        EndTime = endingTime.ToString("HH:mm"),
                        Time = time,
                        Id = id
                    });

                if (rowsAffected > 0)
                    AnsiConsole.MarkupLine($"[green]Updated session {id}.[/]");
                else
                    AnsiConsole.MarkupLine("[yellow]Nothing was updated.[/]");
            });
        }
        static private int SelectSessionId(string action)
        {
            Display();
            var sessions = GetAllSessionIds();
            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
                return -1;
            }

            return AnsiConsole.Prompt(
                new TextPrompt<int>($"Enter the Id of the session to {action}:")
                    .Validate(input => sessions.Contains(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]No session with that Id.[/]")));
        }

    }
}
