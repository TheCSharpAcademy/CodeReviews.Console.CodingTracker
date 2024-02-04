using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker;

public static class UserInterface
{
    private static int _currentCursorTopPosition;
    public static void MainMenu()
    {
        string[] menuOptions = { "New Coding Session", "Show Records", "Goals", "Exit" };

        Header("coding tracker");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void CodingSessionMenu()
    {
        string[] menuOptions = { "Enter Coding Session manually", "Start a new Coding Session", "Go back" };

        Header("new coding session");
        LockCursorPosition();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void RecordsMenu()
    {
        string[] menuOptions = { "Show all", "Filters", "Go back" };

        Header("show sessions");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void FilterSessionsMenu()
    {
        string[] menuOptions = { "By week", "By month", "By year", "Go back" };
        Header("filters");
        Console.WriteLine("Choose a filter:");

        OptionsPicker.Navigate(menuOptions,Console.GetCursorPosition().Top,true);
    }

    public static void FilterWeeksMenu(string[] yearsList)
    {
        string[] yearsListOptions = new string[yearsList.Length+1];

        yearsList.CopyTo(yearsListOptions,0);
        yearsListOptions[yearsListOptions.Length-1] = "Go back";
        
        Header("show sessions by week");

        Console.WriteLine("Choose a year:");

        OptionsPicker.Navigate(yearsListOptions,Console.GetCursorPosition().Top,true);
    }

    public static void RecordsAllMenu(List<CodingSession> codingSessionList, TimeSpan averageDuration, TimeSpan totalDuration)
    {
        string[] menuOptions = { "Update", "Delete", "Go back" };

        Header("show all sessions");

        DisplayTable(codingSessionList);

        Console.WriteLine($"Average duration: {averageDuration:hh\\:mm\\:ss}");
        Console.WriteLine($"Total duration: {totalDuration}");
        Console.WriteLine();
        LockCursorPosition();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }
    public static void UpdateMiniMenu()
    {
        ConsoleClearLines(Console.GetCursorPosition().Top);
        Console.WriteLine("Type ID of a session you want to update: ");
    }
    public static void DeleteMiniMenu()
    {
        ConsoleClearLines(GetLockedCursorPosition());
        Console.WriteLine("Type ID of a session you want to delete: ");
    }
    public static void UpdateMenu(List<CodingSession> codingSessions)
    {

        Header($"update a record of id {codingSessions[0].Id}");
        DisplayTable(codingSessions);
        LockCursorPosition();
    }
    public static void DeleteMenu(List<CodingSession> codingSessions)
    {
        string[] menuOptions = { "No", "Yes" };

        Header($"delete a session of id {codingSessions[0].Id}");
        DisplayTable(codingSessions);
        LockCursorPosition();
        Console.WriteLine("\n Do you really want to delete this session?");
        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, false);
    }

    public static void GoalsMenu()
    {
        string[] menuOptions = { "Set a Goal", "Show Goals", "Go back" };

        Header("goals");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void SetSessionTime(bool isStart, bool isUpdate = false)
    {

        string sessionTimeLabel = isStart ? "Start" : "End";

        ConsoleClearLines(GetLockedCursorPosition());

        Console.WriteLine($"{sessionTimeLabel} time of your session (HH:mm):");

        if (isUpdate)
        {
            string autoTimeEnter = "Enter, if it's the same as original.";
            int boxWidthModifier = 5;

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(string.Format("  {0,-" + (autoTimeEnter.Length + boxWidthModifier) + "}", autoTimeEnter));
            Console.ResetColor();
        }

    }
    public static void SetSessionDate(bool isStart, bool isUpdate = false)
    {
        string autoDateEnter;
        string sessionDateLabel = isStart ? "Start" : "End";
        if (!isUpdate)
        {
            autoDateEnter = isStart ? "Enter, if it's today." : "Enter, if it's the same as start date.";
        }
        else
        {
            autoDateEnter = "Enter, if it's the same as original.";
        }

        int boxWidthModifier = 5;

        ConsoleClearLines(GetLockedCursorPosition());

        Console.WriteLine($"{sessionDateLabel} date of your session (YYYY-MM-DD)(Escape to go back):");

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(string.Format("  {0,-" + (autoDateEnter.Length + boxWidthModifier) + "}", autoDateEnter));
        Console.ResetColor();

    }
    public static void SessionConfirm(DateTime startDateTime, DateTime endDateTime, TimeSpan duration)
    {
        string[] menuOptions = { "Confirm", "Enter again", "Go back" };

        ConsoleClearLines(GetLockedCursorPosition());

        Console.WriteLine($"Start time:\t{startDateTime.ToString("HH:mm")}\t{startDateTime.ToString("yyyy-MM-dd")}");
        Console.WriteLine($"End time:\t{endDateTime.ToString("HH:mm")}\t{endDateTime.ToString("yyyy-MM-dd")}\n");

        if (duration.Days == 0)
            Console.WriteLine($"Duration:\t{duration:hh\\:mm}");
        else
            Console.WriteLine($"Duration:\t{duration.Days} days, {duration:hh\\:mm}");

        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void AutoSessionConfirm(DateTime startDateTime, DateTime endDateTime, TimeSpan duration, TimeSpan totalBreaks)
    {
        string[] menuOptions = { "Confirm", "Start over", "Discard and go back" };

        Header("new coding session");
        Console.WriteLine($"Start time:\t{startDateTime.ToString("HH:mm")}\t{startDateTime.ToString("yyyy-MM-dd")}");
        Console.WriteLine($"End time:\t{endDateTime.ToString("HH:mm")}\t{endDateTime.ToString("yyyy-MM-dd")}\n");

        if (totalBreaks.Seconds > 10) //shows breaks only if 10 or more seconds
            Console.WriteLine($"Total breaks:\t{totalBreaks:hh\\:mm\\:ss}");

        if (duration.Days == 0)
            Console.WriteLine($"Duration:\t{duration:hh\\:mm\\:ss}");
        else
            Console.WriteLine($"Duration:\t{duration.Days} days, {duration:hh\\:mm\\:ss}");

        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }


    public static void SessionNote(bool isUpdate = false)
    {
        string enterMessage = isUpdate ? "as original" : "blank";
        ConsoleClearLines(GetLockedCursorPosition());
        Console.WriteLine($"Enter a note (Press Enter to leave {enterMessage})): ");
    }

    public static void ManualSessionMenu()
    {
        string[] menuOptions = { "START", "Go back" };

        Header("new coding session");
        LockCursorPosition();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void SessionInProgress()
    {
        Header("session in progress");
    }


    private static void Header(string headerText)
    {
        Console.Clear();
        Console.WriteLine($"-----{headerText.ToUpper()}-----");
        Console.WriteLine();
    }
    public static void ConsoleClearLines(int clearFromLine)
    {
        for (int i = Console.WindowHeight; i >= clearFromLine; i--)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
    }
    public static void ConsoleClearLastLines(int lastLines)
    {
        int initialCursorTopPosition = Console.GetCursorPosition().Top - lastLines;
        Console.SetCursorPosition(0, initialCursorTopPosition);

        for (int i = Console.WindowHeight; i >= initialCursorTopPosition; i--)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
    }

    private static void DisplayTable(List<CodingSession> list)
    {
        var table = new Table();
        table.Border(TableBorder.Heavy);

        table.AddColumns("ID", "[green]START[/]", "[red]END[/]", "[yellow]DURATION[/]", "NOTE");

        foreach (var codingSession in list)
        {
            table.AddRow(
            codingSession.Id.ToString(),
            codingSession.StartTime.ToString("HH:mm:ss MM/dd/yyyy"),
            codingSession.EndTime.ToString("HH:mm:ss MM/dd/yyyy"),
            codingSession.Duration.ToString(),
            codingSession.Note);
        }

        AnsiConsole.Write(table);

    }

    private static void LockCursorPosition()
    {
        _currentCursorTopPosition = Console.GetCursorPosition().Top;
    }
    private static int GetLockedCursorPosition() => _currentCursorTopPosition;

}


