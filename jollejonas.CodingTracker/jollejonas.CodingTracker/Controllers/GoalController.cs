using System.Data;
using Dapper;
using jollejonas.CodingTracker.Models;

namespace jollejonas.CodingTracker.Controllers
{
    public class GoalController
    {
        private readonly IDbConnection _db;

        public GoalController(IDbConnection db)
        {
            _db = db;
        }

        public Goal GetGoal()
        {
            string sql = "SELECT * FROM Goals WHERE Id = @Id";
            return _db.QueryFirstOrDefault<Goal>(sql, new {Id = "1"});
        }
        public void SetGoal()
        {
            double duration = 0;
            while (true)
            {
                Console.WriteLine("Enter your goal in hours:");
                string durationInput = Console.ReadLine();
                if (double.TryParse(durationInput, out duration))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a number.");
            }
            Goal goal = GetGoal();
            if (goal != null)
            {
                string updateGoalQuery = "UPDATE Goals SET Duration = @Duration WHERE Id = @Id";
                _db.Execute(updateGoalQuery, new { Duration = duration, Id = 1 });
                return;
            }
            string insertGoalQuery = "INSERT INTO Goals (Duration) VALUES (@Duration)";
            _db.Execute(insertGoalQuery, new { Duration = duration, Id = 1 });
        }

        public string CalculateGoalActualDifference()
        {
            Goal goal = GetGoal();
            DateTime now = DateTime.Now;
            int daysSinceMonday = (int)now.DayOfWeek - (int)DayOfWeek.Monday;
            int daysLeftInWeek = ((int)DayOfWeek.Sunday - (int)now.DayOfWeek + 7) % 7;

            if(daysLeftInWeek == 0)
            {
                daysLeftInWeek = 7;
            }
            DateTime startofWeek = now.AddDays(-daysSinceMonday);
            if (goal == null)
            {
                return "No goal set\n";
            }
            string sql = "SELECT SUM(Duration) FROM CodingSessions WHERE StartTime >= @StartDate AND EndTime <= @EndDate";
            double totalDuration = _db.QueryFirstOrDefault<double>(sql, new {StartDate = startofWeek, EndDate = now});
            double difference = goal.Duration - totalDuration;
            double codingNeededPerDay = difference / daysLeftInWeek;
            return $"Goal: {goal.Duration} hours per week\n" +
                $"Total duration: {totalDuration} hours per week\n" +
                $"Difference: {difference} hours\n" +
                $"You need to code {codingNeededPerDay} hours per day, the rest of the week, to reach you goal.";

        }
    }
}
