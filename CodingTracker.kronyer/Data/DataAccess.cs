using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.Data;

public class DataAccess
{
    private string ConnectionString;

    IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    public DataAccess()
    {

        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    public void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS records (Id INTEGER PRIMARY KEY AUTOINCREMENT, DateStart TEXT NOT NULL, DateEnd TEXT NOT NULL)";

            connection.Execute(createTableQuery);

        }
    }

    public void InsertRecord(CodingRecord record)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string insertQuery = @"INSERT INTO records (DateStart, DateEnd) VALUES (@DateStart, @DateEnd)";
            connection.Execute(insertQuery, new { record.DateStart, record.DateEnd });
        }
    }

    public IEnumerable<CodingRecord> GetRecords()
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

    public void BulkInserRecords(List<CodingRecord> records)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            string insertQuery = @"INSERT INTO records (DateStart, DateEnd) VALUES (@DateStart, @DateEnd)";

            connection.Execute(insertQuery, records.Select(record => new { record.DateStart, record.DateEnd }));
        }
    }

    public void UpdateRecord(CodingRecord updated)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string updateQuery = @"UPDATE records SET DateStart = @DateStart, DateEnd = @DateEnd WHERE Id = @Id";

            connection.Execute(updateQuery, new { updated.DateStart, updated.DateEnd, updated.Id });
        }
    }

    public int DeleteRecord(int recordId)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string deleteQuery = "DELETE FROM records WHERE Id = @Id";
            int rowsAffected = connection.Execute(deleteQuery, new { Id = recordId });

            return rowsAffected;
        }
    }

    public void ResetIds()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            List<CodingRecord> codes = connection.Query<CodingRecord>("SELECT Id FROM records ORDER BY Id").ToList();

            for (int i = 0; i < codes.Count; i++)
            {
                connection.Execute("UPDATE records SET Id = @NewId WHERE Id = @OldId", new { NewId = i + 1, OldId = codes[i].Id });
            }
        }
    }
}
