using System.Configuration;
using System.Data.SQLite;
using CodingModel.HopelessCoding;
using Dapper;
using Helpers.HopelessCoding;
using Spectre.Console;
using ValidationChecks.HopelessCoding;

namespace DbHelpers.HopelessCoding;

public class DatabaseHelpers
{
    public static string connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;

    public static void InitializeDatabase()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string createTableQuery =
                        @"CREATE TABLE IF NOT EXISTS coding_sessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration TEXT
                        )";

            connection.Execute(createTableQuery);
        }
    }

    internal static bool DateTimeAlreadyExists(string startTime, string id)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            string checkQuery = @"SELECT COUNT(*) FROM coding_sessions WHERE StartTime = @StartTime";

            if (!string.IsNullOrEmpty(id))
            {
                checkQuery += " AND Id != @Id";
            }

            var parameters = new { Id = id, StartTime = startTime };
            var count = connection.ExecuteScalar<int>(checkQuery, parameters);

            return count > 0;
        }
    }

    internal static bool IdExists(int id)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            string checkQuery = @"SELECT count(*) FROM coding_sessions WHERE Id = @Id";

            var count = connection.ExecuteScalar<int>(checkQuery, new { Id = id });

            return count > 0;
        }
    }

    internal static (string, string) GetTimeInput(string id)
    {
        string startTime = Validations.ValidateTime("start");

        if (DateTimeAlreadyExists(startTime, id))
        {
            AnsiConsole.Write(new Markup($"[red]\nRecord for the given start time already exists.[/]\n"));
            Console.WriteLine("----------------------------");
            return GetTimeInput(id);
        }

        string endTime = Validations.ValidateEndTimeNotBeforeStartTime(startTime);

        return (startTime, endTime);
    }

    internal static void ListRecords(string query, object parameters, string title)
    {
        Console.Clear();

        using (var connection = new SQLiteConnection(DatabaseHelpers.connectionString))
        {
            List<CodingSession> codingSessions = connection.Query<CodingSession>(query, parameters).ToList();

            GeneralHelpers.ShowConsoleTable(codingSessions, title);
        }
    }
}