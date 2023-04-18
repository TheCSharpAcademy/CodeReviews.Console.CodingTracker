using System;

namespace coding_tracker
{
    public class Menu
    {
        public void PrintMenu()
        {
            Console.WriteLine("Welcome to Coding Tracker App")
            Console.WriteLine("\n---------------Menu---------------");
            Console.WriteLine(@"Type 'i' to Insert Record
    Type 'u' to Update Record
    Type 'v' to View All Records
    Type 'd' to Delete Record
    Type 0 to Close the App
    ----------------------------------"); // Type 'r' to View Reports
        }

        public void GetUserOption()
        {
            bool closeApp = false;
            string choosen_option;
            while (!closeApp)
            {
                PrintMenu();
                user_input = Console.ReadLine();

                switch (choosen_option)
                {
                    case "i":
                        Insert();
                        break;
                    case "u":
                        Update();
                        break;
                    case "v":
                        ViewRecords();
                        break;
                    case "d":
                        Delete();
                        break;
                    case "r":
                        ViewReports();
                        break;
                    case "0":
                        close = true;
                        Console.WriteLine("See you!");
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}