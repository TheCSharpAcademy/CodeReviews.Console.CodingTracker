using Dapper;
using System.Data.SQLite;
using System.Xml;

namespace CodingTracker; 

internal class Database
{
    protected static string connectionString;
    public static IEnumerable<CodingSessionModel> ViewSessionRecords()
    {
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        IEnumerable<CodingSessionModel> sessions = connection.Query<CodingSessionModel>("SELECT * FROM CodingTracker ORDER BY SessionCodingDate ASC, SessionStartTime ASC");
        return sessions;
    }
    public static void InsertRecord(CodingSessionModel record)
    {
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        connection.Execute("INSERT INTO CodingTracker(SessionStartTime, SessionEndTime, SessionDuration, SessionCodingDate) VALUES (@SessionStartTime, @SessionEndTime, @SessionDuration, @SessionCodingDate)", new { SessionStartTime = record.SessionStartTime.ToString(), SessionEndTime = record.SessionEndTime.ToString(), SessionDuration = record.SessionDuration.ToString(), SessionCodingDate = record.SessionCodingDate.ToString() });
    }

    public static void DeleteRecord(int sessionIdToBeDeleted, bool isSessionValidated)
    {
        if (isSessionValidated)
        {
            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            connection.Execute("DELETE FROM CodingTracker WHERE SessionId = @sessionIdToBeDeleted", new { sessionIdToBeDeleted });
        }
    }

    public static void UpdateRecord(bool isSessionValidated, CodingSessionModel record, int sessionUpdateId)
    {
        if (isSessionValidated)
        {
            using SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string sql = @"UPDATE CodingTracker SET SessionStartTime = @SessionStartTime, SessionEndTime = @SessionEndTime, SessionDuration = @SessionDuration, SessionCodingDate = @SessionCodingDate WHERE SessionId = @SessionId";
            int rowsAffected = connection.Execute(sql, new
            {
                record.SessionStartTime,
                record.SessionEndTime,
                record.SessionDuration,
                record.SessionCodingDate,
                SessionId = sessionUpdateId
            });
        }
    }

    public static void CreateDatabase()
    {
        try
        {
            string filePath = "./Appsettings.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            string dbPath = xmlDoc.SelectSingleNode("/database/name").InnerText;
            connectionString = xmlDoc.SelectSingleNode("/database/connectionString").InnerText;
            if (!File.Exists(dbPath))
            {
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                connection.Execute(@"CREATE TABLE CodingTracker (SessionId INTEGER PRIMARY KEY, SessionStartTime TEXT, SessionEndTime TEXT, SessionDuration TEXT,SessionCodingDate TEXT)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public static bool IsGivenSessionIdPresent(int sessionId)
    {
        int count;
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM CodingTracker WHERE SessionId = @Id", new { Id = sessionId });
        if (count <= 0)
            return false;
        return true;
    }

    public static bool IsGivenSessionIdPresentForInputDate(int sessionId, string CodingDate)
    {
        int count;
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM CodingTracker WHERE SessionId = @Id AND SessionCodingDate = @Date", new { Id = sessionId, Date = CodingDate });
        if (count <= 0)
            return false;
        return true;
    }

    public static List<CodingSessionModel> GetRecordsByDate(string sessionCodingDate)
    {
        List<CodingSessionModel> records = new List<CodingSessionModel>();
        using SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM CodingTracker WHERE SessionCodingDate = @SessionCodingDate ORDER BY SessionStartTime ASC";
        var result = connection.Query<CodingSessionModel>(sql, new { SessionCodingDate = sessionCodingDate });
        records.AddRange(result);
        return records;
    }
}