using Spectre.Console;
using System.Diagnostics;
using System.Globalization;


namespace CodingTracker;

public class Controller
{
    static bool timerRunning = true;
    static DateTime todayDate = DateTime.Now;

    internal static void Run()
    {
        bool endApp = false;
        while (!endApp)
        {
            bool isEmpty = CheckDatabase();

            string? input = Views.MainMenu();

            switch (input)
            {
                case "Quit":
                    endApp = true;
                    Environment.Exit(0);
                    break;
                case "Start New Session":
                    TimedSessionHandler();
                    break;
                case "Insert New Session":
                    InsertHandler();
                    break;
                case "Update Session":
                    UpdateHandler(isEmpty);
                    break;
                case "Delete Session":
                    DeleteHandler(isEmpty);
                    break;
                case "View Sessions":
                    ViewHandler(isEmpty);
                    break;
                case "Generate Report":
                    ReportHandler();
                    break;
                case "Coding Goals":
                    GoalsMenu();
                    break;
                default:
                    Console.WriteLine("Please select an option from the menu.");
                    break;
            }
        }
    }

    // Methods for session handling
    internal static void InsertHandler()
    {
        AnsiConsole.Markup("Press [yellow]Q[/] to cancel.\n");
        string? date = UserInput.GetDate();
        string? language = Views.SelectLanguage();
        string? startTime = UserInput.GetTime("start time");
        string? endTime = UserInput.GetTime("end time");
        string? formattedStartTime = Helpers.ParseTimeWithSeconds(startTime);
        string? formattedEndTime = Helpers.ParseTimeWithSeconds(endTime);
        string? duration = Helpers.CalculateDuration(formattedStartTime, formattedEndTime);

        DateTime parsedDate = DateTime.ParseExact(date, "dd/MM/yy", new CultureInfo("en-US"), DateTimeStyles.None);
        string convertedDate = parsedDate.ToString("yyyy/MM/dd");

        bool sessionAdded = Model.InsertSession(convertedDate, language, formattedStartTime, formattedEndTime, duration);

        if (sessionAdded)
        {
            Views.ShowMessage("Session was added.");
            Views.ShowMessage("Press any key to continue.");
            Console.ReadLine();
        }
        else
        {
            Views.ShowError("Something went wrong. Session was not added.");
        }
    }

    internal static void UpdateHandler(bool isEmpty)
    {
        if (!isEmpty)
        {
            ShowSessions();
            string? id = UserInput.GetId();
            string? column = Views.SelectColumn();

            if (column == "Back")
            {
                Run();
            }

            string? value = UserInput.GetUpdatedValue(column);

            // Checks if duration could be successfully and accurately updated
            if (column == "StartTime" || column == "EndTime")
            {
                bool validTime = false;

                string parsedTime = Helpers.ParseTimeWithSeconds(value);
                string updatedDuration = "";

                do
                {
                    string? unchangedTimeColumn = column == "StartTime" ? "EndTime" : "StartTime";
                    string? unchangedTime = Model.FetchValue(id, unchangedTimeColumn, "coding_sessions");

                    // Checks sessions times before updating (e.g. makes sure updated start time is before end time)
                    bool checkedTime = Helpers.CheckTime(column, parsedTime, unchangedTime);

                    if (!checkedTime)
                    {
                        Views.ShowError("Please ensure you're inputting the correct time period, e.g., ensuring an updated start time is before the end time.");

                        value = UserInput.GetUpdatedValue(column);
                    }
                    else
                    {
                        updatedDuration = Helpers.RecalculateDuration(column, parsedTime, unchangedTime);
                        validTime = true;
                    }
                } while (!validTime);

                // Updates the session if checked time is valid
                bool sessionUpdated = Model.UpdateSession(id, column, parsedTime, updatedDuration);

                if (sessionUpdated)
                {
                    Views.ShowMessage("Session was updated.");
                    Views.ShowMessage("Press any key to continue.");
                    Console.ReadLine();
                }
                else
                {
                    Views.ShowError("Something went wrong. Session was not updated.");
                }
            }

            if (column == "Date")
            {
                DateTime parsedDate = DateTime.ParseExact(value, "dd/MM/yy", new CultureInfo("en-US"), DateTimeStyles.None);
                string convertedDate = parsedDate.ToString("yyyy/MM/dd");

                bool sessionUpdated = Model.UpdateSession(id, column, convertedDate);

                if (sessionUpdated)
                {
                    Views.ShowMessage("Session was updated.");
                    Views.ShowMessage("Press any key to continue.");
                    Console.ReadLine();
                }
                else
                {
                    Views.ShowError("Something went wrong. Session was not updated.");
                }
            }
        }
        else
        {
            Views.ShowMessage("Database is empty. Press any key to return to menu.\n");
            Console.ReadLine();
        }
    }

