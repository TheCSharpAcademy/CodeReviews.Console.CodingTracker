using Dapper;
using Microsoft.Data.Sqlite;
using Application.Entities;
using System.Configuration;

namespace DatabaseLibrary
{
    public class Database
    {
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"] ?? "Data Source = Coding_Tracker.db";
        public void Create()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            string createTable = @"CREATE TABLE IF NOT EXISTS codingTracker (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartDate TEXT,
                EndDate TEXT,
                Duration TEXT
             )";

            connection.Execute(createTable);
            connection.Close();
        }

        public async Task<int> Insert(DateTime startDate, DateTime endDate, TimeSpan duration)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = "Insert INTO codingTracker (StartDate, EndDate, Duration) VALUES (@StartDate, @EndDate, @Duration)";
            UserEntity userEntity = new() { 
                StartDate = startDate.ToString("yyyy-MM-dd hh:mm"),
                EndDate = endDate.ToString("yyyy-MM-dd hh:mm"),
                Duration = duration.ToString("d\\.hh\\:mm\\:ss") 
            };


            int affectedRows = await connection.ExecuteAsync(sql, userEntity);
            return affectedRows;
        }

        public async Task<int> Update(int id, DateTime startDate, DateTime endDate, TimeSpan duration)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = @"UPDATE codingTracker SET
                        StartDate = @StartDate, EndDate = @EndDate,
                        Duration= @Duration WHERE Id = @Id";
            UserEntity userEntity = new() { 
                Id = id,
                StartDate = startDate.ToString("yyyy-MM-dd hh:mm"),
                EndDate = endDate.ToString("yyyy-MM-dd hh:mm"),
                Duration = duration.ToString("d\\.hh\\:mm\\:ss")
            };
            var affectedRows = await connection.ExecuteAsync(sql, userEntity);
            return affectedRows; 
        }
        public List<UserEntity> List()
        {
            using var connection = new SqliteConnection(connectionString);
            var sql = @"SELECT * FROM codingTracker";
            var records = connection.Query<UserEntity>(sql).ToList();
            return records;
        }

        public UserEntity GetOne(int id)
        {
            try
            {
                string connectionString = @"Data Source = Coding_Tracker.db";
                using var connection = new SqliteConnection(connectionString);

                var sql = @"SELECT * FROM codingTracker where Id = @Id";

                var record = connection.QuerySingle<UserEntity>(sql, new { Id = id });

                return record;
            }
            catch (InvalidOperationException ex) {
                return null;
            
            }
        }

        public List<UserEntity> Filter(string format, string firstRange, string secondRange, string order)
        {
            using var connection = new SqliteConnection(connectionString);
            var sql = @$"SELECT *
                        FROM codingTracker
                        WHERE strftime('{format}', EndDate)
                        BETWEEN '{firstRange}' AND '{secondRange}' ORDER BY EndDate {order}";

            var records = connection.Query<UserEntity>(sql).ToList();
            return records;
        }

        public List<UserEntity> Filter(string format, string value, string order)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT *
                        FROM codingTracker
                        WHERE strftime('{format}', EndDate) = '{value}' ORDER BY EndDate  {order}";

            var records = connection.Query<UserEntity>(sql).ToList();
            return records;

        }
        public (TimeSpan, TimeSpan) Analyze(string format, string value)
        {
            using var connection = new SqliteConnection(connectionString);
            var sql = @$"SELECT *
                        FROM codingTracker
                        WHERE strftime('{format}', EndDate) = '{value}'";

            var records = connection.Query<UserEntity>(sql).ToList();

            TimeSpan duration;
            TimeSpan total = TimeSpan.Zero;

            foreach (UserEntity record in records)
            {
                TimeSpan.TryParse(record.Duration, out duration);
                total += duration;
            }
            TimeSpan average = total / records.Count;
            return (total, average);
        }
        public async Task<int> Delete(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            var sql = @"DELETE FROM codingTracker where Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows;
        }
    }
}