namespace CodingTracker.barakisbrown;

using Serilog;
using System;

public class Menu
{
    private readonly int[] _menuOptions = new int[] { 0, 1, 2, 3, 4 };
    private readonly string _menuInputString = "Please Select a menu option or 0 to exit?";
    private readonly CodingController ?_controller;
    private readonly CodingSession _session;

    public Menu(CodingController ?controller, CodingSession ?session)
    {
        if (controller is not null)
        {
            _controller = controller;
        }
        if (session is not null)
        {
            _session = session;
        }
        MainLoop();
    }

    private void MainLoop()
    {
        int option = -1;

        while (option != 0)
        {
            Console.WriteLine("Welcome to Coding Session. This will be tracking your coding session.");
            GetMenu();
            option = GetMenuSelection();

            switch (option)
            {
                case 0:
                    break;
                case 1:
                    AddSession();
                    break;
                case 2:
                    DeleteSession();
                    break;
                case 3:
                    UpdateSession();
                    break;
                case 4:
                    ShowAllSessions();
                    break;
            }
        }
        Console.WriteLine("Thank you for using Coding Tracker. Have a good day.");
    }

    private void GetMenu()
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

    private int GetMenuSelection()
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
    
    private void GetKeyReturnMenu()
    {
        Console.ReadKey(true);
        Thread.Sleep(800);
        Console.Clear();
    }

    private void AddSession()
    {
        Console.Clear();
        Console.WriteLine("Adding Session.\n");

        GetKeyReturnMenu();
    }

    private void UpdateSession()
    {
        Console.Clear();
        Console.WriteLine("Update a Session");

        GetKeyReturnMenu();
    }

    private void DeleteSession()
    {
        Console.Clear();
        Console.WriteLine("Delete a Session");

        GetKeyReturnMenu();
    }

    private void ShowAllSessions()
    {
        Console.Clear();
        Console.WriteLine("Show all Sessions");

        GetKeyReturnMenu();
    }
}
