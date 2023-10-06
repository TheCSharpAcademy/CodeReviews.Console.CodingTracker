using CodingTracker.Models;
using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.Services;

public class LogQueries
{
    private static readonly string dbFile = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

    public static void InsertData(CodeSession? session)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        conn.Open();
        cmd.CommandText = "INSERT INTO logs(Date, StartTime, EndTime, Duration) VALUES ($Date, $Start, $End, $Duration)";
        cmd.Parameters.AddWithValue("$Date", session.TodaysDate);
        cmd.Parameters.AddWithValue("$Start", session.StartTime);
        cmd.Parameters.AddWithValue("$End", session.EndTime);
        cmd.Parameters.AddWithValue("$Duration", session.Duration);

        try
        {
            cmd.ExecuteNonQuery();
            Helpers.Finished(session, null, "Saved");
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void UpdateData(CodeSession? newSession)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        conn.Open();

        cmd.CommandText = "UPDATE logs SET Date=$date, StartTime=$start, EndTime=$end, Duration=$duration WHERE Id=$selectedId";
        cmd.Parameters.AddWithValue("$selectedId", newSession.Id);
        cmd.Parameters.AddWithValue("$date", newSession.TodaysDate);
        cmd.Parameters.AddWithValue("$start", newSession.StartTime);
        cmd.Parameters.AddWithValue("$end", newSession.EndTime);
        cmd.Parameters.AddWithValue("$duration", newSession.Duration);

        try
        {
            cmd.ExecuteNonQuery();
            Helpers.Finished(newSession, null, "Edited");
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void DeleteData(CodeSession? currentSession)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        conn.Open();
        cmd.CommandText = "DELETE FROM logs WHERE Id=$selectedId";
        cmd.Parameters.AddWithValue("$selectedId", currentSession.Id);
        
        try
        {
            cmd.ExecuteNonQuery();
            Helpers.Finished(currentSession, null, "Deleted");
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static List<CodeSession?>? GetAllCodingSessions()
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        SQLiteDataReader? reader;
        List<CodeSession?>? sessions = new();

        conn.Open();
        cmd.CommandText = "SELECT * FROM logs";

        reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            int? id = int.Parse(reader["Id"].ToString());
            string? date = reader["Date"].ToString();
            string? startTime = reader["StartTime"].ToString();
            string? endTime = reader["EndTime"].ToString();
            string? duration = reader["Duration"].ToString();

            CodeSession? sess = new()
            {
                Id = id,
                TodaysDate = date,
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration
            };

            sessions.Add(sess);
        }
        reader.Close();
        return sessions;
    }

    public static CodeSession? GetCodeSession(int? selectedId)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        SQLiteDataReader? reader;

        conn.Open();
        cmd.CommandText = "SELECT * FROM logs WHERE Id=$selectedId";
        cmd.Parameters.AddWithValue("$selectedId", selectedId);

        reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            int? id = int.Parse(reader["Id"].ToString());
            string? date = reader["Date"].ToString();
            string? startTime = reader["StartTime"].ToString();
            string? endTime = reader["EndTime"].ToString();
            string? duration = reader["Duration"].ToString();

            CodeSession? sess = new()
            {
                Id = id,
                TodaysDate = date,
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration
            };

            reader.Close();
            return sess;
        }
        else
        {
            return new CodeSession();
        }
    }
}
