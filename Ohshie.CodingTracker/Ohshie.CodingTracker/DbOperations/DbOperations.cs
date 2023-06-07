using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Ohshie.CodingTracker.DbOperations;

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

    internal string? DbConnection { get; }
    
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
                    Description TEXT,
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
                                       "(Date, Description, SessionLength, Note) "+
                                       $"VALUES ('{session.Date}', '{session.Description}', '{session.Length}', '{session.Note}')";
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

    public void RemoveSession(Session session)
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "DELETE FROM 'CodingSessions' " +
                                       $"WHERE Id = {session.Id}";

            tableCommand.ExecuteNonQuery();
        }
    }

    public void WipeTable()
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "DELETE FROM 'CodingSessions'";

            tableCommand.ExecuteNonQuery();
        }
        
    }

    public Session FetchSession(int id)
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = "SELECT * FROM 'CodingSessions'" +
                                       $"Where Id = {id}";

            var reader = tableCommand.ExecuteReader();

            Session session = new();
            while (reader.Read())
            {
                session = new Session
                {
                    Id = int.Parse(reader.GetString(0)),
                    Date = reader.GetString(1),
                    Description = reader.GetString(2),
                    Length = reader.GetString(3),
                    Note = reader.GetString(4)
                };
            }

            return session;
        }
    }

    private List<Session> ReadFromDbToSessionsList(SqliteDataReader reader)
    {
        List<Session> sessionsList = new();
        while (reader.Read())
        {
            sessionsList.Add(new Session
            {
                Id = int.Parse(reader.GetString(0)),
                Date = reader.GetString(1),
                Description = reader.GetString(2),
                Length = reader.GetString(3),
                Note = reader.GetString(4)
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
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        return configuration.GetConnectionString("SQLite");
    }
}