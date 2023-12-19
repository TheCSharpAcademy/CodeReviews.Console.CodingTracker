using CodingTracker.wkktoria.Models;
using Microsoft.Data.Sqlite;

namespace CodingTracker.wkktoria.Services;

public class CodingService
{
    private readonly string _connectionString;

    public CodingService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<CodingSession> ReadAll(string order)
    {
        var tableData = new List<CodingSession>();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"SELECT * FROM sessions ORDER BY Duration {order}";

            using var reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    tableData.Add(new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        StartTime = reader.GetString(1),
                        EndTime = reader.GetString(2),
                        Duration = reader.GetDouble(3)
                    });

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Reading all records failed.");
        }

        return tableData;
    }

    public List<CodingSession> ReadAllBetweenDates(string startDateTime, string endDateTime, string order)
    {
        var tableData = new List<CodingSession>();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @$"SELECT * FROM sessions WHERE StartTime >= '{startDateTime}' AND EndTime <= '{endDateTime}' ORDER BY Duration {order}";

            using var reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    tableData.Add(new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        StartTime = reader.GetString(1),
                        EndTime = reader.GetString(2),
                        Duration = reader.GetDouble(3)
                    });

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Reading records between dates failed.");
        }

        return tableData;
    }

    public CodingSession ReadById(int id)
    {
        var record = new CodingSession();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"SELECT * FROM sessions WHERE Id={id}";

            using var reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    record = new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        StartTime = reader.GetString(1),
                        EndTime = reader.GetString(2),
                        Duration = reader.GetDouble(3)
                    };

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Reading record by id failed.");
        }

        return record;
    }

    public void Create(CodingSession record)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @$"INSERT INTO sessions(StartTime, EndTime, Duration) VALUES('{record.StartTime}', '{record.EndTime}', '{record.Duration}')";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Creating new record failed.");
        }
    }

    public void Update(CodingSession record)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @$"UPDATE sessions SET StartTime='{record.StartTime}', EndTime='{record.EndTime}', Duration='{record.Duration}' WHERE Id={record.Id}";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Updating record failed.");
        }
    }

    public void Delete(int id)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"DELETE FROM sessions WHERE Id={id}";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Deleting record failed.");
        }
    }

    public List<ReportData> CreateReport(string startDateTime, string endDateTime)
    {
        var tableData = new List<ReportData>();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @$"SELECT SUM(Duration), avg(Duration)  FROM sessions WHERE StartTime >= '{startDateTime}' AND EndTime <= '{endDateTime}'";

            using var reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    tableData.Add(new ReportData
                    {
                        Total = reader.GetDouble(0),
                        Average = reader.GetDouble(1)
                    });

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Creating report failed.");
        }

        return tableData;
    }
}