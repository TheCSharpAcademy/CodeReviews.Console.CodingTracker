namespace CodingTracker.barakisbrown;

using Microsoft.Data.Sqlite;
using Serilog;
using System;
using System.Configuration;
using System.Text;

public class CodingController
{
    private readonly string CreateTableString = @"
    CREATE TABLE sessions (
	    ID	INTEGER UNIQUE,
	    StartTime	TEXT NOT NULL,
	    EndTime	    TEXT NOT NULL,
	    Duration	TEXT NOT NULL,
	PRIMARY KEY(ID AUTOINCREMENT));";

    private static readonly string ConnectionName = "myDB";
    private static readonly string DbNameKey = "DbName";
    private static readonly string DbPathKey = "DbPath";

    private readonly string? TableName = "sessions";
    private readonly string? DataSource;

    public CodingController()
    {
        Log.Information("F> CodingController. Initializing DataSource and DB IF IT DOES NOT EXIST.");
        DataSource = GetConnectionString();
        if (!DBExist)
        {
            Log.Error("Database does not exist. It will be created with table needed.");
            CreateDB();
        }
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
            Log.Error("F> CreateDB: Issue can not create database");
            Log.Error("F> CreateDB: Exception Message is {0}", e.Message);
            Console.WriteLine("Can not create DB. Check Error Logs for more detail.");
            throw;
        }
    }
    
    private static bool DBExist => File.Exists(CodingController.Dbpath + CodingController.Dbname);

    private static string? Dbpath => ConfigurationManager.AppSettings.Get(DbPathKey);

    private static string? Dbname => ConfigurationManager.AppSettings.Get(DbNameKey);

    private static string? GetConnectionString()
    {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionName];

        StringBuilder builder = new();
        builder.Append(settings);
        builder.Append(Dbpath);
        builder.Append(Dbname);
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
            Log.Error("F> ExecuteNonQuery. Something went wrong here.");
            Log.Error("F> ExecuteNonQuery. Exception Message is {0}", ex.Message);
            Console.WriteLine("Error: Something went wrong. Please check error logs");
            throw;
        }
    }

    public bool Insert(CodingSession? _session)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"INSERT INTO {TableName}(StartTime,EndTime,Duration) VALUES(@start,@end,@duration)";
        cmd.Parameters.AddWithValue("@start", _session.StartTime);
        cmd.Parameters.AddWithValue("@end", _session.EndTime);
        cmd.Parameters.AddWithValue("@duration", _session.Duration);
        cmd.Prepare();
        Log.Information("INSERTING A NEW RECORD INTO TABLE.");
        return (ExecuteNonQuery(cmd) == 1);
    }

    public List<CodingSession> ShowAllCodingSession()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        string stm = $"SELECT * FROM {TableName}";

        using var cmd = new SqliteCommand(stm, conn);
        using SqliteDataReader rdr = cmd.ExecuteReader();

        if (rdr.HasRows)
        {
            List<CodingSession> _sessions = new();
            while (rdr.Read())
            {
                CodingSession _session = new()
                {
                    Id = rdr.GetInt32(0),
                    StartTime = DateTime.Parse(rdr.GetString(1)),
                    EndTime = DateTime.Parse(rdr.GetString(2)),
                    Duration = rdr.GetTimeSpan(3)
                };
                _sessions.Add(_session);
            }
            return _sessions;
        }
        return (List<CodingSession>)Enumerable.Empty<CodingSession>();
    }

    public bool DeleteSession(int id)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"DELETE FROM {TableName} WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@ID", id);
        cmd.Prepare();
        Log.Information("DELETING A RECORD FROM TABLE.");
        return ExecuteNonQuery(cmd) == 1;
    }

    public bool UpdateStartTime(CodingSession startTime)
    {
        string cmd = $"UPDATE {TableName} SET StartTime = @START,Duration = @DURATION WHERE ID=@ID";

        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var sqlCmd = conn.CreateCommand();
        sqlCmd.Connection = conn;
        sqlCmd.CommandText = cmd;
        sqlCmd.Parameters.AddWithValue("@START", startTime.StartTime);
        sqlCmd.Parameters.AddWithValue("@DURATION", startTime.Duration);
        sqlCmd.Parameters.AddWithValue("@ID", startTime.Id);
        sqlCmd.Prepare();
        Log.Information("UPDATING START TIME");
        return ExecuteNonQuery(sqlCmd) == 1;
    }

    public bool UpdateEndTime(CodingSession endTime)
    {
        string cmd = $"UPDATE {TableName} SET EndTime = @END,Duration = @DURATION WHERE ID=@ID";

        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var sqlCmd = conn.CreateCommand();
        sqlCmd.Connection = conn;
        sqlCmd.CommandText = cmd;
        sqlCmd.Parameters.AddWithValue("@END", endTime.EndTime);
        sqlCmd.Parameters.AddWithValue("@DURATION", endTime.Duration);
        sqlCmd.Parameters.AddWithValue("@ID", endTime.Id);
        sqlCmd.Prepare();
        Log.Information("UPDATING END TIME");
        return ExecuteNonQuery(sqlCmd) == 1;
    }

    public List<int>? GetValidId()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT ID FROM {TableName}";
        cmd.Connection = conn;

        using SqliteDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            List<int> idReturned = new List<int>();
            while (reader.Read())
                idReturned.Add(reader.GetInt32(0));
            return idReturned;
        }
        return (List<int>)Enumerable.Empty<int>();
    }
}