using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker;

public static class UserInterface
{
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

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void RecordsMenu()
    {
        string[] menuOptions = { "Show all", "Show filters", "Update a record", "Delete a record", "Go back" };

        Header("show records");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void RecordsAllMenu(List<CodingSession> codingSessionList)
    {
        Header("show all records");

        var table = new Table();
        table.Border(TableBorder.Heavy);

        table.AddColumns("ID", "[green]START[/]", "[red]END[/]", "[yellow]DURATION[/]", "NOTE");
        
        foreach (var codingSession in codingSessionList)
        {
            table.AddRow(
                codingSession.Id.ToString(),
                codingSession.StartTime.ToString("HH:mm:ss MM/dd/yyyy"),
                codingSession.EndTime.ToString("HH:mm:ss MM/dd/yyyy"),
                codingSession.Duration.ToString(),
                codingSession.Note);
        }
        AnsiConsole.Write(table);
        
        UserInput.DisplayMessage();


    }

    public static void GoalsMenu()
    {
        string[] menuOptions = { "Set a Goal", "Show Goals", "Go back" };

        Header("goals");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }

    public static void ManualSessionTime(bool isStart)
    {

        string sessionTimeLabel = isStart ? "Start" : "End";

        Header("new coding session");

        Console.SetCursorPosition(2, Console.GetCursorPosition().Top);
        Console.WriteLine($"{sessionTimeLabel} time of your session (HH:mm):");
    }
    public static void ManualSessionDate(bool isStart)
    {
        string sessionDateLabel = isStart ? "Start" : "End";
        string autoDateEnter = isStart ? "Enter, if it's today." : "Enter, if it's the same as start date.";
        int boxWidthModifier = 5;

        Header("new coding session");

        Console.SetCursorPosition(2, Console.GetCursorPosition().Top);
        Console.WriteLine($"{sessionDateLabel} date of your session (YYYY-MM-DD)(Escape to go back):");

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(string.Format("  {0,-" + (autoDateEnter.Length + boxWidthModifier) + "}", autoDateEnter));
        Console.ResetColor();

    }
    public static void SessionConfirm(DateTime startDateTime, DateTime endDateTime, TimeSpan duration)
    {
        string[] menuOptions = { "Confirm", "Enter again", "Go back" };

        Header("new coding session");
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
            Console.WriteLine($"Duration:\t{duration:hh\\:mm}");
        else
            Console.WriteLine($"Duration:\t{duration.Days} days, {duration:hh\\:mm}");

        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top, true);
    }


    public static void SessionNote()
    {
        Header("new coding session");
        Console.WriteLine("Enter a note (Press Enter to leave blank)): ");
    }

    public static void ManualSessionMenu()
    {
        string[] menuOptions = { "START", "Go back" };

        Header("new coding session");

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
    }
}


