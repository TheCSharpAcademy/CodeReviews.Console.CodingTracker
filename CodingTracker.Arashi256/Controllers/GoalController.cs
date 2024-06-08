using CodingTracker.Arashi256.Classes;
using CodingTracker.Arashi256.Models;
using Dapper;

namespace CodingTracker.Arashi256.Controllers
{
    internal class GoalController
    {
        private Database _database;

        public GoalController()
        {
            _database = new Database();
        }

        public string AddCodingGoal(DateTime startDT, DateTime? deadlineDT, int hours)
        {
            var codingGoal = new CodingGoal() { Id = null, StartDateTime = startDT, Hours = hours, DeadlineDateTime = (DateTime)deadlineDT };
            int rows = _database.AddNewCodingGoal(codingGoal);
            return $"Added {rows} coding goal";
        }

        public string UpdateCodingGoal(CodingGoal codingGoal)
        {
            int rows = _database.UpdateCodingGoal(codingGoal);
            return $"Updated {rows} coding goal";
        }

        public string DeleteCodingGoal()
        {
            int rows = _database.DeleteCodingGoal();
            return $"Removed {rows} coding goal";
        }

        public CodingGoal? GetCurrentCodingGoal()
        {
            string query = "SELECT Id, StartDateTime, Hours, DeadlineDateTime FROM coding_goal";
            List<CodingGoal> codingGoals = _database.GetCodingGoalResults(query);
            return codingGoals.Count != 1 ? null : codingGoals[0];
        }

        public bool HasCodingGoal()
        {
            List<CodingGoal> codingGoals = _database.GetCodingGoalResults("SELECT * FROM coding_goal");
            return codingGoals.Count > 0 ? true: false;
        }
    }
}
