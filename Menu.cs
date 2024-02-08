using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Threading;
using Spectre.Console;

namespace CodingTracker;

public abstract class Menu
{

    protected MenuManager MenuManager { get; }
    protected Database _database;

    protected Menu(MenuManager menuManager, Database database)
    {
        _database = database;
        MenuManager = menuManager;
    }
    public abstract void Display();
}

public class MainMenu : Menu
{
    public MainMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }

    public override void Display()
    {
        UserInterface.MainMenu();
        switch (OptionsPicker.MenuIndex)
        {
            case 0:
                MenuManager.NewMenu(new CodingSessionMenu(MenuManager, _database));
                break;
            case 1:
                MenuManager.NewMenu(new ShowRecordsMenu(MenuManager, _database));
                break;
            case 2:
                MenuManager.NewMenu(new GoalsMenu(MenuManager, _database));
                break;
            default:
                Environment.Exit(0);
                break;
        }
        MenuManager.DisplayCurrentMenu();

    }
}

public class CodingSessionMenu : Menu
{
    public CodingSessionMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }
    public override void Display()
    {
        UserInterface.CodingSessionMenu();

        switch (OptionsPicker.MenuIndex)
        {
            case 0:
                MenuManager.NewMenu(new SetSessionMenu(MenuManager, _database));
                break;
            case 1:
                MenuManager.NewMenu(new AutoSessionMenu(MenuManager, _database));
                break;
            case 2:
                MenuManager.GoBack();
                break;
        }
    }
}

public class ShowRecordsMenu : Menu
{
    public ShowRecordsMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }
    public override void Display()
    {
        UserInterface.RecordsMenu();

        switch (OptionsPicker.MenuIndex)
        {
            case 0:
                MenuManager.NewMenu(new ShowAllRecordsMenu(MenuManager, _database));
                break;
            case 1:
                MenuManager.NewMenu(new ShowFiltersMenu(MenuManager, _database));
                break;
            case 2:
                MenuManager.GoBack();
                break;
        }
    }
}
public class ShowAllRecordsMenu : Menu
{
    public ShowAllRecordsMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }
    public override void Display()
    {
        var codingSessionList = _database.GetAll();
        var averageDuration = LogicOperations.AverageDuration(codingSessionList);
        var totalDuration = LogicOperations.TotalDuration(codingSessionList);

        UserInterface.RecordsAllMenu(codingSessionList, averageDuration, totalDuration);

        while (true)
        {
            switch (OptionsPicker.MenuIndex)
            {
                case 0:
                    codingSessionList = _database.GetAll();
                    UserInterface.RecordsAllMenu(codingSessionList, averageDuration, totalDuration);
                    continue;
                case 1:
                    codingSessionList = _database.GetAll(false);
                    UserInterface.RecordsAllMenu(codingSessionList, averageDuration, totalDuration);
                    continue;
                case 2: //Update
                    UserInterface.UpdateMiniMenu();
                    MenuManager.NewMenu(new UpdateMenu(MenuManager, _database, codingSessionList));
                    break;
                case 3: //Delete
                    UserInterface.DeleteMiniMenu();
                    MenuManager.NewMenu(new DeleteMenu(MenuManager, _database, codingSessionList));
                    break;
                case 4: //GoBack
                    MenuManager.GoBack();
                    break;
            }
        }
    }
}

