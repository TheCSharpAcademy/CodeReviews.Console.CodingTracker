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
            Month TEXT
            Hours INTEGER
            )";

            connectionString.Execute(createTable);
            connectionString.Close();
        }

        public async void Insert(string month, int hours)
        {
            using var connectionString = new SqliteConnection(@"Data Source = Coding_Tracker.db");

            var sql = "Insert INTO codingGoal (Month, Hours) VALUES (@Date, @Hours)";
            GoalEntity goalEntity = new() { Month = month, Hours=hours};

            var affectedRows = await connectionString.ExecuteAsync(sql, goalEntity);
            Console.WriteLine($"{affectedRows} row(s) inserted");
        }

        public void GoalProgress(string month)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);
            var sql = @$"SELECT sum(Hours), 
                        FROM  codingTracker
                        WHERE strftime('%Y-%m', {month})";

            var codingHours = connection.QuerySingle<int>(sql);

            sql = @$"SELECT Hours, 
                        FROM  codingGoal
                        WHERE strftime('%Y-%m', {month})";

            var codingGoal = connection.QuerySingle<int>(sql);
            var remainingHours = codingGoal - codingHours;
            var surpassedHours = codingHours - codingGoal;


            Console.WriteLine("------------------------------------------------\n"); ;

            if(codingHours < codingGoal)
            {
                Console.WriteLine($"Your coding goal this month is ${codingGoal} and you have coded ${codingHours}, you have {remainingHours} left to reach your goal ");
            }
            else
            {
                Console.WriteLine($"You have surpassed your goal with ${surpassedHours}, Weldone Tiger");
            }
           
            Console.WriteLine("------------------------------------------------\n"); ;
        }

        public void AverageTimePerDay(string month, int days)
        {
            string connectionString = @"Data Source = Coding_Tracker.db";
            using var connection = new SqliteConnection(connectionString);

            var sql = @$"SELECT Hours, 
                        FROM  codingGoal
                        WHERE strftime('%Y-%m', {month})";
            var codingGoal = connection.QuerySingle<int>(sql);
            var codePerDay = codingGoal/days;

            Console.WriteLine("------------------------------------------------\n"); ;
            Console.WriteLine($"To achieve your goal this month you have to code a minimun of {codePerDay} hours everdays, GoodLuck");
            Console.WriteLine("------------------------------------------------\n"); ;
        }
    }
}
