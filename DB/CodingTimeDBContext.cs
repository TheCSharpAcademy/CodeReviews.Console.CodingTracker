using Dapper;
using Microsoft.Data.Sqlite;
using Models.Dtos;
using Models.Entities;

namespace DB;

public class CodingTimeDBContext(string sqlConnectionString)
{
    private readonly string sqlConnectionString = sqlConnectionString;

    public void SeedDatabase()
    {
        using var conn = new SqliteConnection(sqlConnectionString);
        conn.Open();

        var sql = @"
            CREATE TABLE IF NOT EXISTS codingTimes(
                id INTEGER PRIMARY KEY,
                task TEXT,
                startTime TEXT,
                endTime TEXT 
            );
        ";

        conn.Execute(sql);

        sql = "SELECT * FROM codingTimes";

        var codingTimes = conn.Query(sql);

        if (!codingTimes.Any())
        {
            sql = @"
                INSERT INTO codingTimes
                    (task, startTime, endTime)
                VALUES
                    ('Coding Tracker', '0930 22-07-24', '1230 22-07-24'),
                    ('Coding Tracker', '0930 21-07-24', '1230 21-07-24'),
                    ('Coding Tracker', '0930 20-07-24', '1230 20-07-24');
            ";

            conn.Execute(sql);
        }
    }

    public void CreateCodingTime(CreateCodingTimeDto codingTime)
    {
        using var conn = new SqliteConnection(sqlConnectionString);
        conn.Open();

        var sql = @"
            INSERT INTO codingTimes
                (task, startTime, endTime)
            VALUES
                (@Task, @StartTime, @EndTime);
        ";

        object[] parameters = { new { Task = codingTime.Task, StartTime = codingTime.StartTime, EndTime = codingTime.EndTime } };

        conn.Execute(sql, parameters);

        conn.Close();
    }

    // read

    // update

    // delete
}
