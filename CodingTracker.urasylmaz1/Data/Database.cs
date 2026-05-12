using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.urasylmaz1.Data
{
    public class Database
    {
        private readonly string _connectionString;
        public Database(string connectionString)
        {
           _connectionString = connectionString;
        }

        public void Initialize()
        {
            // using Dapper instead of raw ADO.NET for simplicity

            using var connection =
            new SqliteConnection(_connectionString);

            string sql = @"
                CREATE TABLE IF NOT EXISTS CodingSessions
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                    );
            ";

            connection.Execute(sql);
        }
            
    }
}
