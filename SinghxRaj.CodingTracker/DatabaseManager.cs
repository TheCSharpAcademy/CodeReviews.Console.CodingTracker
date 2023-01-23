using System.Configuration;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace SinghxRaj.CodingTracker;

internal class DatabaseManager
{
    private static string ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    private const int SUCCESSFULLY_CHANGED_ROW = 1;

    public static void CreateTable()
    {
        string createTable = @"CREATE TABLE IF NOT EXISTS CODING_TRACKER(
                                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      Start TEXT,
                                      End TEXT,
                                      Duration INTEGER
                                     )";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = createTable;
        command.ExecuteNonQuery();

    }

    public static bool AddNewCodingSession(CodingSession session)
    {
        int rowsAdded;

        string start = session.StartTime.ToString(TimeFormat.SessionTimeStampFormat);
        string end = session.EndTime.ToString(TimeFormat.SessionTimeStampFormat);
        int durationInMinutes = (int)session.Duration.TotalMinutes;

        string newSession = @$"INSERT INTO CODING_TRACKER (Start, End, Duration)
                                  VALUES ('{start}', '{end}', {durationInMinutes})";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = newSession;
        rowsAdded = command.ExecuteNonQuery();
        return rowsAdded == SUCCESSFULLY_CHANGED_ROW;
    }

    public static List<CodingSession> GetCodingSessions()
    {
        string getSessions = @"SELECT * FROM CODING_TRACKER";
        var sessions = new List<CodingSession>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = getSessions;
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string startStr = reader.GetString(1);
            string endStr = reader.GetString(2);
            int durationInMinutes = reader.GetInt32(3);
            TimeSpan duration = TimeSpan.FromMinutes(durationInMinutes);

            bool parseStart = DateTime.TryParseExact(startStr, TimeFormat.SessionTimeStampFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start);

            bool parseEnd = DateTime.TryParseExact(endStr, TimeFormat.SessionTimeStampFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end);

            if (parseStart && parseEnd)
            {
                sessions.Add(new CodingSession(id, start, end, duration));
            }

        }
        return sessions;
    }

    internal static bool DeleteCodingSession(int codingSessionId)
    {
        string deleteSession = @$"DELETE FROM CODING_TRACKER WHERE Id={codingSessionId}";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = deleteSession;

        int rowsDeleted = command.ExecuteNonQuery();

        return rowsDeleted == SUCCESSFULLY_CHANGED_ROW;
    }

    internal static bool UpdateCodingSession(CodingSession session)
    {
        string start = session.StartTime.ToString(TimeFormat.SessionTimeStampFormat);
        string end = session.EndTime.ToString(TimeFormat.SessionTimeStampFormat);
        int durationInMinutes = (int)session.Duration.TotalMinutes;

        string updateSession = @$"UPDATE CODING_TRACKER
                                  SET Start='{start}', End='{end}', Duration={durationInMinutes}
                                  WHERE Id={session.Id}";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = updateSession;

        int updatedRows = command.ExecuteNonQuery();

        return updatedRows == SUCCESSFULLY_CHANGED_ROW;
    }

    internal static CodingSession GetCodingSession(int sessionId)
    {
        string getSessions = @$"SELECT * FROM CODING_TRACKER
                               WHERE Id = {sessionId}";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = getSessions;
        var reader = command.ExecuteReader();

        reader.Read();
        int id = reader.GetInt32(0);
        string startStr = reader.GetString(1);
        string endStr = reader.GetString(2);
        int durationInMinutes = reader.GetInt32(3);
        TimeSpan duration = TimeSpan.FromMinutes(durationInMinutes);

        bool parseStart = DateTime.TryParseExact(startStr, TimeFormat.SessionTimeStampFormat,
        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start);

        bool parseEnd = DateTime.TryParseExact(endStr, TimeFormat.SessionTimeStampFormat,
        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end);

        if (parseStart && parseEnd)
        {
            return new CodingSession(id, start, end, duration);
        }

        return null!;
    }
}
