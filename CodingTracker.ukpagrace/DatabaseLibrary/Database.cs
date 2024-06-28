using Dapper;
using Microsoft.Data.Sqlite;
using Application.Entities;


namespace DatabaseLibrary
{
    public class Database
    {

        public void Create()
        {
            using var connectionString = new SqliteConnection(@"Data Source = Coding_Tracker.db");
            connectionString.Open();
            string createTable = @"CREATE TABLE IF NOT EXISTS codingTracker (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartDate TEXT,
                EndDate TEXT,
                Duration TEXT
             )";

            connectionString.Execute(createTable);
            connectionString.Close();
        }



        public async Task<int> Insert(DateTime startDate, DateTime endDate, TimeSpan duration)
        {
            using var connectionString = new SqliteConnection(@"Data Source = Coding_Tracker.db");

            var sql = "Insert INTO codingTracker (StartDate, EndDate, Duration) VALUES (@StartDate, @EndDate, @Duration)";
            UserEntity userEntity = new() { StartDate = startDate.ToString("yyyy-MM-dd hh:mm"), EndDate = endDate.ToString("yyyy-MM-dd hh:mm"), Duration = duration.ToString() };


            int affectedRows = await connectionString.ExecuteAsync(sql, userEntity);
            return affectedRows;
            
        }


        public async Task<int> Update(int id, DateTime startDate, DateTime endDate, TimeSpan duration)
        {
            using var connectionString = new SqliteConnection(@"Data Source = Coding_Tracker.db");

            var sql = @"UPDATE codingTracker SET
                        StartDate = @StartDate, EndDate = @EndDate,
                        Duration= @Duration WHERE Id = @Id";
            UserEntity userEntity = new() { Id = id, StartDate = startDate.ToString("yyyy-MM-dd hh:mm"), EndDate = endDate.ToString("yyyy-MM-dd hh:mm"), Duration = duration.ToString() };


            var affectedRows = await connectionString.ExecuteAsync(sql, userEntity);
            return affectedRows; 
        }


        public List<UserEntity> List()
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @"SELECT * FROM codingTracker";

            var codingDurations = connection.Query<UserEntity>(sql).ToList();

            return codingDurations;

        }

        public UserEntity GetOne(int id)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @"SELECT * FROM codingTracker where Id = @Id";

            var codingDurations = connection.Query<UserEntity>(sql,new {Id = id}).ToList();

            return codingDurations[0];
        }

        public List<UserEntity> Filter(string format, string firstRange, string secondRange, string order)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT *
                        FROM codingTracker
                        WHERE strftime('{format}', EndDate)
                        BETWEEN '{firstRange}' AND '{secondRange}' ORDER BY EndDate {order}";

            var codingDurations = connection.Query<UserEntity>(sql).ToList();
            return codingDurations;



        }

        public List<UserEntity> Filter(string format, string value, string order)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT *
                        FROM codingTracker
                        WHERE strftime('{format}', EndDate) = '{value}' ORDER BY EndDate  {order}";

            var codingDurations = connection.Query<UserEntity>(sql).ToList();
            return codingDurations;

        }

        public (string, string) Analyze(string format, string value)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT *
                        FROM codingTracker
                        WHERE strftime('{format}', EndDate) = '{value}'";

            var codingDurations = connection.Query<UserEntity>(sql).ToList();

            TimeSpan duration;
            TimeSpan total = TimeSpan.Zero;

            foreach (UserEntity userEntity in codingDurations)
            {
                TimeSpan.TryParse(userEntity.Duration, out duration);
                total += duration;
            }

            string totalString = FormatTimeSpan(total);
            TimeSpan average = total / codingDurations.Count;
            string averageString = FormatTimeSpan(average);
            return (totalString, averageString);
        }

        public string FormatTimeSpan(TimeSpan duration)
        {
            List<string> parts = new List<string>();
            if (duration.Days > 0)
            {
                parts.Add($"{duration.Days} {(duration.Days == 1 ? "day" : "days")}");
            }
            if (duration.Hours > 0)
            {
                parts.Add($"{duration.Hours} {(duration.Hours == 1 ? "hour" : "hours")}");
            }
            if (duration.Minutes > 0)
            {
                parts.Add($"{duration.Minutes} {(duration.Minutes == 1 ? "minute" : "minutes")}");
            }
            if (duration.Seconds > 0)
            {
                parts.Add($"{duration.Seconds} {(duration.Seconds == 1 ? "second" : "seconds")}");
            }
            return string.Join(", ", parts);
        }


        public async Task<int> Delete(int id)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @"DELETE FROM codingTracker where Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows;
        }
    }
}