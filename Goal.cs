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
        public string Status { get; private set; }
        public TimeSpan GoalTime { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public DateTime UntilDate { get; set; }
        public DateTime StartDate { get; private set; }
        public DateTime FulfillDate { get; private set; }
        private static readonly string _unfulfilledKeyword = "unfulfilled";
        private static readonly string _activeKeyword = "active";
        private static readonly string _inactiveKeyword = "inactive";


        private static readonly GoalsDatabase _goalsDatabase;

        static Goal()
        {
            try
            {
                ConfigReader configReader = new();
                string connectionString = configReader.GetConnectionString();
                string fileName = configReader.GetFileNameString();
                _goalsDatabase = new(connectionString, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }


        }
        public Goal(TimeSpan goalTime, DateTime untilDate) : this(0, goalTime, TimeSpan.Zero, untilDate, DateTime.Now, DateTime.MinValue, _activeKeyword) { }

        public Goal(int id, TimeSpan goalTime, TimeSpan currentTime, DateTime untilDate, DateTime startDate, DateTime fulfillDate, string status)
        {
            Id = id;
            GoalTime = goalTime;
            CurrentTime = currentTime;
            UntilDate = untilDate;
            StartDate = startDate;
            FulfillDate = fulfillDate;
            Status = status;
        }

        public void SaveToDatabase()
        {
            DateTime modifiedUntilDay = new DateTime(
                UntilDate.Year,
                UntilDate.Month,
                UntilDate.Day,
                23, 59, 59);

            _goalsDatabase.Insert(
                GoalTime.ToString(),
                CurrentTime.ToString(),
                StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                modifiedUntilDay.ToString("yyyy-MM-dd HH:mm:ss"),
                _unfulfilledKeyword);

            UserInput.DisplayMessage("Goal saved.");
        }

        public static List<Goal> GetAllGoals()
        {
            return _goalsDatabase.GetAll();
        }

        public static List<Goal> GetActiveGoals()
        {
            return _goalsDatabase.GetActiveGoals();
        }

        public static void ValidateDatabase()
        {
            List<Goal> goalsList = _goalsDatabase.GetActiveGoals();
            if (goalsList.Count != 0)
            {
                foreach (var goal in goalsList)
                {
                    if (goal.UntilDate <= DateTime.Now)
                    {
                        _goalsDatabase.UpdateStatus(goal.Id, _inactiveKeyword);
                        _goalsDatabase.UpdateFulfilledDate(goal.Id, _unfulfilledKeyword);
                    }
                }
            }

        }
        public static void UpdateGoals(TimeSpan duration, DateTime sessionStartTime)
        {
            List<Goal> goalsList = _goalsDatabase.GetAll();

            if (goalsList.Count != 0)
            {
                bool firstIteration = true;
                for (int i = 0; i < goalsList.Count; i++)
                {
                    if (goalsList[i].StartDate < sessionStartTime && goalsList[i].UntilDate > sessionStartTime)
                    {
                        if (goalsList[i].UntilDate > DateTime.Now)
                        {
                            goalsList[i].CurrentTime += duration;
                            _goalsDatabase.UpdateCurrentTime(goalsList[i].Id, goalsList[i].CurrentTime.ToString());

                            if (goalsList[i].CurrentTime >= goalsList[i].GoalTime && goalsList[i].Status == _activeKeyword)
                            {
                                if(firstIteration)
                                {
                                    Console.Clear();
                                    firstIteration = false;
                                }
                                UserInterface.GoalReached(goalsList[i].Id);

                                _goalsDatabase.UpdateFulfilledDate(goalsList[i].Id, DateTime.Now.ToString());
                                _goalsDatabase.UpdateStatus(goalsList[i].Id, _inactiveKeyword);
                            }
                        }
                        else
                            _goalsDatabase.UpdateStatus(goalsList[i].Id, _inactiveKeyword);
                    }
                }
            }
        }
        public static void UpdateGoalsAfterUpdate(TimeSpan durationDifference, TimeSpan originalDuration, DateTime originalSessionStart, DateTime newSessionStart)
        {
            List<Goal> goalsList = _goalsDatabase.GetAll();
            if (goalsList.Count != 0)
            {
                foreach (var goal in goalsList)
                {
                    if ((goal.StartDate < originalSessionStart && originalSessionStart < goal.UntilDate) ||
                        (goal.StartDate < newSessionStart && newSessionStart < goal.UntilDate)) //excludes all sessions that started before the goal was set or after it expired, includes sessions that are now in range
                    {
                        if (newSessionStart < goal.StartDate) //if the session's new time is prior to start of the goal
                            goal.CurrentTime -= originalDuration;

                        else if (newSessionStart > goal.StartDate && originalSessionStart < goal.StartDate)
                            goal.CurrentTime += originalDuration;

                        else
                            goal.CurrentTime += durationDifference;

                        _goalsDatabase.UpdateCurrentTime(goal.Id, goal.CurrentTime.ToString());


                        if (goal.CurrentTime >= goal.GoalTime) //handles goals that are now fulfilled
                        {
                            _goalsDatabase.UpdateFulfilledDate(goal.Id, DateTime.Now.ToString());

                            if (goal.Status == _activeKeyword) //shows goal message only if wasn't finished before session update
                            {
                                UserInterface.GoalReached(goal.Id);
                                _goalsDatabase.UpdateStatus(goal.Id, _inactiveKeyword);
                            }
                        }
                        else if (goal.UntilDate > DateTime.Now) //handles goals that are still before deadline
                        {
                            _goalsDatabase.UpdateFulfilledDate(goal.Id, _unfulfilledKeyword);
                            _goalsDatabase.UpdateStatus(goal.Id, _activeKeyword);
                        }
                        else //handles goals that are expired
                        {
                            _goalsDatabase.UpdateFulfilledDate(goal.Id, _unfulfilledKeyword);
                            _goalsDatabase.UpdateStatus(goal.Id, _inactiveKeyword);
                        }
                    }
                }
            }
        }

        public static void UpdateGoalsAfterDelete(TimeSpan duration, DateTime originalSessionStart)
        {
            List<Goal> goalsList = _goalsDatabase.GetAll();
            if (goalsList.Count != 0)
            {
                foreach (var goal in goalsList)
                {
                    if (goal.StartDate < originalSessionStart && originalSessionStart < goal.UntilDate) //excludes all sessions that started before the goal was set or after it expired
                    {
                        goal.CurrentTime -= duration;
                        _goalsDatabase.UpdateCurrentTime(goal.Id, goal.CurrentTime.ToString());

                        if (goal.CurrentTime >= goal.GoalTime) //handles goals that are still fulfilled
                        {
                            _goalsDatabase.UpdateStatus(goal.Id, _inactiveKeyword);
                        }
                        else if (goal.UntilDate > DateTime.Now) //handles goals that are still going
                        {
                            _goalsDatabase.UpdateFulfilledDate(goal.Id, _unfulfilledKeyword);
                            _goalsDatabase.UpdateStatus(goal.Id, _activeKeyword);
                        }
                        else //handles goals that are expired
                        {
                            _goalsDatabase.UpdateFulfilledDate(goal.Id, _unfulfilledKeyword);
                            _goalsDatabase.UpdateStatus(goal.Id, _inactiveKeyword);
                        }
                    }
                }
            }
        }
    }
}
