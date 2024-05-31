using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using System;
using System.Linq;

namespace Patryk_MM.Console.CodingTracker.Queries.Goal {
    /// <summary>
    /// Handles retrieving a coding goal for the current month.
    /// </summary>
    public class GetGoalHandler {
        private readonly TrackerService _trackerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGoalHandler"/> class with the specified <paramref name="trackerService"/>.
        /// </summary>
        /// <param name="trackerService">The TrackerService instance to be used for retrieving goals.</param>
        public GetGoalHandler(TrackerService trackerService) {
            _trackerService = trackerService;
        }

        /// <summary>
        /// Retrieves a coding goal for the current month.
        /// </summary>
        /// <returns>The coding goal for the current month, or null if not found.</returns>
        public CodingGoal? Handle() {
            return _trackerService.GetGoals().FirstOrDefault(goal => goal.YearAndMonth.Month == DateTime.Now.Month);
        }
    }
}
