namespace CodingTracker.barakisbrown;

using Serilog;
using System;

public class Menu
{
    private readonly int[] _menuOptions = new int[] { 0, 1, 2, 3, 4 };
    private readonly string _menuInputString = "Please Select a menu option or 0 to exit?";
    private readonly CodingController ?_controller;

    public Menu(CodingController ?controller)
    {
        if (controller is not null)
        {
            _controller = controller;
        }
    }

    public void GetMenu()
    {
        string menu = @"

            MAIN MENU

            What would you like to do?

            Type 0 to Close Application
            Type 1 to Add Coding Session
            Type 2 to Delete Coding Session
            Type 3 to Update Coding Session
            Type 4 to Show Completed Coding Session
            ---------------------------------------
        ";

        Console.WriteLine(menu);
    }

    public int GetMenuSelection()
    {
        Console.WriteLine(_menuInputString);
        int option;

        while(true)
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            try
            {
                option = int.Parse(input.KeyChar.ToString());
                return option;
            }
            catch (FormatException _)
            {
                Log.Error("F>GetMenuSelection() has fired an exception and was caught. {0}", _.Message);
                Console.WriteLine($"Input needs to be betwen {_menuOptions}");
                Console.WriteLine(_menuInputString);
            }
        }
    }
    
    public void GetKeyReturnMenu()
    {
        Console.ReadKey(true);
        Thread.Sleep(800);
        Console.Clear();
    }
}
