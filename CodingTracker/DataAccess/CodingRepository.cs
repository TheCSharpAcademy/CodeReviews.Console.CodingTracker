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
        for (int i = 0; i < number; i++)
        {
            DateTime startDate;
            DateTime endDate;
            TimeSpan duration;

            do
            {
                startDate = Utilities.GenerateRandomDate();
                duration = new TimeSpan(Utilities.RandomNumber(0, 24), Utilities.RandomNumber(0, 60), Utilities.RandomNumber(0, 60));
                endDate = startDate.Add(duration);
            } while (endDate <= startDate || startDate >= DateTime.Now || duration.TotalHours >= 6);

            CodingSession codingSession = new CodingSession
            {
                StartTime = startDate,
                EndTime = endDate,
                Duration = endDate - startDate
            };

            AddSession(codingSession);

            AnsiConsole.WriteLine($"{i + 1}. Added recored to database: {codingSession.StartTime} - {codingSession.EndTime} - {codingSession.Duration}");

        }
    }
}