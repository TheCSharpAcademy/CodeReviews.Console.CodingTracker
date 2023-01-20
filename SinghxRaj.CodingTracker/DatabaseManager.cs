using System.Configuration;
using Microsoft.Data.Sqlite;

namespace SinghxRaj.CodingTracker;

internal class DatabaseManager
{
    private static string ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
    private const int SucessfullyAddedRow = 1;

    public static void CreateTable()
    {
        string createTable = @"CREATE TABLE IF NOT EXISTS CODING_TRACKER (
                                      Id INTEGER PRIMARY KEY AUTO INCREMENT
                                      Start DATEIME,
                                      End DATETIME,
                                      Duration INTEGER";

        using var connection = new SqliteConnection(ConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = createTable;
        command.ExecuteNonQuery();

    }

    public static bool AddNewCodingSession(CodingSession session)
    {
        int rowsAdded;

        string start = session.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
        string end = session.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
        int durationInSeconds = (int)session.Duration.TotalSeconds;

        string newSession = @$"INSERT INTO CODING_TRACKER (Start, End, Duration)
                                  VALUES ({ start }, { end }, { durationInSeconds }";

        using var connection = new SqliteConnection(ConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = newSession;
        rowsAdded = command.ExecuteNonQuery();
        return rowsAdded == SucessfullyAddedRow;

    }

    public static List<CodingSession> GetCodingSessions()
    {
        string getSessions = @"SELECT * FROM CODING_TRACKER;";
        var sessions = new List<CodingSession>();

        using var connection = new SqliteConnection(ConnectionString);
        using var command = connection.CreateCommand();
        command.CommandText = getSessions;
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var start = reader.GetDateTime(1);
            var end = reader.GetDateTime(2);
            int durationInSeconds = reader.GetInt32(3);
            var duration = TimeSpan.FromSeconds(durationInSeconds);

            sessions.Add(new CodingSession(id, start, end, duration));
        }
        return sessions;
    }
}
