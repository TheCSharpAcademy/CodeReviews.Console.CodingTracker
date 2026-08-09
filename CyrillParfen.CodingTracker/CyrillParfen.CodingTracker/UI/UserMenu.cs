using CyrillParfen.CodingTracker.Data;
using CyrillParfen.CodingTracker.Helpers;
using Spectre.Console;

namespace CyrillParfen.CodingTracker.UI;

internal class UserMenu
{
    private readonly DataAccess _dataAccess;

    public UserMenu(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    internal void RunUserMenu()
    {
        bool isAppRunning = true;

        while (isAppRunning)
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                    .Title("[purple]=== CODING TRACKER - MAIN MENU ===[/]")
                    .AddChoices(Enum.GetValues<MenuOption>())
                    .UseConverter(option => option switch
                    {
                        MenuOption.ViewAllRecords => "View All Records",
                        MenuOption.InsertRecords => "Add Coding Session",
                        MenuOption.DeleteRecords => "Delete Coding Session",
                        MenuOption.UpdateRecords => "Update Coding Session",
                        MenuOption.StartStopwatch => "Start Coding Session (Stopwatch)",
                        MenuOption.ViewSortedRecords => "Find Coding Sessions by Date",
                        _ => option.ToString()
                    }));

            switch (choice)
            {
                case MenuOption.Exit:
                    Console.WriteLine("See you!");
                    isAppRunning = false;
                    break;
                case MenuOption.ViewAllRecords:
                    var records = _dataAccess.GetAllRecord();
                    UserPrompts.PrintRecords(records);
                    break;
                case MenuOption.InsertRecords:
                    _dataAccess.AddCodingSession();
                    break;
                case MenuOption.DeleteRecords:
                    _dataAccess.DeleteCodingSession();
                    break;
                case MenuOption.UpdateRecords:
                    _dataAccess.UpdateCodingSession();
                    break;
                case MenuOption.StartStopwatch:
                    _dataAccess.InitiateStopwatchSession();
                    break;
                case MenuOption.ViewSortedRecords:
                    ShowFilterMenu();
                    break;
            }
        }
    }

    private void ShowFilterMenu()
    {
        AnsiConsole.Clear();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<FilterMenu>()
                .Title("Filter by:")
                .AddChoices(Enum.GetValues<FilterMenu>())
                .UseConverter(option => option switch
                {
                    FilterMenu.LastDay => "Last 24 hours",
                    FilterMenu.LastWeek => "Last 7 days",
                    FilterMenu.LastMonth => "Last 30 days",
                    FilterMenu.LastYear => "Last year",
                    FilterMenu.RangeDate => "Custom range",
                    FilterMenu.BackToMainMenu => "Back to Main Menu",
                    _ => option.ToString()
                }));

        if (choice == FilterMenu.BackToMainMenu) return;

        var isAscending = AnsiConsole.Prompt(
            new SelectionPrompt<bool>()
            .Title("Filter by:")
            .AddChoices(true, false)
            .UseConverter(sortChoice => sortChoice
                ? "Older first"
                : "Latest first"
             ));


        DateTime now = DateTime.Now;
        DateTime? from = null;
        DateTime? to = null;

        switch (choice)
        {
            case FilterMenu.LastDay:
                from = now.AddDays(-1);
                to = now;
                var daySessions = _dataAccess.GetFilteredSelection(from, to, isAscending);
                UserPrompts.PrintRecords(daySessions);
                break;
            case FilterMenu.LastWeek:
                from = now.AddDays(-7);
                to = now;
                var weekSessions = _dataAccess.GetFilteredSelection(from, to, isAscending);
                UserPrompts.PrintRecords(weekSessions);
                break;
            case FilterMenu.LastMonth:
                from = now.AddMonths(-1);
                to = now;
                var monthSessions = _dataAccess.GetFilteredSelection(from, to, isAscending);
                UserPrompts.PrintRecords(monthSessions);
                break;
            case FilterMenu.LastYear:
                from = now.AddYears(-1);
                to = now;
                var yearSessions = _dataAccess.GetFilteredSelection(from, to, isAscending);
                UserPrompts.PrintRecords(yearSessions);
                break;
            case FilterMenu.RangeDate:
                from = DateHelper.GetValidDate("Enter start of the range:");
                to = DateHelper.GetValidDate("Enter end of the range:");
                var customRangeSessions = _dataAccess.GetFilteredSelection(from, to, isAscending);
                UserPrompts.PrintRecords(customRangeSessions);
                break;
        }
    }

    internal enum MenuOption
    {
        ViewAllRecords,
        InsertRecords,
        DeleteRecords,
        UpdateRecords,
        StartStopwatch,
        ViewSortedRecords,
        Exit,
    }

    internal enum FilterMenu
    {
        LastDay,
        LastWeek,
        LastMonth,
        LastYear,
        RangeDate,
        BackToMainMenu,
    }
}
