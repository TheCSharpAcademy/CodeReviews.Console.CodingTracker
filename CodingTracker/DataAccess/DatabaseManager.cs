using System.Configuration;
using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;

public class DatabaseManager
{
    private readonly string _connectionString;
    private readonly string? _databsePath = ConfigurationManager.AppSettings["path"];

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection GetConnection() => new(_connectionString);

    public void InitializeDatabase()
    {
        if (!File.Exists(_databsePath))
        {
            CreateDatabase();
        }
    }

    private void CreateDatabase()
    {
        using (var conn = GetConnection())
        {
            try
            {
                conn.Open();

                string query = @"
                    CREATE TABLE codingtracker (
	                    Id	INTEGER NOT NULL UNIQUE,
	                    StartTime	TEXT,
	                    EndTime	TEXT,
	                    Duration	TEXT,
	                    PRIMARY KEY(id AUTOINCREMENT)
                    );";

                conn.Execute(query);
            }
            catch (DbException dbEx)
            {
                Console.WriteLine($"Database error occurred: {dbEx.Message}"); //TODO will this need updating for Spectre?
                throw; 
            }
        }
    }
}