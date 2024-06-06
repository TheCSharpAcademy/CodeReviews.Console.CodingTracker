using Spectre.Console;
using System.Configuration;

public class App
{
    private DatabaseManager _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString);
    private UserInput _userInput;
    private LiveTracker _liveTracker;
    private CodingRepository _codingRepository;
    private GoalRepository _goalRepository;
    private ValidateInput _validateInput;

    public void Run()
    {
        _databaseManager.InitializeDatabase();
        _validateInput = new ValidateInput();
        _userInput = new UserInput(_validateInput);
        _codingRepository = new CodingRepository(_databaseManager);
        _goalRepository =  new GoalRepository(_databaseManager);

        while (true)
        {
            var mainMenuOptions = _userInput.MainMenu();

            switch (mainMenuOptions)
            {
                case MainMenuOptions.NewSession:
                    NewSession();
                    break;
                case MainMenuOptions.AddManualSession: 
                    AddManualSession();
                    break;
                case MainMenuOptions.ViewSessions: 
                    ViewSessions();
                    break;
                case MainMenuOptions.Goals: 
                    ManageGoals();
                    break;
                case MainMenuOptions.InsertTestData:
                    InsertTestData();
                    break; 
                case MainMenuOptions.Exit: 
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void NewSession()
    {
        _liveTracker = new LiveTracker();
        bool exit = false;

        while (!exit)
        {
            var newSessionOptions = _userInput.NewSessionMenu(_liveTracker.GetTime(), _liveTracker.Stopwatch.IsRunning);

            switch (newSessionOptions)
            {
                case NewSessionOptions.Start:
                    _liveTracker.Start();
                    break;
                case NewSessionOptions.Reset:
                    _liveTracker.Reset();
                    break;
                case NewSessionOptions.Update:
                    break;
                case NewSessionOptions.Exit:
                    CodingSession codingSession = _liveTracker.Save();
                    _codingRepository.AddSession(codingSession);
                    exit = true;
                    return;
            }
        }
    }

    private void AddManualSession()
    {
        CodingSession codingSession = _userInput.AddManualSession();

        if (codingSession.EndTime > codingSession.StartTime)
        {
            _codingRepository.AddSession(codingSession);
        }
    }

    private void ViewSessions()
    {
        var getSessions = _codingRepository.GetSessions();

        if (_codingRepository.GetSessions().Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No entries in database to filter. Enter any key to continue.[/]");
            Console.ReadKey(true);
        }
        else
        {
            var sessionList = _codingRepository.GetSessions();
            var sortOrber = _userInput.GetSortingOrder();
            var filter = _userInput.FilteringOptionsMenu();

            switch (filter)
            {
                case FilteringOptions.Dates:
                    var option = _userInput.GetUserFilterPeriod();
                    int length = _userInput.FilterDuration(option);
                    var filterSession = sessionList.Where(e => e.StartTime >= DateTime.Today.AddDays(-length));
                    _userInput.OutputSessions(filterSession, sortOrber);
                    break;
                case FilteringOptions.All:
                    _userInput.OutputSessions(sessionList, sortOrber);
                    break;
            }
        }

        
    }

    private void ManageGoals()
    {
        var codingGoals = _goalRepository.GetCodingGoals();
        var goalOption = _userInput.GoalMenuOptions();
        var codingSessions = _codingRepository.GetSessions();

        switch (goalOption)
        {
            case GoalOptions.NewGoal:
                var goalHours = _userInput.SetNewGoal();
                _goalRepository.AddGoal(goalHours);
                break;
            case GoalOptions.CurrentGoals:
                UpdateGoals(codingGoals, codingSessions); // complete goals

                if (codingGoals.Any())
                {
                    _userInput.DisplayGoalList(codingGoals);
                    int goalPicked = _validateInput.GoalPicked(codingGoals);

                    var selectedGoal = codingGoals.First(x => x.Id == goalPicked);
                    int totalHours = codingSessions
                        .Where(cs => cs.StartTime >= selectedGoal.StartDate && 
                        (selectedGoal.EndDate == null || cs.StartTime <= selectedGoal.EndDate))
                        .Sum(cs => (int)cs.Duration.TotalHours);

                    _userInput.DisplayGoal(selectedGoal, totalHours);
                }
                else
                {
                    AnsiConsole.WriteLine("No goals to show. Press any key to continue");
                    Console.ReadKey();
                }

                break;
            case GoalOptions.DeleteGoal:
                if (codingGoals.Any())
                {
                    _userInput.DisplayGoalList(codingGoals);
                    int goalPicked = _validateInput.GoalPicked(codingGoals);
                    var selectedGoal = codingGoals.First(x => x.Id == goalPicked);
                    _goalRepository.RemoveGoal(selectedGoal);
                    AnsiConsole.WriteLine("Goal removed. Press any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    AnsiConsole.WriteLine("No goals to delete. Press any key to continue");
                    Console.ReadKey();
                }
                break;
            case GoalOptions.InsertTestGoals:
                var testRecords = _userInput.InsertTestData();
                _goalRepository.InsertTestData(testRecords);
                AnsiConsole.MarkupLine($"[green]{testRecords} random records have been inserted[/]");
                Console.ReadKey(true);
                break;
        }
    }

    private void UpdateGoals(List<CodingGoals> codingGoals, List<CodingSession> codingSessions)
    {
        foreach (var goal in codingGoals)
        {
            var completedSessions = codingSessions
                .Where(cs => cs.StartTime >= goal.StartDate && (goal.EndDate == null || cs.StartTime <= goal.EndDate))
                .Sum(cs => (int)cs.Duration.TotalHours);

            if (completedSessions >= goal.TotalHours && goal.EndDate == null)
            {
                goal.EndDate = DateTime.Now;
                goal.CompletionStatus = "Complete";
                _goalRepository.UpdateGoal(goal);
            }
        }
    }

    private void InsertTestData()
    {
        // TODO 
        var number = _userInput.InsertTestData();
        _codingRepository.InsertTestData(number);
        AnsiConsole.MarkupLine($"[green]{number} random records have been inserted[/]");
        Console.ReadKey(true);
    }
}