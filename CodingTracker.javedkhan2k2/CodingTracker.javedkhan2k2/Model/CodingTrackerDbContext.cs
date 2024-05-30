using Dapper;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace CodingTracker.Models;

internal class CodingTrackerDbContext
{
    private string ConnectionString { get; init; }

    public CodingTrackerDbContext(string connectionString)
    {
        ConnectionString = connectionString;
        InitDatabase();
        //SeedData();
    }

    private void InitDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"Create Table If Not Exists CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration INTEGER NOT NULL
            )";
            connection.Execute(sql);
        }
        //SeedData();
    }

    private void SeedData()
    {
        // AddSessionCode("2024-02-16", "2024-02-16", 1000);
        // AddSessionCode("2024-02-17", "2024-02-17", 95000);
        // AddSessionCode("2024-02-18", "2024-02-18", 35000);
        // AddSessionCode("2024-02-19", "2024-02-19", 62000);
        // AddSessionCode("2024-02-20", "2024-02-20", 16000);
    }

    // internal void AddSessionCode(string startTime, string endTime, long durationInSeconds)
    // {
    //     using (var connection = new SqliteConnection(ConnectionString))
    //     {
    //         connection.Open();
    //         var sql = @"insert into CodingSessions (StartTime, EndTime, Duration)
    //                     Values 
    //                         (@StartTime, @EndTime, @Duration)
    //                 ";
    //         var parameters = new { StartTime = startTime, EndTime = endTime, Duration = durationInSeconds };
    //         int result = connection.Execute(sql, parameters);
    //         if (result == 0)
    //         {
    //             Console.WriteLine($"Record could not created");
    //         }
    //         else
    //         {
    //             Console.WriteLine($"Record created successfully.");
    //         }
    //     }
    // }

    internal void AddSessionCode(CodingSessionDto codingSession)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"insert into CodingSessions (StartTime, EndTime, Duration)
                        Values 
                            (@StartTime, @EndTime, @Duration)
                    ";
            //var parameters = new { StartTime = startTime, EndTime = endTime, Duration = durationInSeconds };
            int result = connection.Execute(sql, codingSession);
            if (result == 0)
            {
                Console.WriteLine($"Record could not created");
            }
            else
            {
                Console.WriteLine($"Record created successfully.");
            }
        }
    }

    internal bool UpdateSessionCode(int id, CodingSessionDto codingSession)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"Update CodingSessions
                    Set 
                        StartTime = @StartTime,
                        EndTime = @EndTime,
                        Duration = @Duration
                    where
                        Id = @Id;
                  ";
            var parameters = new { Id = id, StartTime = codingSession.StartTime, EndTime = codingSession.EndTime, Duration = codingSession.Duration };
            int result = connection.Execute(sql, parameters);
            return result == 0 ? false : true;
        }
    }

    internal bool DeleteSessionCode(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"Delete from CodingSessions
                            Where Id = @Id
                       ";
            var parameters = new { Id = id };
            int result = connection.Execute(sql, parameters);
            return result == 0 ? false : true;
        }
    }

    internal IEnumerable<CodingSession>? GetAllCodingSessions()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            
            var sql = "SELECT Id, StartTime, EndTime, Duration AS DurationInSeconds FROM CodingSessions";
            IEnumerable<CodingSession>? codingSessions = connection.Query<CodingSession>(sql);
            return codingSessions;
        }
    }

    internal CodingSession? GetCodingSessionById(int id)
    {
        CodingSession? codingSession;
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"SELECT Id, StartTime, EndTime, Duration AS DurationInSeconds FROM CodingSessions Where Id = @Id";
            var parameters = new { Id = id };
            codingSession = connection.QueryFirstOrDefault<CodingSession>(sql, parameters);
        }

        return codingSession;
    }

}