    internal static void DeleteHandler(bool isEmpty)
    {
        if (!isEmpty)
        {
            ShowSessions();
            string? id = null;
            string? input = Views.DeleteMenu();

            switch (input)
            {
                case "Delete Session":
                    id = UserInput.GetId();
                    break;
                case "Clear Database":
                    id = "*";
                    break;
                case "Cancel":
                    Run();
                    break;
                default:
                    break;
            }

            bool sessionDeleted = Model.DeleteSession(id);

            if (sessionDeleted)
            {
                Views.ShowMessage("Session(s) deleted.");
                Views.ShowMessage("Press any key to continue.");
                Console.ReadLine();
            }
            else
            {
                Views.ShowError("Something went wrong. Session was not deleted.");
            }
        }
        else
        {
            Views.ShowMessage("Database is empty. Press any key to return to menu.\n");
            Console.ReadLine();
        }
    }

    internal static void ViewHandler(bool isEmpty)
    {
        if (!isEmpty)
        {
            ShowSessions();

            // Add menu for filtering results
            string filterSelected = Views.FilterSessionsMenu();

            switch (filterSelected)
            {
                case "Filter by Days":
                    FilterHandler("1");
                    break;
                case "Filter by Weeks":
                    FilterHandler("2");
                    break;
                case "Filter by Months":
                    FilterHandler("3");
                    break;
                case "Filter by Ascending":
                    FilterHandler("4");
                    break;
                case "Filter by Descending":
                    FilterHandler("5");
                    break;
                default:
                    break;
            }

            Views.ShowMessage("Press any key to continue.\n");
            Console.ReadLine();
        }
        else
        {
            Views.ShowMessage("Database is empty. Press any key to return to menu.\n");
            Console.ReadLine();
        }
    }

    internal static void ShowSessions()
    {
        List<CodingSession> sessions = Model.FetchSessions();
        TableVisualizationEngine.DisplaySessionsTable(sessions);
    }

    internal static bool CheckDatabase()
    {
        List<CodingSession> sessions = Model.FetchSessions();

        bool databaseEmpty = (sessions.Count == 0) ? true : false;

        return databaseEmpty;
    }

