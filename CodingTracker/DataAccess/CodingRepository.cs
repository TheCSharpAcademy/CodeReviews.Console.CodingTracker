using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Collections.Generic;
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
        using (var conn = _databaseManager.GetConnection())
        {
            conn.Execute(query, codingSession);
        }
    }

    public List<CodingSession> GetSessions()
    {
        string query = "SELECT * FROM codingtracker";
        using (var conn = _databaseManager.GetConnection())
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            return conn.Query<CodingSession>(query).ToList();
        }
    }

    public void InsertTestData(int number)
    {
        for (int i = 0; i <= number; i++)
        {
            DateTime startDate;
            DateTime endDate;

            do
            {
                startDate = Utilities.GenerateRandomDate();
                endDate = Utilities.GenerateRandomDate();
            } while (endDate <= startDate);

            CodingSession codingSession = new CodingSession
            {
                StartTime = startDate,
                EndTime = endDate,
                Duration = endDate - startDate
            };

            AddSession(codingSession);
        }
    }
}