public class ShowFiltersMenu : Menu
{
    public ShowFiltersMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }

    public override void Display()
    {
        UserInterface.FilterSessionsMenu();
        switch (OptionsPicker.MenuIndex)
        {
            case 0:
                MenuManager.NewMenu(new WeeksMenu(MenuManager, _database));
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                MenuManager.GoBack();
                break;
        }
    }
}
public class WeeksMenu : Menu
{
    public WeeksMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }

    public override void Display()
    {
        string userYear = GetUserYear();
        string userMonth = GetUserMonth(userYear);
        string userWeek = GetUserWeek(userYear, userMonth);

        List<CodingSession> codingSessionList = _database.GetByWeeks(userYear, userMonth, userWeek);

        var averageDuration = LogicOperations.AverageDuration(codingSessionList);
        var totalDuration = LogicOperations.TotalDuration(codingSessionList);

        UserInterface.FilterByWeeksMenu(codingSessionList, userWeek, userYear, averageDuration, totalDuration);

        while (true)
        {
            switch (OptionsPicker.MenuIndex)
            {
                case 0: //asc
                    codingSessionList = _database.GetByWeeks(userYear,userMonth,userWeek);
                    UserInterface.FilterByWeeksMenu(codingSessionList,userWeek, userYear, averageDuration, totalDuration);
                    continue;
                case 1: //desc
                    codingSessionList = _database.GetByWeeks(userYear,userMonth,userWeek,false);
                    UserInterface.FilterByWeeksMenu(codingSessionList,userWeek, userYear, averageDuration, totalDuration);
                    continue;
                case 2: //Update
                    UserInterface.UpdateMiniMenu();
                    MenuManager.NewMenu(new UpdateMenu(MenuManager, _database, codingSessionList));
                    break;
                case 3: //Delete
                    UserInterface.DeleteMiniMenu();
                    MenuManager.NewMenu(new DeleteMenu(MenuManager, _database, codingSessionList));
                    break;
                case 4: //GoBack
                    MenuManager.GoBack();
                    break;
            }
        }
    }
    private string GetUserYear()
    {
        var yearArray = _database.GetDistinctYears();
        UserInterface.PickYearMiniMenu(yearArray);

        if (OptionsPicker.MenuIndex == yearArray.Length)
            MenuManager.GoBack();

        return yearArray[OptionsPicker.MenuIndex];
    }
    private string GetUserMonth(string year)
    {
        var monthArray = _database.GetDistinctMonths(year);
        string[] monthNameArray = LogicOperations.MonthsToNamesArray(monthArray);
        UserInterface.PickMonthMiniMenu(monthNameArray);

        if (OptionsPicker.MenuIndex == monthArray.Length)
            MenuManager.GoBack();
        return monthArray[OptionsPicker.MenuIndex];
    }
    private string GetUserWeek(string year, string month)
    {
        var weekArray = _database.GetDistinctWeeks(year, month);
        UserInterface.PickWeekMiniMenu(weekArray);

        if (OptionsPicker.MenuIndex == weekArray.Length)
            MenuManager.GoBack();
        return weekArray[OptionsPicker.MenuIndex];
    }
}
public class UpdateMenu : SetSessionMenu
{
    private List<CodingSession> _codingSessionList;

