using CodingTracker.K_MYR.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Globalization;

namespace CodingTracker.K_MYR;

internal class SQLiteOperations
{
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

        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS CodingGoals
                                    (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Name TEXT,
                                        StartDate TEXT,
                                        Deadline TEXT,
                                        Goal TEXT,
                                        ElapsedTime TEXT                                  
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
                    StartTime = DateTime.ParseExact(reader.GetString(1), "HH:mm:ss dd-MM-yyyy", new CultureInfo("de-DE")),
                    EndTime = DateTime.ParseExact(reader.GetString(2), "HH:mm:ss dd-MM-yyyy", new CultureInfo("de-DE")),
                    Duration = TimeSpan.ParseExact(reader.GetString(3), "dd\\:hh\\:mm\\:ss", new CultureInfo("de-DE"), TimeSpanStyles.None)
                });
            }
        }

        connection.Close();
        return records;
    }

    internal static void InsertRecord(string startTime, string endTime, string duration)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"INSERT INTO CodingSessions (StartTime, EndTime, Duration) VALUES ('{startTime}', '{endTime}', '{duration}')";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void UpdateRecord(int id, string startTime, string endTime, string duration)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"UPDATE CodingSessions SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}' WHERE Id = {id} ";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void DeleteRecord(int id)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"DELETE FROM CodingSessions WHERE Id = {id}";
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

    internal static List<CodingGoal> SelectAllGoals()
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM CodingGoals";

        SqliteDataReader reader = tableCmd.ExecuteReader();
        List<CodingGoal> goals = new();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                goals.Add(new CodingGoal
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    StartDate = DateTime.ParseExact(reader.GetString(2), "dd-MM-yyyy", new CultureInfo("de-DE"), DateTimeStyles.None),
                    Deadline = DateTime.ParseExact(reader.GetString(3), "dd-MM-yyyy", new CultureInfo("de-DE"), DateTimeStyles.None),
                    Goal = TimeSpan.ParseExact(reader.GetString(4), "dd\\:hh\\:mm\\:ss", new CultureInfo("de-DE"), TimeSpanStyles.None),
                    ElapsedTime = TimeSpan.ParseExact(reader.GetString(5), "dd\\:hh\\:mm\\:ss", new CultureInfo("de-DE"), TimeSpanStyles.None),
                });
            }
        }

        connection.Close();
        return goals;
    }

    internal static void InsertGoal(string name, string startDate, string deadline, string goal, string elapsedTime)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"INSERT INTO CodingGoals (Name, StartDate, Deadline, Goal, ElapsedTime) VALUES ('{name}', '{startDate}', '{deadline}', '{goal}', '{elapsedTime}')";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void UpdateGoal(int id, string name, string startDate, string deadline, string goal, string elapsedTime)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"UPDATE CodingGoals SET Name = '{name}', StartDate = '{startDate}', Deadline = '{deadline}', Goal = '{goal}', ElapsedTime = '{elapsedTime}' WHERE Id = {id} ";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void UpdateGoalElapsedTime(int id, string elapsedTime)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"UPDATE CodingGoals SET ElapsedTime = '{elapsedTime}' WHERE Id = {id} ";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static void DeleteGoal(int id)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"DELETE FROM CodingGoals WHERE Id = {id}";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    internal static int GoalExists(int id)
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM CodingGoals WHERE Id = {id})";

        return Convert.ToInt32(tableCmd.ExecuteScalar());
    }




}