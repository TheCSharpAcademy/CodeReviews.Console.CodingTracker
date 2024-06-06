using System.Collections.Immutable;
using System.Reflection;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class UserInput
{
    private ValidateInput _validateInput;

    public UserInput(ValidateInput validateInput)
    {
        _validateInput = validateInput;
    }

    public void Header()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("CodingTracker").Color(Color.Orange1));
    }

    public MainMenuOptions MainMenu()
    {
        Header();

        var optionDescriptions = new Dictionary<string, MainMenuOptions>
        {
            { "Start a new session", MainMenuOptions.NewSession },
            { "Add a session manually", MainMenuOptions.AddManualSession },
            { "View previous sessions", MainMenuOptions.ViewSessions },
            { "Add, update or view goals", MainMenuOptions.Goals },
            { "Insert Test Data", MainMenuOptions.InsertTestData },
            { "Exit the application", MainMenuOptions.Exit }
        };

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(10)
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];
    }

    public NewSessionOptions NewSessionMenu(TimeSpan elapsedTime, bool isRunning)
    {
        Header();

        var optionDescriptions = new Dictionary<string, NewSessionOptions>
        {
            { "Start", NewSessionOptions.Start },
            { "Reset", NewSessionOptions.Reset },
            { "Get Updated Time", NewSessionOptions.Update },
            { "Save & Exit to main menu", NewSessionOptions.Exit },
        };

        var panel = new Panel($"Coding Time: {elapsedTime}\nRunning: {isRunning}")
        {
            Header = new PanelHeader("Session Tracker"),
            Border = BoxBorder.Square
        };
        AnsiConsole.Write(panel);


        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(10)
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];
    }

    public CodingSession AddManualSession()
    {
        Header();
        DateTime startDate;
        DateTime endDate;

        do
        {
            var startYear = _validateInput.GetValidInt("In what year did your coding session start? (e.g., 2024)", DateTime.Now.Year, DateTime.Now.Year + 1);
            var startMonth = _validateInput.GetValidInt("In what month did your coding session start? (1 for January, 12 for December)", 1, 12);
            var startDay = _validateInput.GetValidInt("On what day did your coding session start? (1-31)", 1, 31);
            var startTime = _validateInput.GetValidTime("What time did you start your session? (hh:mm:ss)");

            var endYear = _validateInput.GetValidInt("In what year did your coding session end? (e.g., 2024)", DateTime.Now.Year, DateTime.Now.Year + 1);
            var endMonth = _validateInput.GetValidInt("In what month did your coding session end? (1 for January, 12 for December)", 1, 12);
            var endDay = _validateInput.GetValidInt("On what day did your coding session end? (1-31)", 1, 31);
            var endTime = _validateInput.GetValidTime("What time did you end your session? (hh:mm:ss)");

            startDate = new DateTime(startYear, startMonth, startDay, startTime.Hours, startTime.Minutes, startTime.Seconds);
            endDate = new DateTime(endYear, endMonth, endDay, endTime.Hours, endTime.Minutes, endTime.Seconds);

            if (!_validateInput.ValidateSessionDates(startDate, endDate))
            {
                AnsiConsole.Markup("[red]End date must be after start date.[/]");
            }

        } while (!_validateInput.ValidateSessionDates(startDate, endDate));


        CodingSession codingSession = new CodingSession();
        codingSession.StartTime = startDate;
        codingSession.EndTime = endDate;
        codingSession.Duration = codingSession.EndTime - codingSession.StartTime;

        return codingSession;
    }

    public int InsertTestData() => _validateInput.GetValidInt($"How many test records would you like to insert? (1-1000)", 1, 1000);

    public SortBy GetSortingOrder()
    {
        Header();

        var optionDescriptions = new Dictionary<string, SortBy>
        {
            { "Sort by Ascending", SortBy.Ascending },
            { "Sort by Decending", SortBy.Descending },
        };

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];

    }

    public FilteringOptions FilteringOptionsMenu()
    {
        Header();

        var optionDescriptions = new Dictionary<string, FilteringOptions>
        {
            { "Filter by date range", FilteringOptions.Dates },
            { "Show all", FilteringOptions.All },
        };

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];

    }

    public void OutputSessions(IEnumerable<CodingSession> codingSessions, SortBy sortOrder)
    {
        Header();

        var table = new Table();

        foreach (PropertyInfo p in typeof(CodingSession).GetProperties())
        {
            table.AddColumn(new TableColumn(p.Name).Centered());
        }

        IEnumerable<CodingSession> sortedSessions = sortOrder switch
        {
            SortBy.Descending => codingSessions.OrderByDescending(x => x.StartTime),
            SortBy.Ascending => codingSessions.OrderBy(x => x.StartTime),
            _ => codingSessions
        };

        foreach (var item in sortedSessions)
        {
            table.AddRow(item.Id.ToString(), item.StartTime.ToString(), item.EndTime.ToString(), item.Duration.ToString());
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("Press any key to go back to main menu.");
        Console.ReadKey();
    }

    public FilterByPeriod GetUserFilterPeriod()
    {
        var optionDescriptions = new Dictionary<string, FilterByPeriod>
        {
            { "Days", FilterByPeriod.Days },
            { "Weeks", FilterByPeriod.Weeks },
            { "Years", FilterByPeriod.Years },
        };

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];
    }

    public int FilterDuration(FilterByPeriod filter)
    {
        Header();

        var number = _validateInput.GetValidInt($"How long would you like to filter", 1, 1000);

        switch (filter)
        {
            case FilterByPeriod.Days:
                return number;
            case FilterByPeriod.Weeks:
                return number * 7;
            case FilterByPeriod.Years:
                return number * 365;
        }
        return 0;
    }

    public GoalOptions GoalMenuOptions()
    {
        Header();

        var optionDescriptions = new Dictionary<string, GoalOptions>
        {
            { "Add a new goal", GoalOptions.NewGoal },
            { "View Current Goals", GoalOptions.CurrentGoals },
            { "Remove a goal", GoalOptions.DeleteGoal },
            { "Insert test goals", GoalOptions.InsertTestGoals },
        };

        var options = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(optionDescriptions.Keys));

        return optionDescriptions[options];

    }

    public int SetNewGoal() => _validateInput.GetValidInt($"How many hours do you want your new goal to be? (1-1000)", 1, 1000);

    public void DisplayGoalList(List<CodingGoals> codingGoals)
    {
        Header();

        var table = new Table();

        foreach (PropertyInfo p in typeof(CodingGoals).GetProperties())
        {
            table.AddColumn(new TableColumn(p.Name).Centered());
        }

        foreach (var item in codingGoals)
        {
            table.AddRow(item.Id.ToString(), item.TotalHours.ToString(), item.StartDate.ToString(), item.EndDate.ToString(), item.CompletionStatus);
        }

        AnsiConsole.Write(table);
    }

    public void DisplayGoal(CodingGoals codingGoal, int currentHours)
    {
        Header();

        double completionPercentage = (double)currentHours / codingGoal.TotalHours * 100;
        double remainingPercentage = 100 - completionPercentage;

        if (completionPercentage + remainingPercentage != 100) remainingPercentage = 100 - completionPercentage;
        
        var progress = new BreakdownChart()
            .HideTags()
            .Width(100)
            .AddItem("Complete", (int)completionPercentage, Color.Green)
            .AddItem("Still to do", (int)remainingPercentage, Color.Red);

        var startDate = $"[bold]Start Date:[/] {codingGoal.StartDate}";
        var goalHours = $"[bold]Hours to reach:[/] {codingGoal.TotalHours}";
        var displayCurrentHours = $"[bold]Current hours:[/] {currentHours}"; 
        var endDate = $"[bold]Completed Date:[/] {codingGoal.EndDate}";

        var grid = new Grid();
        grid.AddColumn(new GridColumn().NoWrap());
        grid.AddColumn();

        grid.AddRow(startDate);
        grid.AddRow(goalHours);
        grid.AddRow(displayCurrentHours);
        grid.AddRow(progress);
        grid.AddRow(endDate);

        var panel = new Panel(grid)
        {
            Header = new PanelHeader("Goal Progress"),
            Padding = new Padding(2, 2, 2, 2)
        };

        AnsiConsole.Write(panel);

        AnsiConsole.WriteLine("Press any key to go back to main menu.");
        Console.ReadKey();
    }
}