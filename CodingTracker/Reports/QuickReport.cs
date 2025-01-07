using Spectre.Console;

internal class QuickReport
{
    private static Dictionary<string, Action> _quickReportMenu = new()
    {
        { "Back", Console.Clear },
        { "This week", ThisWeek },
        { "Last week", LastWeek },
        { "This month", ThisMonth },
        { "Last month", LastMonth },
        { "This year", ThisYear },
        { "Last year", LastYear }
    };

    internal static void ShowMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose period: ")
            .PageSize(10)
            .AddChoices(_quickReportMenu.Keys));
        _quickReportMenu[choice]();
    }

    private static void ThisWeek()
    {
        var lastWeekStart = DateTime.Today - TimeSpan.FromDays(((int)DateTime.Today.DayOfWeek + 6) % 7);
        var lastWeekEnd = DateTime.Today;
        if (!RecordReport.ShowTotalAverageInfo(lastWeekStart, lastWeekEnd)) 
        {
            Console.Clear();
            return;
        }
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static void LastWeek()
    {
        var lastWeekStart = DateTime.Today - TimeSpan.FromDays(((int)DateTime.Today.DayOfWeek + 6) % 7 + 7);
        var lastWeekEnd = lastWeekStart.AddDays(6);
        if (!RecordReport.ShowTotalAverageInfo(lastWeekStart, lastWeekEnd))
        {
            Console.Clear();
            return;
        }
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static void ThisMonth()
    {
        var thisMonthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var thisMonthEnd = DateTime.Today;
        if (!RecordReport.ShowTotalAverageInfo(thisMonthStart, thisMonthEnd))
        {
            Console.Clear();
            return;
        }
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static void LastMonth()
    {
        var lastMonthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
        var lastMonthEnd = lastMonthStart.AddMonths(1).AddDays(-1);
        if (!RecordReport.ShowTotalAverageInfo(lastMonthStart, lastMonthEnd))
        {
            Console.Clear();
            return;
        }
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static void ThisYear()
    {
        var thisYearStart = new DateTime(DateTime.Today.Year, 1, 1);
        var thisYearEnd = DateTime.Today;
        if (!RecordReport.ShowTotalAverageInfo(thisYearStart, thisYearEnd))
        {
            Console.Clear();
            return;
        }
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    private static void LastYear()
    {
        var lastYearStart = new DateTime(DateTime.Today.Year, 1, 1).AddYears(-1);
        var lastYearEnd = lastYearStart.AddYears(1).AddDays(-1);
        var (isValid, minDate) = ReportCrud.GetMinDateFromDb();
        if (!isValid) return;

        if (lastYearStart <  minDate) lastYearStart = minDate;

        if (!RecordReport.ShowTotalAverageInfo(lastYearStart, lastYearEnd))
        {
            Console.Clear();
            return;
        }
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }
}