    public UpdateMenu(MenuManager menuManager, Database database, List<CodingSession> codingSessionList) : base(menuManager, database) { _codingSessionList = codingSessionList; }
    public override void Display()
    {
        var codingSession = UserInput.IdInput(MenuManager, _database, _codingSessionList);
        UserInterface.UpdateMenu(codingSession);

        bool menuContinue = true;
        string sessionNote;

        while (menuContinue)
        {
            UpdateDateTime(codingSession);

            TimeSpan duration = LogicOperations.CalculateDuration(_endDateTime, _startDateTime);

            if (duration < TimeSpan.Zero)
            {
                UserInput.DisplayMessage("End date time must more recent than Start date time.", "retry");
                continue;
            }

            UserInterface.SessionConfirm(_startDateTime, _endDateTime, duration);

            switch (OptionsPicker.MenuIndex)
            {
                case 0:
                    menuContinue = false;

                    UserInterface.SessionNote(true);
                    sessionNote = UserInput.InputWithSpecialKeys(MenuManager, false);

                    int yearNumber = Convert.ToInt32(_startDateTime.ToString("yyyy"));
                    int monthNumber = Convert.ToInt32(_startDateTime.ToString("MM"));
                    int weekNumber = LogicOperations.GetWeekNumber(_startDateTime);

                    _database.Update(codingSession.Id, sessionNote, _startDateTime.ToString("yyyy-MM-dd HH:mm:ss"), _endDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    $"{duration:hh\\:mm\\:ss}", yearNumber, monthNumber, weekNumber);

                    UserInput.DisplayMessage("Session updated!", "return to Main Menu");
                    MenuManager.ReturnToMainMenu();
                    break;
                case 1:
                    menuContinue = true;
                    break;
                case 2:
                    menuContinue = false;
                    MenuManager.GoBack();
                    break;
            }
        }
    }
    public void UpdateDateTime(CodingSession codingSession)
    {
        bool update = true;

        UserInterface.SetSessionTime(true, update);
        _startTimeInput = UserInput.TimeInput(MenuManager, true);
        if (_startTimeInput == "_noInput_") _startTimeInput = codingSession.StartTime.ToString("HH:mm");


        UserInterface.SetSessionTime(false, update);
        _endTimeInput = UserInput.TimeInput(MenuManager, true);
        if (_endTimeInput == "_noInput_") _endTimeInput = codingSession.EndTime.ToString("HH:mm");


        UserInterface.SetSessionDate(true, update);
        _startDateInput = UserInput.DateInput(MenuManager, true);
        if (_startDateInput == "_noInput_") _startDateInput = codingSession.StartTime.ToString("yyyy-MM-dd");


        UserInterface.SetSessionDate(false, update);
        _endDateInput = UserInput.DateInput(MenuManager, true);
        if (_endDateInput == "_noInput_") _endDateInput = codingSession.EndTime.ToString("yyyy-MM-dd");

        _startDateTime = LogicOperations.ConstructDateTime(_startTimeInput, _startDateInput);
        _endDateTime = LogicOperations.ConstructDateTime(_endTimeInput, _endDateInput);
    }
}
public class DeleteMenu : Menu
{
    private List<CodingSession> _codingSessionList;
    public DeleteMenu(MenuManager menuManager, Database database, List<CodingSession> codingSessionList) : base(menuManager, database) { _codingSessionList = codingSessionList; }
    public override void Display()
    {
        var codingSession = UserInput.IdInput(MenuManager, _database, _codingSessionList);
        UserInterface.DeleteMenu(codingSession);
        if (OptionsPicker.MenuIndex == 1)
        {
            _database.Delete(codingSession.Id);
            UserInput.DisplayMessage("Session deleted!", "return to Main Menu");
            MenuManager.ReturnToMainMenu();
        }
        else
        {
            MenuManager.GoBack();
        }
    }
}

public class GoalsMenu : Menu
{
    public GoalsMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }
    public override void Display()
    {
        UserInterface.GoalsMenu();

        switch (OptionsPicker.MenuIndex)
        {
            case 2:
                MenuManager.GoBack();
                break;
        }
    }
}
public class SetSessionMenu : Menu
{
    protected string _startTimeInput = "";
    protected string _endTimeInput = "";
    protected string _startDateInput = "";
    protected string _endDateInput = "";
    protected DateTime _startDateTime;
    protected DateTime _endDateTime;

