using CodingTracker.Dejmenek.DataAccess.Interfaces;
using CodingTracker.Dejmenek.Models;
using Dapper;
using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.Dejmenek.DataAccess.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["CodingTracker"].ConnectionString;

        public void AddGoal(string startDate, string endDate, int targetDuration)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO Goals (StartDate, EndDate, TargetDuration)
                    VALUES (@StartDate, @EndDate, @TargetDuration);
                ";

                connection.Execute(sql, new
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    TargetDuration = targetDuration
                });
            }
        }

        public void DeleteGoal(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    DELETE FROM Goals WHERE Id = @Id;
                ";

                connection.Execute(sql, new
                {
                    Id = id
                });

            }
        }

        public void UpdateGoal(int id, int targetDuration)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    UPDATE Goals SET TargetDuration = @TargetDuration
                    WHERE Id = @Id;
                ";

                connection.Execute(sql, new
                {
                    TargetDuration = targetDuration,
                    Id = id
                });
            }
        }

        public List<Goal> GetAllGoals()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    SELECT * FROM Goals;
                ";

                return connection.Query<Goal>(sql).ToList();
            }
        }

        public IEnumerable<(int targetDuration, int durationSum)> GetGoalProgress(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    SELECT g.TargetDuration, SUM(c.Duration)
                    FROM Goals g
                    LEFT JOIN CodingSessions c
                    ON c.StartDateTime >= datetime(g.StartDate) AND c.EndDateTime <= datetime(g.EndDate) 
                    WHERE g.Id = @Id;
                ";

                return connection.Query<(int targetDuration, int durationSum)>(sql, new
                {
                    Id = id
                });
            }
        }
    }
}
