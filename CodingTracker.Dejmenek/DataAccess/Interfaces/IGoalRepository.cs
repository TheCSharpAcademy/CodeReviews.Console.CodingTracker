using CodingTracker.Dejmenek.Models;

namespace CodingTracker.Dejmenek.DataAccess.Interfaces
{
    public interface IGoalRepository
    {
        void AddGoal(string startDate, string endDate, int targetDuration);
        void DeleteGoal(int id);
        void UpdateGoal(int id, int targetDuration);
        List<Goal> GetAllGoals();
        IEnumerable<(int targetDuration, int durationSum)> GetGoalProgress(int id);
    }
}
