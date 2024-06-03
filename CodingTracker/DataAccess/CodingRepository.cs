using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Collections.Generic;
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
        string query = "INSERT INTO codingtracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
        using (var conn = _databaseManager.GetConnection()){
            conn.Execute(query, codingSession);
        }
    }

    public List<CodingSession> GetSessions()
    {
        string query = "SELECT * FROM codingtracker";
        using (var conn = _databaseManager.GetConnection())
        {
            return conn.Query<CodingSession>(query).ToList();
        }
    }
}