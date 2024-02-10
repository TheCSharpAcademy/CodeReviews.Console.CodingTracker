using Buutyful.Coding_Tracker.Models;
using Spectre.Console;
using System.Data.SQLite;
using System.Globalization;

namespace Buutyful.Coding_Tracker;

public class DbAccess
{
    private readonly string? _connectionString = Constants.ConnectionString;

    public void CreateDatabase()
    {
        using SQLiteConnection connection = new(_connectionString);
        connection.Open();

        string createTableQuery = "CREATE TABLE IF NOT EXISTS coding_tracker (" +
            "Id INTEGER PRIMARY KEY," +
            "Guid TEXT, " +
            "StartAt DATETIME," +
            "EndAt DATETIME," +
            "Duration INTEGER)";
        using (SQLiteCommand command = new(createTableQuery, connection))
        {
            command.ExecuteNonQuery();
        }

        connection.Close();
    }
    public void Create(CodingSession session)
    {
        using SQLiteConnection connection = new(_connectionString);
        connection.Open();

        string insertQuery = $"INSERT INTO coding_tracker (Guid, StartAt, EndAt, Duration) VALUES " +
            $"('{session.Id}', '{session.StartAt}', '{session.EndAt}', '{session.Duration}')";

        using (SQLiteCommand command = new(insertQuery, connection))
        {
            command.ExecuteNonQuery();
        }

        connection.Close();
        AnsiConsole.MarkupLine($"[green]Created[/]: {session.Id}");
    }
    public List<CodingSession> Get()
    {
        var sessions = new List<CodingSession>();
        using (SQLiteConnection connection = new(_connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM coding_tracker";
            using (SQLiteCommand command = new(selectQuery, connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    try
                    {
                        Guid id = reader.GetGuid(reader.GetOrdinal("Guid"));                     


                        string durationString = reader.GetString(reader.GetOrdinal("Duration"));

                        if (DateTime.TryParse(reader.GetString(reader.GetOrdinal("StartAt")), out var startAt) &&
                        DateTime.TryParse(reader.GetString(reader.GetOrdinal("EndAt")), out var endAt) &&
                        TimeSpan.TryParse(durationString, out var duration))
                        {
                            sessions.Add(CodingSession.Map(id, startAt, endAt, duration));
                        }
                        else
                        {
                            Console.WriteLine($"Error parsing duration for session with Id {id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            connection.Close();
        }

        return sessions;
    }
    public CodingSession? GetById(Guid id)
    {
        using SQLiteConnection connection = new(_connectionString);
        connection.Open();

        string selectQuery = $"SELECT * FROM coding_tracker WHERE Guid = '{id}'";

        using (SQLiteCommand command = new(selectQuery, connection))
        {
            using SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                try
                {
                    Guid sessionId = reader.GetGuid(reader.GetOrdinal("Guid"));

                    DateTime startAt = DateTime.ParseExact(
                        reader.GetString(reader.GetOrdinal("StartAt")),
                        "dd/MM/yyyy HH:mm:ss",
                        CultureInfo.InvariantCulture);

                    DateTime endAt = DateTime.ParseExact(
                        reader.GetString(reader.GetOrdinal("EndAt")),
                        "dd/MM/yyyy HH:mm:ss",
                        CultureInfo.InvariantCulture);

                    string durationString = reader.GetString(reader.GetOrdinal("Duration"));
                    TimeSpan duration = TimeSpan.ParseExact(durationString, @"d\.hh\:mm\:ss", CultureInfo.InvariantCulture);

                    return CodingSession.Map(sessionId, startAt, endAt, duration);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        connection.Close();

        return null;
    }
    public void Delete(Guid sessionId)
    {
        using SQLiteConnection connection = new(_connectionString);
        connection.Open();

        string deleteQuery = $"DELETE FROM coding_tracker WHERE Guid = '{sessionId}'";

        using SQLiteCommand command = new(deleteQuery, connection);
        int rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            AnsiConsole.MarkupLine($"Id {sessionId} [yellow]not found[y]");
        }
        else
        {
            AnsiConsole.MarkupLine($"Id {sessionId} [green]deleted successfully[/].");
        }
        connection.Close();
    }
}