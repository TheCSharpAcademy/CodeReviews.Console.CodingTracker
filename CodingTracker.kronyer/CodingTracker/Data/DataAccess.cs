using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.Data;

public class DataAccess
{
    private string connectionString;

    IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    public DataAccess()
    {

        connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    public void CreateDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS records (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity INTEGER, HabitId INTEGER, FOREIGN KEY(habitId) REFERENCES habits(Id) ON DELETE CASCADE)";

                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habits (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, MeasurementUnit Text)";

                tableCmd.ExecuteNonQuery();
            }


        }
    }

    
   
    public void InsertRecord(CodingRecord record)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string insertQuery = @"INSERT INTO records (DateStart, DateEnd) VALUES (@DateStart, @DateEnd)";
            connection.Execute(insertQuery, new { record.DateStart, record.DateEnd });
        }
    }

    public IEnumerable<CodingRecord> GetRecords()
    {
        using (var connection = new SqliteConnection(connectionString))
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
        using (var connection = new SqliteConnection(connectionString))
        {
            string insertQuery = @"INSERT INTO records (DateStart, DateEnd) VALUES (@DateStart, @DateEnd)";

            connection.Execute(insertQuery, records.Select(record => new { record.DateStart, record.DateEnd }));
        }
    }

    public void UpdateRecord(CodingRecord updated)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string updateQuery = @"UPDATE records SET DateStart = @DateStart, DateEnd = @DateEnd WHERE Id = @Id";

            connection.Execute(updateQuery, new { updated.DateStart, updated.DateEnd, updated.Id });
        }
    }

    public int DeleteRecord(int recordId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string deleteQuery = "DELETE FROM records WHERE Id = @Id";
            int rowsAffected = connection.Execute(deleteQuery, new { Id = recordId });

            return rowsAffected;
        }
    }

    public void ResetIds()
    {
        using (var connection = new SqliteConnection(connectionString))
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
