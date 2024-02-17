using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace Doc415.CodingTracker;

internal class DataAccess
{
    IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

    private string ConnectionString;

    public DataAccess()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }
    internal void CreateDatabase()
    {   
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS records(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DateStart TEXT NOT NULL,
                    DateEnd TEXT NOT NULL)";
            connection.Execute(createTableQuery);
        }
        CreateGoalsTable();
    }

    internal void CreateGoalsTable()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string addGoalsTable = @"CREATE TABLE IF NOT EXISTS goals(
                            startDate TEXT NOT NULL,
                            endDate TEXT NOT NULL,
                            codingGoal TEXT NOT NULL,
                            Id INTEGER PRIMARY KEY AUTOINCREMENT)";
            connection.Execute(addGoalsTable);
        }

    }
    internal void InsertRecord(CodingRecord record)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string insertQuery = @"
        INSERT INTO records (DateStart, DateEnd)
        VALUES (@DateStart, @DateEnd)";

            connection.Execute(insertQuery, new { record.DateStart, record.DateEnd });
        }
    }
    internal IEnumerable<CodingRecord> GetAllRecords()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM records";

            var records = connection.Query<CodingRecord>(selectQuery);

            foreach (var record in records)
            {
                record.Duration = record.DateEnd - record.DateStart;
            }

            return records;
        }
    }

    internal void UpdateRecord(CodingRecord updatedRecord)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string updateQuery = @"
     UPDATE records
     SET DateStart = @DateStart, DateEnd = @DateEnd
     WHERE Id = @Id";

            connection.Execute(updateQuery, new { updatedRecord.DateStart, updatedRecord.DateEnd, updatedRecord.Id });
        }
    }

    internal void DeleteRecord(int _Id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string deleteQuery = @"
     DELETE FROM records WHERE Id = @Id";

            connection.Execute(deleteQuery, new { Id = _Id });
        }
    }

    internal IEnumerable<CodingRecord> GetRecordsBetween(DateTime _startDate, DateTime _endDate)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM records WHERE DateStart>=@startDate AND DateStart<@endDate";

            var records = connection.Query<CodingRecord>(selectQuery, new { startDate = _startDate, endDate = _endDate });

            foreach (var record in records)
            {
                record.Duration = record.DateEnd - record.DateStart;
            }

            return records;
        }
    }


    internal void BulkInsertRecords(List<CodingRecord> records)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            // Prepare the query with placeholders for multiple records
            string insertQuery = @"
        INSERT INTO records (DateStart, DateEnd)
        VALUES (@DateStart, @DateEnd)";

            // Execute the query for each record in the collection
            connection.Execute(insertQuery, records.Select(record => new
            {
                record.DateStart,
                record.DateEnd
            }));
        }
    }

    internal void AddGoal(Goal _goal)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string insertGoal = @"
        INSERT INTO goals (startDate, endDate, codingGoal)
        VALUES (@startDate, @endDate,@codingGoal)";

            // Execute the query for each record in the collection
            connection.Execute(insertGoal, new
            {
                startDate = _goal.startDate,
                endDate = _goal.endDate,
                codingGoal = _goal.codingGoal
            });
        }
    }

    internal IEnumerable<Goal> GetGoals()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM goals";

            var records = connection.Query<Goal>(selectQuery);
            return records;
        }
    }
}
