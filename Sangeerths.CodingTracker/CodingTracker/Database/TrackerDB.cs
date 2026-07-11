using CodingTracker.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Database
{
    public class TrackerDB
    {
        private readonly string _connectionString;

        public TrackerDB(string connectionString)
        {
            _connectionString = connectionString;
            SQLitePCL.Batteries.Init();
        }

        public void Creation()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                string sql = @"
        CREATE TABLE IF NOT EXISTS CodingSessions (
            Id INTEGER PRIMARY KEY AUTOINCREMENT, 
            StartTime TEXT, 
            EndTime TEXT, 
            Duration REAL
        );";
                connection.Open();
                connection.Execute(sql);

            }
        }

        public void InsertCodingSession(TimeSession request)
        {
            string sql = @"
    INSERT INTO CodingSessions (StartTime, EndTime, Duration)
    VALUES (@StartTime, @EndTime, @Duration);";
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql, request);
            }
        }

        public IEnumerable<List<CodingSessionModel>> GetCodingSessions()
        {
            string sql = @"SELECT * FROM CodingSessions;";
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<CodingSessionModel>(sql);
                return new List<List<CodingSessionModel>> { new List<CodingSessionModel>(result) };
            }
        }

        public bool DeleteCodingSession(int id)
        {
            string sql = @"DELETE FROM CodingSessions WHERE Id = @Id;";
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                int rowsAffected = connection.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public bool UpdateCodingSession( CodingSessionModel request)
        {
            string sql = @"UPDATE CodingSessions SET StartTime = @startTime, EndTime = @endTime, Duration = @duration WHERE Id = @Id;";
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                int rowsAffected = connection.Execute(sql, new { Id = request.Id, request.startTime, request.endTime, request.duration });
                return rowsAffected > 0;
            }
        }

        public bool CodingSessionExists(int id)
        {
            string sql = "SELECT COUNT(*) FROM CodingSessions WHERE Id = @Id;";

            using var connection = new SqliteConnection(_connectionString);

            int count = connection.ExecuteScalar<int>(sql, new { Id = id });

            return count > 0;
        }

        public List<CodingSessionModel> FilterCodingSessionsByDate(DateTime? dateTime)
        {
            string sql = @"SELECT * FROM CodingSessions
                   WHERE date(StartTime) = date(@dateTime);";

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<CodingSessionModel>(sql, new { dateTime }).ToList();
            }
        }
        public List<CodingSessionModel> FilterCodingSessionsByWeek(DateTime? dateTime)
        {
            string sql = @"SELECT * FROM CodingSessions WHERE strftime('%W', StartTime) = strftime('%W', @dateTime) AND strftime('%Y', StartTime) = strftime('%Y', @dateTime);";
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<CodingSessionModel>(sql, new { dateTime }).ToList();
            }
        }

        public IEnumerable<CodingSessionModel> FilterCodingSessionsByMonth(int month)
        {
            string sql = @"
        SELECT *
        FROM CodingSessions
        WHERE strftime('%m', StartTime) = printf('%02d', @month);";

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<CodingSessionModel>(sql, new { month }).ToList();
            }
        }

        public IEnumerable<CodingSessionModel> FilterCodingSessionsByYear(int year)
        {
            string sql = @"
        SELECT *
        FROM CodingSessions
        WHERE strftime('%Y', StartTime) = @Year;";

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<CodingSessionModel>(
                    sql,
                    new { Year = year.ToString() });
            }
        }

        public List<CodingSessionModel> SortCodingSession(string sortBy)
        {
            StringBuilder sql = new StringBuilder();
            if(sortBy == "Ascending")
            {
                 sql.Append(@"SELECT * FROM CodingSessions ORDER BY StartTime ASC;");
                
            }
            else if (sortBy == "Descending")
            {
                 sql.Append(@"SELECT * FROM CodingSessions ORDER BY StartTime DESC;");
                
            }
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<CodingSessionModel>(sql.ToString()).ToList();
            }

        }
    }
}
