using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.Services;

public class Database
{
    private static string dbConnection = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

    public static void CreateDatabase()
    {
        using SQLiteConnection sqlite = new SQLiteConnection(dbConnection);
        sqlite.Open();

        string sql = @"CREATE TABLE IF NOT EXISTS 
                        [logs] (
                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                        [Date] VARCHAR(75) NULL,
                        [StartTime] VARCHAR(75) NULL, 
                        [EndTime] VARCHAR(75) NULL, 
                        [Duration] VARCHAR(75) NULL)";
        string sql2 = @"CREATE TABLE IF NOT EXISTS
                        [goals] (
                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [Name] VARCHAR(75) NULL,
                        [DateStarted] VARCHAR(75) NULL,
                        [DateEnded] VARCHAR(75) NULL,
                        [TimePerDay] INTEGER,
                        [Achieved] VARCHAR(3) NULL)";

        using SQLiteCommand cmd = new SQLiteCommand(sqlite);
        using SQLiteCommand cmd2 = new SQLiteCommand(sqlite);

        cmd.CommandText = sql;
        cmd2.CommandText = sql2;

        try
        {
            cmd.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
        }
        catch (SQLiteException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] " + e.Message);

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
