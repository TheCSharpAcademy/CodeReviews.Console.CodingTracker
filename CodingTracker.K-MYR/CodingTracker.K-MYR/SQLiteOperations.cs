using Microsoft.Data.Sqlite;
using CodingTracker.K_MYR.Models;
using System.Globalization;
using System.Configuration;

namespace CodingTracker.K_MYR;

internal class SQLiteOperations{
    
    static private readonly string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

    internal static void CreateTable()
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS CodingSessions 
                                    (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        StartTime TEXT,
                                        EndTime TEXT,
                                        Duration TEXT                                     
                                    )";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static List<CodingSession> SelectAllRecords()
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM CodingSessions";

        SqliteDataReader reader = tableCmd.ExecuteReader();
        List<CodingSession> records = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                records.Add(new CodingSession
                {
                    Id = reader.GetInt32(0),
                    StartTime = DateTime.ParseExact(reader.GetString(1), "HH:mm dd-MM-yyyy", new CultureInfo("de-DE")),
                    EndTime = DateTime.ParseExact(reader.GetString(2), "HH:mm dd-MM-yyyy", new CultureInfo("de-DE")),
                    Duration = TimeSpan.ParseExact(reader.GetString(3), "hh\\:mm", new CultureInfo("de-DE"), TimeSpanStyles.None)
                });
            }
        }
        return records;
    }

    internal static void Insert(string startTime, string endTime, string duration)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"INSERT INTO CodingSessions (StartTime, EndTime, Duration) VALUES ('{startTime}', '{endTime}', '{duration}')";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static int RecordExists(int recordId)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM CodingSessions WHERE Id = {recordId})";

        return Convert.ToInt32(tableCmd.ExecuteScalar());
    }

    internal static void Update(int id, string startTime, string endTime, string duration)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE CodingSessions SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}' WHERE Id = {id} ";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void Delete(int id)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE FROM CodingSessions WHERE Id = {id}";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}