    // Methods for filtering
    internal static void FilterHandler(string? optionSelected)
    {        
        string endDate = todayDate.ToString("yyyy/MM/dd");
        string startDate = "";
        string? daysInputted = "";

        List<CodingSession> sortedSessions = new();

        switch (optionSelected)
        {
            case "1":
                Views.ShowMessage($"Please enter the number of days you want to filter by.\n");
                daysInputted = UserInput.GetNumber();
                startDate = todayDate.AddDays(-Convert.ToDouble(daysInputted)).ToString("yyyy/MM/dd");
                break;
            case "2":
                Views.ShowMessage($"Please enter the number of weeks you want to filter by.\n");
                daysInputted = UserInput.GetNumber();
                startDate = todayDate.AddDays(-7 * Convert.ToDouble(daysInputted)).ToString("yyyy/MM/dd");
                break;
            case "3":
                Views.ShowMessage($"Please enter the number of months you want to filter by.\n");
                daysInputted = UserInput.GetNumber();
                startDate = todayDate.AddMonths(-Convert.ToInt32(daysInputted)).ToString("yyyy/MM/dd");
                break;
            case "4":
                sortedSessions = Model.SortSessions("ASC");
                break;
            case "5":                
                sortedSessions = Model.SortSessions("DESC");
                break;
            default:
                break;
        }

        bool sortingSelected = (optionSelected == "4" || optionSelected == "5") ? true : false;

        Views.ShowSpinner();

        Console.Clear();
        Views.ShowBanner();

        if (!sortingSelected)
        {
            List<CodingSession> filteredSessions = Model.FilterSessions(startDate, endDate);

            if (filteredSessions.Count > 0)
            {
                TableVisualizationEngine.DisplaySessionsTable(filteredSessions);
            }
            else
            {
                Views.ShowMessage("Returned no results.");
            }
        }
        else
        {
            if (sortedSessions.Count > 0)
            {
                TableVisualizationEngine.DisplaySessionsTable(sortedSessions);
            }
            else
            {
                Views.ShowMessage("Returned no results.");
            }
        }
    }

    // Methods for goal handling
    internal static void GoalsMenu()
    {
        bool quitGoalsMenu = false;

        while (!quitGoalsMenu)
        {
            string? input = Views.GoalsMenu();

            bool isGoalsEmpty = CheckGoalsDatabase();

            switch (input)
            {
                case "View Current Goals":
                    GoalViewHandler(isGoalsEmpty);
                    break;
                case "Create Goal":
                    GoalInsertHandler();
                    break;
                case "Delete Goal":
                    GoalDeleteHandler(isGoalsEmpty);
                    break;
                case "Back":
                    quitGoalsMenu = true;
                    break;
                default:
                    break;
            }
        }
    }

    internal static void ShowGoals()
    {
        List<CodingGoal> goals = GoalsModel.FetchGoals();
        TableVisualizationEngine.DisplayGoalsTable(goals);
    }

    internal static bool CheckGoalsDatabase()
    {
        List<CodingGoal> goals = GoalsModel.FetchGoals();

        bool databaseEmpty = (goals.Count == 0) ? true : false;

        return databaseEmpty;
    }

    internal static void GoalInsertHandler()
    {
        string language = Views.SelectLanguage();
        Views.ShowMessage("How many hours would you like to set for your practice goal?\n");
        string? hours = UserInput.GetNumber();

        bool goalAdded = GoalsModel.InsertGoal(language, hours);

        if (goalAdded)
        {
            Views.ShowMessage("Session was added.");
            Views.ShowMessage("Press any key to continue.");
            Console.ReadLine();
        }
        else
        {
            Views.ShowError("Something went wrong. Session was not added.");
        }
    }

    internal static void GoalViewHandler(bool isEmpty)
    {
        if (!isEmpty)
        {
            List<CodingGoal> goals = GoalsModel.FetchGoals();

            Helpers.CalculateGoalStatus(goals);
            ShowGoals();

            Views.ShowMessage("Press any key to continue.\n");
            Console.ReadLine();
        }
        else
        {
            Views.ShowMessage("Database is empty. Press any key to return to menu.\n");
            Console.ReadLine();
        }
    }

    internal static void GoalDeleteHandler(bool isGoalsEmpty)
    {
        if (!isGoalsEmpty)
        {
            ShowGoals();
            string? id = null;
            string? input = Views.DeleteGoalMenu();

            switch (input)
            {
                case "Delete Goal":
                    id = UserInput.GetId();
                    break;
                case "Clear Goals":
                    id = "*";
                    break;
                case "Back":
                    id = "cancel";
                    break;
                default:
                    break;
            }

            if (id != "cancel")
            {
                bool goalDeleted = GoalsModel.DeleteGoal(id);

                if (goalDeleted)
                {
                    Views.ShowMessage("Session(s) deleted.");
                    Views.ShowMessage("Press any key to continue.");
                    Console.ReadLine();
                }
                else
                {
                    Views.ShowError("Something went wrong. Goal was not deleted.");
                }
            }
        }
        else
        {
            Views.ShowMessage("Database is empty. Press any key to return to menu.\n");
            Console.ReadLine();
        }
    }

