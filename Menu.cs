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
                MenuManager.NewMenu(new ManualSessionMenu(MenuManager, _database));
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
            case 4:
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

        switch (OptionsPicker.MenuIndex)
        {
            case 0: //Update
                UserInterface.UpdateMiniMenu();
                MenuManager.NewMenu(new UpdateMenu(MenuManager, _database));
                break;
        }
    }
}

public class UpdateMenu : Menu
{
    public UpdateMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }
    public override void Display()
    {
        var codingSession = UserInput.IdInput(MenuManager,_database);

        UserInterface.UpdateMenu(codingSession);
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
public class ManualSessionMenu : Menu
{
    public ManualSessionMenu(MenuManager menuManager, Database database) : base(menuManager, database) { }

    public override void Display()
    {
        bool menuContinue = true;
        string sessionNote;

        while (menuContinue)
        {
            UserInterface.SetSessionTime(true);
            string startTimeInput = UserInput.TimeInput(MenuManager);

            UserInterface.SetSessionTime(false);
            string endTimeInput = UserInput.TimeInput(MenuManager);

            UserInterface.SetSessionDate(true);
            string startDateInput = UserInput.DateInput(MenuManager, true);

            UserInterface.SetSessionDate(false);
            string endDateInput = UserInput.DateInput(MenuManager, false);
            if (endDateInput == "_sameAsStart_") endDateInput = startDateInput;

            DateTime startDateTime = LogicOperations.ConstructDateTime(startTimeInput, startDateInput);
            DateTime endDateTime = LogicOperations.ConstructDateTime(endTimeInput, endDateInput);
            TimeSpan duration = LogicOperations.CalculateDuration(endDateTime, startDateTime);

            if (duration < TimeSpan.Zero)
            {
                UserInput.DisplayMessage("End date time must more recent than Start date time.", "retry");
                continue;
            }

            UserInterface.SessionConfirm(startDateTime, endDateTime, duration);

            switch (OptionsPicker.MenuIndex)
            {
                case 0:
                    menuContinue = false;

                    UserInterface.SessionNote();
                    sessionNote = UserInput.InputWithSpecialKeys(MenuManager, false);

                    _database.Insert(sessionNote, startDateTime.ToString("yyyy-MM-dd HH:mm:ss"), endDateTime.ToString("yyyy-MM-dd HH:mm:ss"), $"{duration:hh\\:mm\\:ss}");

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

                    _database.Insert(sessionNote, startDateTime.ToString("yyyy-MM-dd HH:mm:ss"), endDateTime.ToString("yyyy-MM-dd HH:mm:ss"), $"{duration:hh\\:mm\\:ss}");

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
