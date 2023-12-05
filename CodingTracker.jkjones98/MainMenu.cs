namespace CodingTracker.jkjones98
{
    internal class MainMenu
    {
        CodingController controller = new();
        internal void DisplayMenu()
        {
            GetUserInputs getUserInputs = new();
            ReportMenu reportMenu = new();
            bool closeApp = false;
            while(!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nSelect what you would like to do from the options below");
                Console.WriteLine("\nEnter 0 to close the application");
                Console.WriteLine("Enter 1 to view records");
                Console.WriteLine("Enter 2 to insert a record");
                Console.WriteLine("Enter 3 to delete a record");
                Console.WriteLine("Enter 4 to update a record");
                Console.WriteLine("Enter 5 to time todays session");
                Console.WriteLine("Enter 6 to set goals");
                Console.WriteLine("Enter 7 to view goals");
                Console.WriteLine("Enter 8 to view reports");

                string selectionInput = Console.ReadLine();

                while(string.IsNullOrEmpty(selectionInput))
                {
                    Console.WriteLine("\nInvalid selection. Please choose an option from the list above");
                    selectionInput = Console.ReadLine();
                }

                switch(selectionInput)
                {
                    case "0":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        getUserInputs.ViewRecords();
                        break;
                    case "2":
                        getUserInputs.InsertRecord();
                        break;
                    case "3":
                        getUserInputs.DeleteRecord();
                        break;
                    case "4":
                        getUserInputs.UpdateRecord();
                        break;
                    case "5":
                        getUserInputs.StopWatchMenu();
                        break;
                    case "6":
                        getUserInputs.AddGoals();
                        break;
                    case "7":
                        controller.GetGoalRecords();
                        break;  
                    case "8":
                        reportMenu.DisplayReportMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid entry. Please choose between 0 and 8");
                        break;
                }
            }
        }
    }
}