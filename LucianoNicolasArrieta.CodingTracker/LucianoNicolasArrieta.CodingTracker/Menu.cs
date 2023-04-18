namespace coding_tracker
{
    internal class Menu
    {
        public void PrintMenu()
        {
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
                choosen_option = Console.ReadLine();

                switch (choosen_option)
                {
                    case "i":
                        Console.WriteLine("Insert");
                        //Insert();
                        break;
                    case "u":
                        Console.WriteLine("Update");
                        //Update();
                        break;
                    case "v":
                        Console.WriteLine("View");
                        //ViewRecords();
                        break;
                    case "d":
                        Console.WriteLine("Delete");
                        //Delete();
                        break;
                    /*
                    case "r":
                        ViewReports();
                        break;
                    */
                    case "0":
                        closeApp = true;
                        Console.WriteLine("See you!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Error: Invalid option. Try again");
                        break;
                }
            }
        }
    }
}