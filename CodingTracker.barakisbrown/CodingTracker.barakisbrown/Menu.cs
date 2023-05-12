namespace CodingTracker.barakisbrown;

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
        string? result = Console.ReadLine();
        int amount;

        while(string.IsNullOrEmpty(result) || !Int32.TryParse(result, out amount) || !_menuOptions.Contains(amount))
        {
            Console.WriteLine("Your answer need to be an acceptable result");
            Console.WriteLine(_menuInputString);
            result = Console.ReadLine();
        }
        return amount;
    }   
}
