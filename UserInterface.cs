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

        Console.Clear();
        Console.WriteLine("-----CODING TRACKER-----");
        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }

    public static void CodingSessionMenu()
    {
        string[] menuOptions = { "Enter Coding Session manually", "Start a new Coding Session", "Go back" };

        Console.Clear();
        Console.WriteLine("-----NEW CODING SESSION-----");
        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }

    public static void RecordsMenu()
    {
        string[] menuOptions = { "Show all", "Show filters", "Update a record", "Delete a record", "Go back" };

        Console.Clear();
        Console.WriteLine("-----SHOW RECORDS-----");
        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }
    public static void GoalsMenu()
    {
        string[] menuOptions = { "Set a Goal", "Show Goals", "Go back" };

        Console.Clear();
        Console.WriteLine("-----GOALS-----");
        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }

    public static void ManualSessionTime(bool isStart)
    {

        string sessionTimeLabel = isStart ? "Start" : "End";

        Console.Clear();
        Console.WriteLine("-----NEW CODING SESSION-----");
        Console.WriteLine();

        Console.SetCursorPosition(2, Console.GetCursorPosition().Top);
        Console.WriteLine($"{sessionTimeLabel} time of your session (HH:mm):");
    }
    public static void ManualSessionDate(bool isStart)
    {
        string sessionDateLabel = isStart ? "Start" : "End";
        string autoDateEnter = isStart ? "Enter, if it's today." : "Enter, if it's the same as start date.";
        int boxWidthModifier = 5;

        Console.Clear();
        Console.WriteLine("-----NEW CODING SESSION-----");
        Console.WriteLine();

        Console.SetCursorPosition(2, Console.GetCursorPosition().Top);
        Console.WriteLine($"{sessionDateLabel} date of your session (YYYY-MM-DD)(Escape to go back):");

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(string.Format("  {0,-"+ (autoDateEnter.Length + boxWidthModifier) +"}", autoDateEnter));
        Console.ResetColor();

    }


}


