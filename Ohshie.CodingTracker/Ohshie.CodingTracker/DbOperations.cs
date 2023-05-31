using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Ohshie.CodingTracker;

public class DbOperations
{
    public DbOperations()
    {
        if (EnsureDbExist() < 1)
        {
            CreateDb();
        }
        DbConnection = GetConnectionStringFromSettings();
    }

    private string? DbConnection { get; }
    
    public void CreateDb()
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = 
                $@"CREATE TABLE IF NOT EXISTS 'CodingSessions'
                    (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    SessionLength INTEGER,
                    Note TEXT)";

            tableCommand.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void NewSessionEntry(Session session)
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $@"INSERT INTO CodingSessions "+
                                            "(Date, SessionLength, Note) "+
                                            $"VALUES ('{session.Date}', '{session.Length}', '{session.Note}')";
            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public List<Session> FetchAllSessions()
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM 'CodingSessions'";

            var reader = tableCommand.ExecuteReader();
            
            var sessionsList = ReadFromDbToSessionsList(reader);

            return sessionsList;
        }
    }

    private List<Session> ReadFromDbToSessionsList(SqliteDataReader reader)
    {
        List<Session> sessionsList = new();
        int idCounter = 0;
        while (reader.Read())
        {
            sessionsList.Add(new Session
            {
                Id = ++idCounter,
                Date = reader.GetString(1),
                Length = reader.GetString(2),
                Note = reader.GetString(3)
            });
        }

        return sessionsList;
    }

    // tbh no idea wtf this code returns 1 even when i delete db while app is running.
    // i though it would be pretty nice way to ensure that db exist. I'll leave it here for now.
    private int EnsureDbExist()
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = "SELECT count(*) FROM sqlite_master " +
                                       "WHERE TYPE = 'table' " +
                                       "AND NAME = 'CodingSessions'";

            int dbExist = Convert.ToInt32(tableCommand.ExecuteScalar());
            
            return dbExist;
        }
    }

    private string? GetConnectionStringFromSettings()
    {
        var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        return configuration.GetConnectionString("SQLite");
    }
}