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

        OptionsPicker.Navigate(menuOptions,1);
    }

    public static void CodingSessionMenu()
    {
        string[] menuOptions = { "Enter Coding Session manually", "Start a new Coding Session", "Go back"};

        Console.Clear();
        Console.WriteLine("-----NEW CODING SESSION-----");

        OptionsPicker.Navigate(menuOptions,1);

    }
}


