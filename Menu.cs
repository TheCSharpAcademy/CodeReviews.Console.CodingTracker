using System;
using System.Collections.Generic;
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

    protected Menu(MenuManager menuManager)
    {
        MenuManager = menuManager;
    }
    public abstract void Display();
}

public class MainMenu : Menu
{
    public MainMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        UserInterface.MainMenu();
        switch (OptionsPicker.MenuIndex)
        {
            case 0:
                MenuManager.NewMenu(new CodingSessionMenu(MenuManager));
                break;
            case 1:
                MenuManager.NewMenu(new ShowRecordsMenu(MenuManager));
                break;
            case 2:
                MenuManager.NewMenu(new GoalsMenu(MenuManager));
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
    public CodingSessionMenu(MenuManager menuManager) : base(menuManager) { }
    public override void Display()
    {
        UserInterface.CodingSessionMenu();

        switch (OptionsPicker.MenuIndex)
        {
            case 0:
                MenuManager.NewMenu(new ManualSessionMenu(MenuManager));
                break;
            case 1:
                MenuManager.NewMenu(new AutoSessionMenu(MenuManager));
                break;
            case 2:
                MenuManager.GoBack();
                break;
        }
    }
}

public class ShowRecordsMenu : Menu
{
    public ShowRecordsMenu(MenuManager menuManager) : base(menuManager) { }
    public override void Display()
    {
        UserInterface.RecordsMenu();

        switch (OptionsPicker.MenuIndex)
        {
            case 4:
                MenuManager.GoBack();
                break;
        }
    }
}
public class GoalsMenu : Menu
{
    public GoalsMenu(MenuManager menuManager) : base(menuManager) { }
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
    public ManualSessionMenu(MenuManager menuManager) : base(menuManager) { }
    public override void Display()
    {
        bool menuContinue = true;
        string sessionNote;

        while (menuContinue)
        {
            UserInterface.ManualSessionTime(true);
            string startTimeInput = UserInput.TimeInput(MenuManager);

            UserInterface.ManualSessionTime(false);
            string endTimeInput = UserInput.TimeInput(MenuManager);

            UserInterface.ManualSessionDate(true);
            string startDateInput = UserInput.DateInput(MenuManager, true);

            UserInterface.ManualSessionDate(false);
            string endDateInput = UserInput.DateInput(MenuManager, false);
            if (endDateInput == "_sameAsStart_") endDateInput = startDateInput;

            DateTime startDateTime = LogicOperations.ConstructDateTime(startTimeInput, startDateInput);
            DateTime endDateTime = LogicOperations.ConstructDateTime(endTimeInput, endDateInput);
            TimeSpan duration = LogicOperations.CalculateDuration(endDateTime, startDateTime);

            UserInterface.SessionConfirm(startDateTime, endDateTime, duration);

            switch (OptionsPicker.MenuIndex)
            {
                case 0:
                    menuContinue = false;
                    UserInterface.SessionNote();
                    sessionNote = UserInput.InputWithSpecialKeys(MenuManager, false);
                    Console.WriteLine("entry sent to the database"); //ENTRY SENT TO THE DATABASE
                    Console.ReadKey();
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

    public AutoSessionMenu(MenuManager menuManager) : base(menuManager) { }
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
                    Console.WriteLine("entry sent to the database"); //ENTRY SENT TO THE DATABASE
                    Console.ReadKey();
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
