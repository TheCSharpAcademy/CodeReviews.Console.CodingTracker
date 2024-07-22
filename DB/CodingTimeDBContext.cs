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

        conn.Execute(sql, codingTime);

        conn.Close();
    }

    public List<CodingTime> GetAllCodingTimes()
    {
        using var conn = new SqliteConnection(sqlConnectionString);
        conn.Open();

        var sql = "SELECT * FROM codingTimes";

        var codingTimes = conn.Query<CodingTime>(sql);

        conn.Close();

        return codingTimes.AsList();
    }

    public void UpdateCodingTime(CodingTime codingTime)
    {
        using var conn = new SqliteConnection(sqlConnectionString);
        conn.Open();

        var sql = @"
            UPDATE codingTimes
            SET task = @Task,
                startTime = @StartTime,
                endTime = @EndTime
            WHERE
                id = @Id;
        ";

        conn.Execute(sql, codingTime);
        conn.Close();
    }

    public void DeleteCodingTime(int id)
    {
        using var conn = new SqliteConnection(sqlConnectionString);
        conn.Open();

        var sql = @"
            DELETE FROM codingTimes WHERE id=@Id;
        ";

        conn.Execute(sql, new { Id = id });
    }
}
