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

    public static void ShowRecordsMenu()
    {
        string[] menuOptions = { "Show all", "Show filters", "Update a record", "Delete a record", "Go back" };

        Console.Clear();
        Console.WriteLine("-----SHOW RECORDS-----");
        Console.WriteLine();

        OptionsPicker.Navigate(menuOptions, Console.GetCursorPosition().Top);
    }
}


