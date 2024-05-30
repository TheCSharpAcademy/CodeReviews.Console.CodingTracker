using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patryk_MM.Console.CodingTracker.Queries.Goal {
    public class GetGoalHandler {
        private readonly TrackerService _trackerService;

        public GetGoalHandler(TrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public CodingGoal? Handle() {
            return _trackerService.GetGoals().FirstOrDefault(goal => goal.YearAndMonth.Month == DateTime.Now.Month);
        }
    }
}