    // Methods for report handling
    internal static void ReportHandler()
    {
        List<CodingSession> sessions = Model.FetchSessions();

        int month = todayDate.Month;
        int year = todayDate.Year;

        // Create calendar
        var calendar = new Spectre.Console.Calendar(year, month);

        string monthStart = Helpers.StartOfMonth(todayDate).ToString("dd/MM/yy");

        // Gets list of sessions in the current month
        List<CodingSession> monthlySessionDates = ReportModel.GetMonthlySessions(monthStart);

        // Adds the dates to the calendar
        foreach (CodingSession session in monthlySessionDates)
        {
            var convertedDateToString = session.Date.ToString();
            var parsedDate = DateTime.Parse(convertedDateToString);

            calendar.AddCalendarEvent(parsedDate);
        }

        // Fun fact: Total time coded
        TimeSpan totalTime = Helpers.SumTimes(sessions);

        int days = totalTime.Days;
        int hours = totalTime.Hours;
        int minutes = totalTime.Minutes;

        // Create Breakdown Chart
        List<BreakdownItem> breakdownData = CreateBreakdownList(totalTime);
        var chart = Views.ConfigureBreakdown(breakdownData);

        // Displays everything to widget
        Views.DisplayReportLayout(chart, days, hours, minutes, calendar);
        Console.ReadLine();
    }

    internal static List<BreakdownItem> CreateBreakdownList(TimeSpan totalTime)
    {
        List<BreakdownItem> breakdown = new();

        List<CodingSession> languages = ReportModel.GetLanguagesUsed();

        foreach (CodingSession language in languages)
        {
            List<CodingSession> filteredList = ReportModel.GetFilteredLanguageSessions(language);

            TimeSpan languageCodingTime = Helpers.SumTimes(filteredList);
            double percentage = Math.Round(Helpers.CalculatePercentage(languageCodingTime, totalTime));

            Color languageColor = Helpers.SelectLanguageColor(language);

            breakdown.Add(new BreakdownItem(language.Language, percentage, languageColor));
        }

        return breakdown;
    }

    // Stopwatch methods
    internal static void TimedSessionHandler()
    {
        string? userInput = Views.StartNewSessionMenu();

        if (userInput == "Start Timer")
        {
            string? date = DateTime.Now.Date.ToString("yyyy/MM/dd");
            string? startTime = DateTime.Now.ToString("h\\:mm\\:ss tt");

            Thread stopwatchThread = new Thread(Stopwatch);
            Views.TimerStarted(startTime);

            timerRunning = true;
            stopwatchThread.Start();

            // Stops timer
            Console.ReadLine();

            timerRunning = false;
            stopwatchThread.Join();

            Views.ShowMessage("Timer stopped.");

            string endTime = DateTime.Now.ToString("h\\:mm\\:ss tt");
            Views.TimerStopped(endTime);

            string language = Views.SelectLanguage();
            string duration = Helpers.CalculateDuration(startTime, endTime);

            Model.InsertSession(date, language, startTime, endTime, duration);
        }
        else if (userInput == "Cancel")
        {
            Run();
        }
    }

    internal static void Stopwatch()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine();

        while (timerRunning)
        {
            TimeSpan duration = stopwatch.Elapsed;
            string printedDuration = duration.ToString("hh\\:mm\\:ss");

            Views.PrintStopwatchDuration($"\r{printedDuration}");
            Thread.Sleep(1000);
        }
    }
}
