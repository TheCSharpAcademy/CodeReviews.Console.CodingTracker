namespace CodingTracker.barakisbrown;

using Microsoft.Data.Sqlite;
using System;
using System.Configuration;

public class CodingController
{
    private readonly string CreateTableString = @"CREATE TABLE sessions (
	ID	INTEGER UNIQUE,
	StartTime	TEXT NOT NULL,
	EndTime	TEXT NOT NULL,
	Duration	TEXT NOT NULL,
	PRIMARY KEY(ID AUTOINCREMENT));";
    private static readonly string ConnectionName = "myDB";
    private readonly string? DataSource;

    public CodingController()
    {
        DataSource = CodingController.GetConnectionString();
        if (!DBExist())
            CreateDB();
    }

    private void CreateDB()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand(CreateTableString, conn);
        try
        {
            CodingController.ExecuteNonQuery(cmd);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Creating Database. ");
            Console.WriteLine("Exception Message is {0}",e.Message);
            throw;
        }
    }

    private static string? GetDbPath()
    {
        return ConfigurationManager.AppSettings.Get("DbPath");
    }

    private static string? GetConnectionString()
    {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionName];
        return settings?.ConnectionString;
    }

    private static int ExecuteNonQuery(SqliteCommand cmd)
    {
        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: Something went wrong.");
            Console.WriteLine($"Exception Message is {ex.Message}");
            throw;
        }
    }

    private static bool DBExist()
    {
        string ?DbName = ConfigurationManager.AppSettings.Get("DbName");
        return File.Exists(GetDbPath() + DbName);       
    }
}
