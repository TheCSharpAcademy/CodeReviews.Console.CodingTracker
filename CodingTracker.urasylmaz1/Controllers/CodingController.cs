using CodingTracker.urasylmaz1.Helpers;
using CodingTracker.urasylmaz1.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.urasylmaz1.Controllers
{
    public class CodingController
    {
        private readonly string _connectionString;
        public CodingController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Coding Tracker[/]")
                        .AddChoices(
                            "Add Session",
                            "View Sessions",
                            "Delete Session",
                            "Exit"
                        )
                );

                switch (choice)
                {
                    case "Add Session":
                        AddSession();
                        break;

                    case "View Sessions":
                        ReadSession();
                        break;

                    case "Delete Session":
                        RemoveSession();
                        break;

                    case "Exit":
                        running = false;
                        break;
                }

                if (running)
                {
                    AnsiConsole.MarkupLine(
                        "\n[yellow]Press any key to continue...[/]"
                    );

                    Console.ReadKey();
                }
            }
        }

        public void AddSession()
        {
            DateTime start =
                UserInput.GetDateTime(
                    "Enter start time (yyyy-MM-dd HH:mm):"
                );

            DateTime end =
                UserInput.GetDateTime(
                    "Enter end time (yyyy-MM-dd HH:mm):"
                );

            string duration = (end - start).ToString();

            CodingSession session = new()
            {
                StartTime = start,
                EndTime = end,
                Duration = duration
            };

            using var connection =
                new SqliteConnection(_connectionString);

            string sql = @"
            INSERT INTO CodingSessions
            (StartTime, EndTime, Duration)
            VALUES
            (@StartTime, @EndTime, @Duration)
        ";

            connection.Execute(sql, session);
        }

        public void ReadSession()
        {
            using var connection = new SqliteConnection(_connectionString);
            List<CodingSession> sessions = connection.Query<CodingSession>("SELECT * FROM CodingSessions").AsList();
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Duration");

            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.StartTime.ToString(),
                    session.EndTime.ToString(),
                    session.Duration.ToString()
                );
            }

            AnsiConsole.Write(table);
        }

        public void RemoveSession()
        {
            ReadSession();

            int id = AnsiConsole.Ask<int>("Enter session Id to delete:");

            bool exists = Validation.SessionExists(_connectionString, id );

            if (!exists)
            {
                AnsiConsole.MarkupLine("[red]Session not found.[/]" );
                return;
            }

            using var connection = new SqliteConnection(_connectionString);

            string sql ="DELETE FROM CodingSessions WHERE Id = @Id";

            connection.Execute(sql, new { Id = id });

            AnsiConsole.MarkupLine("[green]Session deleted.[/]");
        }
    }
}
