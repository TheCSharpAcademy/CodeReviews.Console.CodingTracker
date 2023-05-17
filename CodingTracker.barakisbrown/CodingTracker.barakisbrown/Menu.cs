namespace CodingTracker.barakisbrown;

using Serilog;
using System;

public class Menu
{
    private readonly int[] _menuOptions = new int[] { 0, 1, 2, 3, 4 };
    private readonly string _menuInputString = "\t    Please Select a menu option or 0 to exit?";
    private readonly CodingController ?_controller;
    private readonly CodingSession ?_session;

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
            Console.Clear();
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
                Log.Error("F>GetMenuSelection() has thrown an exception and was caught. {0}", _.Message);
                Log.Debug("F>GetMenuSelection() User has entered a non numeric key");
            }
        }
    }
    
    private void AddSession()
    {
        Console.Clear();
        Console.WriteLine("Add a new Coding Session.\n");
        Console.WriteLine("Please Note: Time should be in the 24hr format. So 23:00 is 11pm.");
        Console.WriteLine("Session Begin");
        DTSeperated begin = Input.GetSessionInfo();
        Console.WriteLine();
        Console.WriteLine("Session End");
        DTSeperated end = Input.GetSessionInfo();

        // TEST IF BEGIN IS greater THAN END
        var exitFlag = true;
        while (exitFlag)
        {
            if ((begin.Time >= end.Time)&&(begin.Date == end.Date))
            {
                Console.WriteLine("\nThe beginning time can not be equal or greater than the end time.  Please re-enter the begin time.");
                begin.Time = Input.GetTime();
            }
            else
                exitFlag = false;
        }

        // Show Output of both
        Console.WriteLine();
        Console.WriteLine($"Begin Session Info: {begin}");
        Console.WriteLine($"End Session Info:   {end}");
        TimeSpan span = CodingSession.CalculateDuration(begin.Time, end.Time);
        Console.WriteLine($"Duration is Days:{span.Days}\tHours:{span.Hours}\tMinutes:{span.Minutes}");
        // Add this to the Database Backend
        _session.CombineDTSeperated(begin, end);
        string success = _controller.Insert(_session) ? "Sucess" : "Failure";
        Console.WriteLine($"Session was added => {success}");


        Input.GetKeyReturnMenu();
    }

    private void UpdateSession()
    {
        Console.Clear();
        Console.WriteLine("Update a Session");

        Input.GetKeyReturnMenu();
    }

    private void DeleteSession()
    {
        Console.Clear();
        Console.WriteLine("Deleting a Code Session.");
        Console.WriteLine();
        List<CodingSession> sessions = GetAll();




        Input.GetKeyReturnMenu();
    }

    private void ShowAllSessions()
    {
        Console.Clear();
        Console.WriteLine("Show all Sessions");

        List<CodingSession> sessions = GetAll();
        if (sessions.Count == 0)
            Console.WriteLine("Table is empty");
        else
            TableEngine.DisplayAllRecords(sessions);

        Console.WriteLine();
        Input.GetKeyReturnMenu();
    }

    private List<CodingSession> GetAll() => _controller.ShowAllCodingSession();
}
