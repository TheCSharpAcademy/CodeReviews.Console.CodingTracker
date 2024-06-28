using Application.Entities;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DatabaseLibrary
{
    public class Goal
    {
        public void Create()
        {
            using var connectionString = new SqliteConnection(@"Data Source = Coding_Tracker.db");
            connectionString.Open();
            string createTable = @"CREATE TABLE IF NOT EXISTS codingGoal (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Month TEXT,
            Hours INTEGER
            )";

            connectionString.Execute(createTable);
            connectionString.Close();
        }

        public async void Insert(string month, int hours)
        {
            using var connectionString = new SqliteConnection(@"Data Source = Coding_Tracker.db");

            var sql = "Insert INTO codingGoal (Month, Hours) VALUES (@Month, @Hours)";
            GoalEntity goalEntity = new() { Month = month, Hours=hours};

            var affectedRows = await connectionString.ExecuteAsync(sql, goalEntity);
            Console.WriteLine($"{affectedRows} row(s) inserted");
        }

        public void GoalProgress(string month)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
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

            Console.WriteLine($"total {total} totalMilliseconds {TotalToMilliseconds} codingHours {codingHours}");

            sql = @$"SELECT Hours
                        FROM  codingGoal
                        WHERE strftime('%Y-%m', {month})";


            var codingGoal = connection.QuerySingle<int>(sql);
            var remainingHours = codingGoal - codingHours;
            var surpassedHours = codingHours - codingGoal;


            Console.WriteLine("------------------------------------------------\n"); ;

            if(codingHours < codingGoal)
            {
                Console.WriteLine($"Your coding goal this month is {codingGoal} hours and you have coded {codingHours} hours, you have {remainingHours} hours left to reach your goal ");
            }
            else
            {
                Console.WriteLine($"You have surpassed your goal by {surpassedHours} hours, Weldone Tiger");
            }
           
            Console.WriteLine("------------------------------------------------\n"); ;
        }

        public double AverageTimePerDay(string month, int days)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
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
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT * 
                        FROM  codingGoal
                        WHERE strftime('%Y-%m', {month}) Limit 2";

            
            var goals = connection.Query<Goal>(sql).ToList();
            return goals.Count > 0;
        }


        public async void Update(string month, int hours)
        {
            using var connectionString = new SqliteConnection(@"Data Source = Coding_Tracker.db");

            var sql = @"UPDATE codingGoal SET
                        Hours = @Hours WHERE Month = @Month";
            GoalEntity goalEntity = new() {Month = month, Hours = hours};

            var affectedRows = await connectionString.ExecuteAsync(sql, goalEntity);
            Console.WriteLine($"{affectedRows} row(s) inserted");
        }
    }
}
