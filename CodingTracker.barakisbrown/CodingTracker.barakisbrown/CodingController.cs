namespace CodingTracker.barakisbrown;

using Microsoft.Data.Sqlite;
using System;
using System.Configuration;
using System.Text;

public class CodingController
{
    private readonly string CreateTableString = @"CREATE TABLE sessions (
	ID	INTEGER UNIQUE,
	StartTime	TEXT NOT NULL,
	EndTime	TEXT NOT NULL,
	Duration	TEXT NOT NULL,
	PRIMARY KEY(ID AUTOINCREMENT));";

    private static readonly string ConnectionName = "myDB";
    private static readonly string DbNameKey = "DbName";
    private static readonly string DbPathKey = "DbPath";

    private readonly string? TableName = "sessions";
    private readonly string? DataSource;

    public CodingController()
    {
        DataSource = GetConnectionString();
        if (!DBExist)
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
            Console.WriteLine("Exception Message is {0}", e.Message);
            throw;
        }
    }

    private static string? DBPATH => ConfigurationManager.AppSettings.Get(DbPathKey);

    private static string? DBNAME => ConfigurationManager.AppSettings.Get(DbNameKey);

    private static string? GetConnectionString()
    {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionName];

        StringBuilder builder = new();
        builder.Append(settings);
        builder.Append(DBPATH);
        builder.Append(DBNAME);
        return builder.ToString();
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

    private bool DBExist => File.Exists(CodingController.DBPATH + CodingController.DBNAME);
}