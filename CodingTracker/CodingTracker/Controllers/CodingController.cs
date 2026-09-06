using CodingTracker.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;


namespace CodingTracker.Controllers
{
    internal class CodingController
    {
        private readonly string _connectionString;

        public CodingController()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = config.GetConnectionString("DefaultConnection") ?? "Data Source=coding-tracker.db";

            InitDatabase();
        }

        private void InitDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            string initSql = @"
                        CREATE TABLE IF NOT EXISTS CodingSessions(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT NOT NULL                        
                        );";
            connection.Execute(initSql);
        }

        public bool InsertSql(CodingSession session)
        {
            using var connection = new SqliteConnection(_connectionString);
            try
            {
                string sql = @"
            INSERT INTO CodingSessions (StartTime, EndTime) 
            VALUES (@StartTime, @EndTime);";

                int rowsAffected = connection.Execute(sql, new
                {
                    StartTime = session.StartTime.ToString("dd-MM-yyyy HH:mm:ss"),
                    EndTime = session.EndTime.ToString("dd-MM-yyyy HH:mm:ss")
                });

                return rowsAffected > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<CodingSession> GetSessionHistory()
        {
            using var connection = new SqliteConnection(_connectionString);
            try
            {
                string sql = @"
                SELECT 
                Id, 
                StartTime AS StartTimeString, 
                EndTime AS EndTimeString 
                FROM CodingSessions;";

                // Dapper makes List<CodingSession>
                List<CodingSession> sessionList = connection.Query<CodingSession>(sql).AsList();

                return sessionList;
            }
            catch (Exception)
            {
                return new List<CodingSession>();
            }
        }

        public bool UpdateSessionHistory(CodingSession session)
        {
            using var connection = new SqliteConnection(_connectionString);
            try
            {
                string sql = @"
                UPDATE CodingSessions 
                SET StartTime = @StartTimeString, 
                EndTime = @EndTimeString 
                WHERE Id = @Id;";

                int rowsAffected = connection.Execute(sql, session);

                // true if row was affected
                return (rowsAffected > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSession(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            try
            {
                string sql = @"
                DELETE FROM CodingSessions 
                WHERE Id = @Id;";

                int rowsAffected = connection.Execute(sql,new { Id = id });

                // true if row was affected
                return (rowsAffected > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}