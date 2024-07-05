using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.Model;

namespace CodingTracker.Database
{
    public static class DatabaseController
    {
        private static readonly string? ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        public static void CreateDb()
        {
            using var db = new SqliteConnection(ConnectionString);
            var sql = @"CREATE TABLE IF NOT EXISTS coding_tracker (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        start TEXT NOT NULL,
                        end TEXT NOT NULL
                )";

            db.Execute(sql);
        }

        public static void Insert(DateTime start, DateTime end)
        {
            using var db = new SqliteConnection(ConnectionString);
            var sql = @"INSERT INTO coding_tracker (start, end) VALUES ($Start, $End)";
            var parameters = new { Start = start.ToString(), End = end.ToString() };
            db.Execute(sql, parameters);
        }

        public static IEnumerable<CodingSession> GetList()
        {
            using var db = new SqliteConnection(ConnectionString);
            var sql = "Select * FROM coding_tracker";
            return db.Query<CodingSession>(sql);
        }
    }
}