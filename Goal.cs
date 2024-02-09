using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using CodingTracker;

namespace CodingTracker
{
    public class Goal
    {
        public int Id { get; private set; }
        public TimeSpan GoalTime { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public DateTime UntilDate { get; set; }
        public DateTime StartDate { get; private set; }
        public DateTime FulfillDate { get; private set; }
        public bool IsFullfilled { get; private set; }
        private static readonly GoalsDatabase _goalsDatabase;

        static Goal()
        {
            ConfigReader configReader = new();
            string connectionString = configReader.GetConnectionString();
            string fileName = configReader.GetFileNameString();
            _goalsDatabase = new(connectionString, fileName);
        }
        public Goal(TimeSpan goalTime, DateTime untilDate) : this(0, goalTime, TimeSpan.Zero, untilDate, DateTime.Now, DateTime.MinValue) { }

        public Goal(int id, TimeSpan goalTime, TimeSpan currentTime, DateTime untilDate, DateTime startDate, DateTime fulfillDate)
        {
            Id = id;
            GoalTime = goalTime;
            CurrentTime = currentTime;
            UntilDate = untilDate;
            StartDate = startDate;
            FulfillDate = fulfillDate;
        }

        public void SaveToDatabase()
        {

            _goalsDatabase.Insert(GoalTime.ToString(), CurrentTime.ToString(), StartDate.ToString(), UntilDate.ToString(), "unfulfilled");
        }

        public static List<Goal> GetAllGoals()
        {
            return _goalsDatabase.GetAll();
        }
        public static void UpdateGoals(TimeSpan duration)
        {
            List<Goal> goalsList = _goalsDatabase.GetActiveGoals();
            if (goalsList.Count != 0)
            {
                foreach (var goal in goalsList)
                {
                    if (goal.UntilDate > DateTime.Now)
                    {
                        goal.CurrentTime += duration;
                        _goalsDatabase.UpdateCurrentTime(goal.Id, goal.CurrentTime.ToString());

                        if (goal.CurrentTime >= goal.GoalTime)
                        {
                            _goalsDatabase.UpdateFulfilledDate(goal.Id, DateTime.Now.ToString());
                            _goalsDatabase.UpdateStatus(goal.Id);
                            
                            UserInterface.GoalReached(goal.Id);
                        }
                    }
                }
            }

        }
    }
}
