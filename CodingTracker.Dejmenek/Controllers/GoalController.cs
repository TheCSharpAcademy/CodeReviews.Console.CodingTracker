using CodingTracker.Dejmenek.DataAccess.Repositories;
using CodingTracker.Dejmenek.Models;
using CodingTracker.Dejmenek.Services;
using Spectre.Console;

namespace CodingTracker.Dejmenek.Controllers
{
    public class GoalController
    {
        private readonly UserInteractionService _userInteractionService;
        private readonly GoalRepository _goalRepository;

        public GoalController(UserInteractionService userInteractionService, GoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
            _userInteractionService = userInteractionService;
        }

        public void AddGoal()
        {
            string startDate = _userInteractionService.GetDate();
            string endDate = _userInteractionService.GetDate();

            while (!Validation.IsChronologicalOrder(DateTime.Parse(startDate), DateTime.Parse(endDate)))
            {
                AnsiConsole.MarkupLine("The ending time should always be after the starting time. Try again.");
                startDate = _userInteractionService.GetDate();
                endDate = _userInteractionService.GetDate();
            }

            int targetDuration = _userInteractionService.GetDuration();

            _goalRepository.AddGoal(startDate, endDate, targetDuration);
        }

        public void DeleteGoal()
        {
            int id = _userInteractionService.GetId();

            _goalRepository.DeleteGoal(id);
        }

        public void UpdateGoal()
        {
            int id = _userInteractionService.GetId();
            int targetDuration = _userInteractionService.GetDuration();

            _goalRepository.UpdateGoal(id, targetDuration);
        }

        public List<Goal> GetAllGoals()
        {
            return _goalRepository.GetAllGoals();
        }

        public IEnumerable<(int targetDuration, int durationSum)> GetGoalProgress()
        {
            int id = _userInteractionService.GetId();
            return _goalRepository.GetGoalProgress(id);
        }
    }
}