    public SetSessionMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }

    public override void Display()
    {
        bool menuContinue = true;
        string sessionNote;

        while (menuContinue)
        {
            SetDateTime();

            TimeSpan duration = LogicOperations.CalculateDuration(_endDateTime, _startDateTime);

            if (duration < TimeSpan.Zero)
            {
                UserInput.DisplayMessage("End date time must more recent than Start date time.", "retry");
                continue;
            }

            UserInterface.SessionConfirm(_startDateTime, _endDateTime, duration);

            switch (OptionsPicker.MenuIndex)
            {
                case 0:
                    menuContinue = false;

                    UserInterface.SessionNote();
                    sessionNote = UserInput.InputWithSpecialKeys(MenuManager, false);

                    int yearNumber = Convert.ToInt32(_startDateTime.ToString("yyyy"));
                    int monthNumber = Convert.ToInt32(_startDateTime.ToString("MM"));
                    int weekNumber = LogicOperations.GetWeekNumber(_startDateTime);

                    _database.Insert(sessionNote, _startDateTime.ToString("yyyy-MM-dd HH:mm:ss"), _endDateTime.ToString("yyyy-MM-dd HH:mm:ss"), $"{duration:hh\\:mm\\:ss}", yearNumber, monthNumber, weekNumber);

                    UserInput.DisplayMessage("Session saved!", "return to Main Menu");

                    MenuManager.ReturnToMainMenu();
                    break;
                case 1:
                    menuContinue = true;
                    break;
                case 2:
                    menuContinue = false;
                    MenuManager.GoBack();
                    break;
            }
        }
    }
    public virtual void SetDateTime()
    {
        UserInterface.SetSessionTime(true);
        _startTimeInput = UserInput.TimeInput(MenuManager, false);

        UserInterface.SetSessionTime(false);
        _endTimeInput = UserInput.TimeInput(MenuManager, false);

        UserInterface.SetSessionDate(true);
        _startDateInput = UserInput.DateInput(MenuManager, false);

        UserInterface.SetSessionDate(false);
        _endDateInput = UserInput.DateInput(MenuManager, true);
        if (_endDateInput == "_noInput_") _endDateInput = _startDateInput;

        _startDateTime = LogicOperations.ConstructDateTime(_startTimeInput, _startDateInput);
        _endDateTime = LogicOperations.ConstructDateTime(_endTimeInput, _endDateInput);
    }
}

public class AutoSessionMenu : Menu
{
    private DateTime startDateTime = new();
    private DateTime endDateTime = new();
    private TimeSpan duration = new();

    public AutoSessionMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }
    public override void Display()
    {

        string sessionNote;
        bool menuContinue = true;

        while (menuContinue)
        {
            UserInterface.SessionInProgress();
            HandleStopWatch();

            TimeSpan totalBreaks = LogicOperations.CalculateBreaks(startDateTime, endDateTime, duration);

            UserInterface.AutoSessionConfirm(startDateTime, endDateTime, duration, totalBreaks);

            switch (OptionsPicker.MenuIndex)
            {
                case 0:
                    menuContinue = false;

                    UserInterface.SessionNote();
                    sessionNote = UserInput.InputWithSpecialKeys(MenuManager, false);

                    int yearNumber = Convert.ToInt32(startDateTime.ToString("yyyy"));
                    int monthNumber = Convert.ToInt32(startDateTime.ToString("MM"));
                    int weekNumber = LogicOperations.GetWeekNumber(startDateTime);

                    _database.Insert(sessionNote, startDateTime.ToString("yyyy-MM-dd HH:mm:ss"), endDateTime.ToString("yyyy-MM-dd HH:mm:ss"), $"{duration:hh\\:mm\\:ss}", yearNumber, monthNumber, weekNumber);

                    MenuManager.ReturnToMainMenu();
                    break;
                case 1:
                    menuContinue = true;
                    break;
                case 2:
                    menuContinue = false;
                    MenuManager.GoBack();
                    break;
            }
        }
    }
    private void HandleStopWatch()
    {
        StopwatchTimer stopwatch = new();
        bool sessionContinue = true;
        string[] inProgressOption = { "Pause" };
        string[] pausedOptions = { "Continue", "End" };

        startDateTime = DateTime.Now;
        stopwatch.Start();

        while (sessionContinue)
        {
            Console.SetCursorPosition(0, 4);
            OptionsPicker.Navigate(inProgressOption, Console.GetCursorPosition().Top, false);

            stopwatch.Pause();
            Console.SetCursorPosition(0, 4);
            OptionsPicker.Navigate(pausedOptions, Console.GetCursorPosition().Top, true);

            if (OptionsPicker.MenuIndex == 0)
            {
                UserInterface.ConsoleClearLines(4);
                stopwatch.Resume();
            }
            else
            {
                sessionContinue = false;
                endDateTime = DateTime.Now;
                duration = stopwatch.Duration;
            }
        }
    }

}
