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
        _session.ValidateDate(begin, end);
        _session.ValidateTime(begin, end);      
        // Show Output of both
        Console.WriteLine("\n");
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
        Console.WriteLine("Update a Session Start or End Time");
        Console.WriteLine();
        List<CodingSession> sessions = GetAll();

        if (sessions.Count != 0)
        {
            TableEngine.DisplayAllRecords(sessions);
            Console.WriteLine();
            int idUpdated = Input.GetNumberFromList(_controller.GetValidId(), false);
            if (idUpdated != -1)
            {
                CodingSession single = sessions.First(x => x.Id == idUpdated);
                Console.WriteLine(single);
                Console.Write("Update S)tart time / E)nd time, C)ancel update.");
                char option = Input.GetUpdateOptions();
                if (option == 'C')
                {
                    Console.WriteLine();
                    Console.WriteLine("Okay. Nothing will be updated");
                }
                else if (option == 'S')
                {
                    DTSeperated oldBegin = single.SeperateBegin();

                    Console.WriteLine();
                    DTSeperated begin = Input.GetSessionInfo();

                }
                else if (option == 'E')
                {
                    Console.WriteLine();
                    Console.WriteLine("Updating Start End Date and Time");
                    Console.WriteLine("Also duration has been updated.");
                }
            }               
        }
        else
            Console.WriteLine("There are currently no coding session to update.");
        Input.GetKeyReturnMenu();
    }

    private void DeleteSession()
    {
        Console.Clear();
        Console.WriteLine("Deleting a Code Session.");
        Console.WriteLine();
        List<CodingSession> sessions = GetAll();

        if (sessions.Count == 0)
        {
            Console.WriteLine("There are currently no coding session to delete.");            
        }
        else
        {
            TableEngine.DisplayAllRecords(sessions);
            Console.WriteLine();
            int idDeleted = Input.GetNumberFromList(_controller.GetValidId(), true);
            if (idDeleted != -1)
            {
                Console.WriteLine("\nDo you wish to delete this (Y/N)");
                if (Input.GetYesNo())
                {
                    var success = _controller.DeleteSession(idDeleted);
                    if (success)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Session has been successfully deleted.");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Session has not been deleted.");
                    }
                }
            }
        }
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
