using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker.Model;

namespace CodingTracker.Database
{
    public static class DatabaseController
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

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
        public static IEnumerable<CodingSession> GetList()
        {
            using var db = new SqliteConnection(ConnectionString);
            var sql = "SELECT * FROM coding_tracker";
            return db.Query<CodingSession>(sql);
        }

        public static void Insert(DateTime start, DateTime end)
        {
            using var db = new SqliteConnection(ConnectionString);
            var sql = @"INSERT INTO coding_tracker (start, end) VALUES ($Start, $End)";
            var parameters = new { Start = start.ToString(), End = end.ToString() };
            db.Execute(sql, parameters);
        }

        public static void Delete(int id)
        {
            using var db = new SqliteConnection(ConnectionString);
            var sql = "DELETE FROM coding_tracker WHERE id = $ID";
            var parameters = new { ID = id };
            db.Execute(sql, parameters);
        }

        public static void Update(int id, string start, string end)
        {
            using var db = new SqliteConnection(ConnectionString);
            var sql = "UPDATE coding_tracker SET start = $Start, end = $End WHERE id = $ID";
            var parameters = new { ID = id, Start = start, End = end };
            var result = db.Execute(sql, parameters);
        }
    }
}