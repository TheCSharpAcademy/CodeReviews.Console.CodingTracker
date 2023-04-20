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
Type 't' to Track your session while you code
Type 'f' to View Records between two dates
Type 0 to Close the App
----------------------------------"); // Type 'r' to View Reports
        }

        public void GetUserOption()
        {
            bool closeApp = false;
            string choosen_option;
            UserInput userInput = new UserInput();
            CodingController codingController = new CodingController();
            while (!closeApp)
            {
                PrintMenu();
                choosen_option = Console.ReadLine().Trim().ToLower();

                switch (choosen_option)
                {
                    case "i":
                        CodingSession codSession = userInput.CodingSessionInput();
                        codingController.Insert(codSession);
                        break;
                    case "u":
                        codingController.ViewAll();
                        int idToUpdate = userInput.IdInput();
                        codingController.Update(idToUpdate);
                        break;
                    case "v":
                        codingController.ViewRecords();
                        break;
                    case "d":
                        codingController.ViewAll();
                        int idToDelete = userInput.IdInput();
                        codingController.Delete(idToDelete);
                        break;
                    case "t":
                        CodingSession currentSession = new CodingSession();
                        currentSession.TrackCurrentSession();
                        codingController.Insert(currentSession);
                        break;
                    case "f":
                        string from = userInput.DateInput();
                        string to = userInput.DateInput();
                        string order = userInput.OrderInput();
                        codingController.ViewRecordBetweenDates(from, to,order);
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
                        Console.Clear();
                        Console.WriteLine("Error: Invalid option. Try again");
                        break;
                }
            }
        }
    }
}