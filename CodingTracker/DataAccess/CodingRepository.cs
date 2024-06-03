using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

public class CodingRepository
{
    private DatabaseManager _databaseManager;

    public CodingRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void AddSession(CodingSession codingSession)
    {
        try
        {
            string query = "INSERT INTO codingtracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
            using (var conn = _databaseManager.GetConnection()){
                conn.Execute(query, codingSession);
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine("Error inserting into database: " + e);
        }
    }
}