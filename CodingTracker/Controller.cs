namespace CodingTracker;

public static class Controller
{
    public static DateTime MonthToShow = Utils.GetFirstDayMonth(DateTime.Now);
    private static UserOption.PromptMenus _promptMenu = UserOption.PromptMenus.Main;
    private static bool _endApp = false;
    
    public static async Task Start()
    {
        UserInterface.Title();
        do
        {
            var sessions = await DataBaseInterface.ViewCodingSessions(MonthToShow, Utils.NextMonth(MonthToShow));
            var sessionsPerDay = AmountCodingPerDay(sessions);
            
            TableVisualisationEngine.ShowCalendarWithCaption(sessionsPerDay);
            await Prompt();

        } while (!_endApp);
    }

    private static async Task Prompt()
    {
        switch (_promptMenu)
        {
            case UserOption.PromptMenus.Main:
                var choiceMainMenu = UserInterface.MainMenuPrompt();
                MainMenu(choiceMainMenu);
                break;
            case UserOption.PromptMenus.View:
                var choiceViewMenu = UserInterface.ViewMenuPrompt();
                ViewMenu(choiceViewMenu);
                break;
            case UserOption.PromptMenus.AddLive:
                var (startDateLive, endDateLive) = UserInterface.AskDateRangeLive();
                await AddMenu(startDateLive, endDateLive);
                _promptMenu = UserOption.PromptMenus.Main;
                break;
            case UserOption.PromptMenus.Add:
                var (startDate, endDate) = UserInterface.AskDateRange();
                await AddMenu(startDate, endDate);
                _promptMenu = UserOption.PromptMenus.Main;
                break;
            default:
                throw new ArgumentOutOfRangeException("Not valid promptMenu");
        }
    }

    private static void MainMenu(UserOption.MainMenu choice)
    {
        switch (choice)
        {
            case UserOption.MainMenu.Insert:
                _promptMenu = UserOption.PromptMenus.Add;
                break;
            case UserOption.MainMenu.InsertLive:
                _promptMenu = UserOption.PromptMenus.AddLive;
                break;
            case UserOption.MainMenu.View:
                _promptMenu = UserOption.PromptMenus.View;
                break;
            case UserOption.MainMenu.Quit:
                _endApp = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
        }
    }

    private static void ViewMenu(UserOption.ViewMenu choice)
    {
        switch (choice)
        {
            case UserOption.ViewMenu.Previous:
                MonthToShow = MonthToShow.AddMonths(-1);
                break;
            case UserOption.ViewMenu.Next:
                MonthToShow = MonthToShow.AddMonths(1);
                break;
            case UserOption.ViewMenu.Quit:
                _promptMenu = UserOption.PromptMenus.Main;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
        }
    }

    private static async Task AddMenu(DateTime start, DateTime end)
    {
        var duration = Utils.Duration(start, end);
        var codingSession = new CodingSession(start, end, duration);
        bool confirmation = UserInterface.ConfirmationPrompt($"add {codingSession}");

        if (confirmation) await DataBaseInterface.AddCodingSession(codingSession);
    }
    
    /// Aggregates coding sessions by calendar day, returning one session per day with summed durations
    /// We take this postulate: Sessions that happens past midnight are attributed entirely to their start date
    public static List<CodingSession> AmountCodingPerDay(List<CodingSession> codingSessions)
    {
        return codingSessions
            .GroupBy(codingSession => codingSession.StartTime.Date) // Only take the day of the startTime
            .Select(codingSession =>
            new CodingSession(
                codingSession.Key,
                codingSession.Key.AddDays(1),
                TimeSpan.FromTicks(codingSession.Sum(s => s.Duration.Ticks))))
            .ToList();
    }
    
}