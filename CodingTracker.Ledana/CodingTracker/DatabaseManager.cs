using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.Ledana
{
    internal class DatabaseManager
    {
        internal void CreateTable(string connectionString)
        {
            CreateDb();
            using SqlConnection connection = new(connectionString);
            connection.Open();
            string createTableQuery = @"IF OBJECT_ID('dbo.Coding', 'U') IS NULL
                                        CREATE TABLE dbo.Coding (
                                        [Id] INT PRIMARY KEY IDENTITY,
                                        [Date] NVARCHAR(250),
                                        [Start] NVARCHAR(250),
                                        [End] NVARCHAR(250),
                                        [Duration] NVARCHAR(250)
                                        )";
            connection.Execute(createTableQuery);
        }
        internal static void CreateDb()
        {
            var dbName = "CodingTracker";
            var masterConn = ConfigurationManager.AppSettings["MasterString"];
            using SqlConnection masterConnection = new(masterConn);

            //Check if db exists
            var exists = masterConnection.QuerySingleOrDefault<int>(
                "SELECT COUNT(*) FROM sys.databases WHERE name = @name",
                new { name = dbName });
            if (exists == 0)
            {
                masterConnection.Execute("CREATE DATABASE CodingTracker");
                Console.WriteLine($"Database '{dbName}' created.");
            }
        }
    }
}