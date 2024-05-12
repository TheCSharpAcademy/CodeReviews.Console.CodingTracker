using CodingTracker.Models;
using Dapper;

namespace CodingTracker.Controllers;

public class CrudManager
{
    public static void InsertSqlRecord()
    {
        using (var connection = DbBuilder.GetConnection())
        {
            connection.Open();
            CodingSession session = HelpersValidation.GetSessionData();

            if (session.StartTime != "" || session.EndTime != "")
            {
                var sql =
                    $"INSERT INTO coding_tracker (startTime, endTime, duration)" +
                    $"VALUES ('{session.StartTime}', '{session.EndTime}', '{session.Duration}')";

                var rowsAffected = connection.Execute(sql);
                Console.WriteLine($"\n{rowsAffected} row(s) inserted.");
            }
        }
    }

    public static List<CodingSession> GetAllSessions()
    {
        List<CodingSession> sessionData = new();
        using (var connection = DbBuilder.GetConnection())
        {
            connection.Open();
            var reader = connection.ExecuteReader("SELECT id, startTime, endTime FROM coding_tracker");

            while (reader.Read())
            {
                string id = reader.GetString(0);
                string startTime = reader.GetString(1);
                string endTime = reader.GetString(2);

                sessionData.Add(new CodingSession(id, startTime, endTime));
            }
        }

        return sessionData;
    }
    
    public static List<CodingSession> GetSummarySessions()
    {
        List<CodingSession> sessionData = new();
        using (var connection = DbBuilder.GetConnection())
        {
            connection.Open();
            var reader = connection.ExecuteReader(
                                    @"SELECT * FROM(
                                        SELECT id, startTime, endTime FROM coding_tracker ORDER BY id ASC LIMIT 3) a
                                        UNION
                                        SELECT * FROM(
                                        SELECT id, startTime, endTime FROM coding_tracker ORDER BY id DESC LIMIT 3) b");


            while (reader.Read())
            {
                string id = reader.GetString(0);
                string startTime = reader.GetString(1);
                string endTime = reader.GetString(2);

                sessionData.Add(new CodingSession(id, startTime, endTime));
            }
        }

        return sessionData;
    }
    
    public static List<CodingSession> GetFilteredSessions(string startDate, string endDate)
    {
        List<CodingSession> sessionData = new();
        using (var connection = DbBuilder.GetConnection())
        {
            connection.Open();
            var reader = connection.ExecuteReader(@$"SELECT * FROM coding_tracker
                                                        WHERE startTime > '{startDate} 00:00'
                                                        AND endTime < '{endDate} 23:59'");


            while (reader.Read())
            {
                string id = reader.GetString(0);
                string startTime = reader.GetString(1);
                string endTime = reader.GetString(2);

                sessionData.Add(new CodingSession(id, startTime, endTime));
            }
        }

        return sessionData;
    }
}