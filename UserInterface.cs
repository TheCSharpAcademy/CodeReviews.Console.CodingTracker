using System;
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
        string[] menuOptions = { "Set a Goal", "Show Goals", "Go back"};

        Console.Clear();
        Console.WriteLine("-----GOALS-----");
        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }

    public static void ManualSessionStartTime()
    {
        Console.Clear();
        Console.WriteLine("-----SHOW RECORDS-----");
        Console.WriteLine();

        Console.SetCursorPosition(2,Console.GetCursorPosition().Top);
        Console.WriteLine("Enter the Start time of your session (HH:mm):");
    }
     public static void ManualSessionDate()
    {
        Console.Clear();
        Console.WriteLine("-----SHOW RECORDS-----");
        Console.WriteLine();

        Console.SetCursorPosition(2,Console.GetCursorPosition().Top);
        Console.WriteLine("Start time of your session (HH:mm)(Escape to go back):");
    }
}


