using CyrillParfen.CodingTracker.Exceptions;
using CyrillParfen.CodingTracker.Helpers;
using CyrillParfen.CodingTracker.Model;
using CyrillParfen.CodingTracker.Services;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CyrillParfen.CodingTracker.Data;

internal class DataAccess
{
    private readonly string _connectionString;

    public DataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateTable()
    {
        string sqlCreateCodingTrackerTable =
            @"CREATE TABLE IF NOT EXISTS CodingSession (
                Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL
                )";

        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(sqlCreateCodingTrackerTable);
        }
    }

    public void AddCodingSession()
    {
        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            string sqlCommand =
                @"INSERT INTO CodingSession (StartTime, EndTime)
                    VALUES (@StartTime, @EndTime)";

            try
            {
                DateTime startDate = DateHelper.GetValidDate("Enter start of the session:");
                DateTime endDate = DateHelper.GetValidDate("Enter the end of the session:");
                while (!DateHelper.IsStartBeforeEnd(startDate, endDate))
                {
                    endDate = DateHelper.GetValidDate("End date cannot be before start date." +
                        " Please enter valid date.");
                }

                int checkCount = connection.Execute(sqlCommand, new CodingSession
                {
                    StartTime = startDate,
                    EndTime = endDate
                });

                if (checkCount > 0)
                {
                    AnsiConsole.MarkupLine("[green]A new record has been added[/]");
                    AnsiConsole.MarkupLine("[Blink]Press any key to continue...[/]");
                    Console.ReadKey(intercept: true);
                }

            }
            catch (ReturnToMainMenuException) { }
        }
    }

    public void DeleteCodingSession()
    {
        var records = GetAllRecord();
        UserPrompts.PrintRecords(records);

        int id = InputValidation.GetNumberInput("Enter record ID you want to delete...");
        var sessionToDelete = SelectById(id);

        if (UserPrompts.IsSessionMissing(sessionToDelete, id)) return;

        bool isSureToDelete = UserPrompts.ActionConfirmation(
            $"You're going to delete record with ID:{id}\n" +
            $"Are you sure?");

        if (!isSureToDelete) return;

        string sqlCommand = "DELETE FROM CodingSession WHERE Id = @Id";

        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            int checkCount = connection.Execute(sqlCommand, new { Id = id });

            AnsiConsole.MarkupLine(checkCount > 0
                ? $"[green]Record with ID:{id} has been deleted![/]"
                : $"[red]No session found with the Id {id}[/]");

            AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
            Console.ReadKey(intercept: true);
        }
    }

    public void UpdateCodingSession()
    {
        string sqlCommand = "UPDATE CodingSession SET StartTime = @StartTime, EndTime = @EndTime WHERE Id = @Id";

        var records = GetAllRecord();
        UserPrompts.PrintRecords(records);

        int id = InputValidation.GetNumberInput("Enter record ID you want to update:");

        var recordToUpdate = SelectById(id);

        if (UserPrompts.IsSessionMissing(recordToUpdate, id)) return;

        AnsiConsole.Clear();
        UserPrompts.PrintRecords(recordToUpdate);

        DateTime startDate = DateHelper.GetValidDate("Enter start of the session: ");
        DateTime endDate = DateHelper.GetValidDate("Enter end of the session: ");

        while (!DateHelper.IsStartBeforeEnd(startDate, endDate))
        {
            endDate = DateHelper.GetValidDate("End date cannot go before start date." +
                " Please enter valid date.");
        }

        bool isSureToUpdate = UserPrompts.ActionConfirmation(
            $"You're going to update record with ID:{id}\n" +
            $"Are you sure?");

        if (!isSureToUpdate) return;

        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            int checkCount = connection.Execute(sqlCommand, new
            {
                StartTime = startDate,
                EndTime = endDate,
                Id = id
            });

            AnsiConsole.MarkupLine(checkCount > 0
                ? $"[green]The coding session with ID:{id} has been updated[/]"
                : $"[red]No session found with the Id {id}[/]");

            AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
            Console.ReadKey(intercept: true);
        }
    }

    internal List<CodingSession> GetAllRecord()
    {
        string sqliteCommand = "SELECT Id, StartTime, EndTime FROM CodingSession";

        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            List<CodingSession> sessions = connection.Query<CodingSession>(sqliteCommand).ToList();

            return sessions;
        }
    }

    internal CodingSession? SelectById(int id)
    {
        string sql = "SELECT Id, StartTime, EndTime FROM CodingSession WHERE Id = @Id";

        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            return connection.QuerySingleOrDefault<CodingSession>(sql, new { Id = id });
        }
    }

    internal void InitiateStopwatchSession()
    {
        CodingStopwatch codingStopwatch = new CodingStopwatch();
        codingStopwatch.Start();

        CodingSession codingSession = codingStopwatch.Stop();
        codingStopwatch.ShowStopwatchResult(codingSession);

        var isSureToSave = UserPrompts.ActionConfirmation("Save session?");
        if (!isSureToSave) return;

        string sql = "INSERT INTO CodingSession (StartTime, EndTime)" +
            "VALUES (@StartTime, @EndTime)";

        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            int checkCount = connection.Execute(sql, codingSession);

            if (checkCount > 0) AnsiConsole.MarkupLine("[green]Session saved![/]");

            AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
            Console.ReadKey(intercept: true);
        }
    }

    internal List<CodingSession> GetFilteredSelection(DateTime? from, DateTime? to, bool ascending)
    {
        var (sql, parameters) = FilterQueryBuilder(from, to, ascending);

        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            List<CodingSession> codingSessions = connection.Query<CodingSession>(sql, parameters).ToList();

            return codingSessions;
        }
    }

    internal static (string sql, object parameters) FilterQueryBuilder(DateTime? from, DateTime? to, bool ascending)
    {
        string sql = "SELECT Id, StartTime, EndTime FROM CodingSession WHERE 1 = 1";

        if (from != null)
            sql += " AND StartTime >= @From";

        if (to != null)
            sql += " AND StartTime <= @To";

        sql += ascending
            ? " ORDER BY StartTime ASC"
            : " ORDER BY StartTime DESC";

        return (sql, new { From = from, To = to });
    }
}
