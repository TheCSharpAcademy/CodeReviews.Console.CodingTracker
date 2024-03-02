using System.Configuration;
using System.Data.Common;
using codingTracker.Fennikko.Models;
using Dapper;
using Microsoft.Data.Sqlite;

public class CodingController
{
    private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("connectionString") +
                                                      ConfigurationManager.AppSettings.Get("databasePath");

    public static void DatabaseCreation()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Execute(
            """
            CREATE TABLE IF NOT EXISTS coding_tracker (
                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                 StartTime TEXT,
                 EndTime TEXT,
                 Duration TEXT
                 )               
            """);
    }

    public static void Session()
    {

    }

    public static void GetAllSessions()
    {

    }

    public static void TableCreation(IEnumerable<CodingSession> sessions)
    {

    }
}