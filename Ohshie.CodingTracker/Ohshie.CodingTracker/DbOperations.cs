using Microsoft.Data.Sqlite;

namespace Ohshie.CodingTracker;

public class DbOperations
{
    public DbOperations()
    {
        if (EnsureDbExist() < 1)
        {
            CreateDb();
        }
    }

    private static readonly string DbConnection = @"Data Source=coding_tracker.db";
    
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
        List<Session> sessionsList = new();
        
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM 'CodingSessions'";

            var reader = tableCommand.ExecuteReader();
            
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
}