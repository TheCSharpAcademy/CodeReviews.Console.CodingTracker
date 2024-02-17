namespace CodingTracker;

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
    private static bool _consoleCleared;

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

    public static List<Goal> GetAllGoals() => _goalsDatabase.GetAll();

    public static List<Goal> GetActiveGoals() => _goalsDatabase.GetActiveGoals();

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
        _consoleCleared = false;

        foreach (var goal in goalsList)
        {
            bool goalUnfulfilled = goal.UntilDate > DateTime.Now;

            if (IsSessionWithinGoal(goal, sessionStartTime))
            {
                if (goalUnfulfilled)
                {
                    goal.CurrentTime += duration;
                    _goalsDatabase.UpdateCurrentTime(goal.Id, goal.CurrentTime.ToString());

                    UpdateGoalStatusAndFulfilledDate(goal);
                }
            }
        }
    }

    public static void UpdateGoalsAfterUpdate(TimeSpan durationDifference, TimeSpan originalDuration, DateTime originalSessionStart, DateTime newSessionStart)
    {
        List<Goal> goalsList = _goalsDatabase.GetAll();
        _consoleCleared = false;

        if (goalsList.Count != 0)
        {
            foreach (var goal in goalsList)
            {
                if (IsSessionWithinGoal(goal, originalSessionStart) || IsSessionWithinGoal(goal, newSessionStart))
                {
                    UpdateCurrentGoalTime(goal, originalDuration, durationDifference, newSessionStart, originalSessionStart);
                    UpdateGoalStatusAndFulfilledDate(goal);
                }
            }
        }
    }

    public static void UpdateGoalsAfterDelete(TimeSpan duration, DateTime originalSessionStart)
    {
        List<Goal> goalsList = _goalsDatabase.GetAll();
        _consoleCleared = false;

        if (goalsList.Count != 0)
        {
            foreach (var goal in goalsList)
            {
                if (IsSessionWithinGoal(goal, originalSessionStart)) //excludes all sessions that started before the goal was set or after it expired
                {
                    goal.CurrentTime -= duration;
                    _goalsDatabase.UpdateCurrentTime(goal.Id, goal.CurrentTime.ToString());

                    UpdateGoalStatusAndFulfilledDate(goal);
                }
            }
        }
    }

    private static bool IsSessionWithinGoal(Goal goal, DateTime sessionStart)
    {
        return goal.StartDate < sessionStart && sessionStart < goal.UntilDate;
    }

    private static void UpdateCurrentGoalTime(Goal goal, TimeSpan originalDuration, TimeSpan durationDifference, DateTime newSessionStart, DateTime originalSessionStart)
    {
        if (newSessionStart < goal.StartDate)
            goal.CurrentTime -= originalDuration;

        else if (newSessionStart > goal.StartDate && originalSessionStart < goal.StartDate)
            goal.CurrentTime += originalDuration;

        else
            goal.CurrentTime += durationDifference;

        _goalsDatabase.UpdateCurrentTime(goal.Id, goal.CurrentTime.ToString());
    }

    private static void UpdateGoalStatusAndFulfilledDate(Goal goal)
    {
        bool goalFulfilled = goal.CurrentTime >= goal.GoalTime;
        bool goalUnfulfilled = goal.UntilDate > DateTime.Now;
        bool goalActive = goal.Status == _activeKeyword;

        if (goalFulfilled)
        {
            if (goalActive)
                SetReached(goal);
        }
        else if (goalUnfulfilled)
            SetUnfulfilled(goal);

        else //handles goals that are expired
            SetExpired(goal);
    }

    private static void SetExpired(Goal goal)
    {
        _goalsDatabase.UpdateFulfilledDate(goal.Id, _unfulfilledKeyword);
        _goalsDatabase.UpdateStatus(goal.Id, _inactiveKeyword);
    }

    private static void SetUnfulfilled(Goal goal)
    {
        _goalsDatabase.UpdateFulfilledDate(goal.Id, _unfulfilledKeyword);
        _goalsDatabase.UpdateStatus(goal.Id, _activeKeyword);
    }
    
    private static void SetReached(Goal goal)
    {
        if (!_consoleCleared)
        {
            Console.Clear();
            _consoleCleared = true;
        }

        UserInterface.GoalReached(goal.Id);
        _goalsDatabase.UpdateStatus(goal.Id, _inactiveKeyword);
        _goalsDatabase.UpdateFulfilledDate(goal.Id, DateTime.Now.ToString());
    }
}