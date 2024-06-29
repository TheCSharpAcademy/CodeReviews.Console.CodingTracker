using Application.Entities;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace DatabaseLibrary
{
    public class Goal
    {
        readonly string connectionString = ConfigurationManager.AppSettings["connectionString"] ?? "Data Source = Coding_Tracker.db";
        public void Create()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            string createTable = @"CREATE TABLE IF NOT EXISTS codingGoal (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Month TEXT,
            Hours INTEGER
            )";

            connection.Execute(createTable);
            connection.Close();
        }

        public async Task<int> Insert(string month, int hours)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = "Insert INTO codingGoal (Month, Hours) VALUES (@Month, @Hours)";
            GoalEntity goalEntity = new() { Month = month, Hours=hours};

            var affectedRows = await connection.ExecuteAsync(sql, goalEntity);
            return affectedRows;
        }

        public (int, int) GoalProgress(string month)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                var sql = @$"SELECT *
                        FROM codingTracker
                        WHERE strftime('%Y-%m', EndDate) = '{month}'";

                var codingDurations = connection.Query<UserEntity>(sql).ToList();

                TimeSpan duration;
                TimeSpan total = TimeSpan.Zero;

                foreach (UserEntity userEntity in codingDurations)
                {
                    TimeSpan.TryParse(userEntity.Duration, out duration);
                    total += duration;
                }

                int TotalToMilliseconds = (int)TimeSpan.Parse(total.ToString()).TotalMilliseconds;
                int codingHours = (int)TimeSpan.FromMilliseconds(TotalToMilliseconds).TotalHours;

                sql = @$"SELECT Hours
                        FROM  codingGoal
                        WHERE strftime('%Y-%m', {month})";


                var codingGoal = connection.QuerySingle<int>(sql);


                return (codingGoal, codingHours);
            }
            catch (InvalidOperationException ex) {
                return (-1, -1);
            }
        }

        public double AverageTimePerDay(string month, int days)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT Hours 
                        FROM  codingGoal
                        WHERE strftime('%Y-%m', {month})";
            var codingGoal = connection.QuerySingle<int>(sql);
            double codePerDay = (double)codingGoal/days;

            return codePerDay;
        }

        public bool GoalCreated(string month)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT * 
                        FROM  codingGoal
                        WHERE strftime('%Y-%m', {month}) Limit 2";

            
            var goals = connection.Query<Goal>(sql).ToList();
            return goals.Count > 0;
        }

        public async void Update(string month, int hours)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = @"UPDATE codingGoal SET
                        Hours = @Hours WHERE Month = @Month";
            GoalEntity goalEntity = new() {Month = month, Hours = hours};

            var affectedRows = await connection.ExecuteAsync(sql, goalEntity);
            Console.WriteLine($"{affectedRows} row(s) inserted");
        }
    }
}
