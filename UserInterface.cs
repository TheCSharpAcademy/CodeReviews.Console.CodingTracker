using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker;

public static class UserInterface
{
    public static void MainMenu()
    {
        string[] menuOptions = { "New Coding Session", "Show Records", "Goals", "Exit" };

        Header("coding tracker");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }

    public static void CodingSessionMenu()
    {
        string[] menuOptions = { "Enter Coding Session manually", "Start a new Coding Session", "Go back" };

        Header("new coding session");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }

    public static void RecordsMenu()
    {
        string[] menuOptions = { "Show all", "Show filters", "Update a record", "Delete a record", "Go back" };

        Header("show records");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }
    public static void GoalsMenu()
    {
        string[] menuOptions = { "Set a Goal", "Show Goals", "Go back" };

        Header("goals");

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
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
    public static void SessionConfirm(string startDateTime, string endDateTime)
    {
        Header("new coding session");// dodělat okno pro potvrzení zadaného času, pak udělat i samotnou mechaniku
    }

    private static void Header(string headerText)
    {
        Console.Clear();
        Console.WriteLine($"-----{headerText.ToUpper()}-----");
        Console.WriteLine();
    }
}


