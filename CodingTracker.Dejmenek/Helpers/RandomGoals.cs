using CodingTracker.Dejmenek.Models;

namespace CodingTracker.Dejmenek.Helpers
{
    public static class RandomGoals
    {
        private static readonly Random _random = new Random();
        public static List<Goal> GenerateRandomGoals()
        {
            List<Goal> goals = new List<Goal>();
            for (int i = 0; i < 3; i++)
            {
                (DateTime startDateTime, DateTime endDateTime) = RandomDate.GetRandomDates();
                int targetDuration = _random.Next(60, 1201);
                goals.Add(new Goal
                {
                    StartDate = startDateTime.ToString("yyyy-MM-dd"),
                    EndDate = endDateTime.ToString("yyyy-MM-dd"),
                    TargetDuration = targetDuration,
                });
            }

            return goals;
        }
    }
}
