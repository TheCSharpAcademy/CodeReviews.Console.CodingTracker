namespace CodingTracker.jkjones98
{
    internal class MainMenu
    {
        CodingController controller = new();
        internal void DisplayMenu()
        {
            GetUserInputs getUserInputs = new();
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
                        // View records - read data from Db and display using ConsoleTableExt
                        controller.GetRecords();
                        break;
                    case "2":
                        // Insert record - 
                        getUserInputs.InsertRecord();
                        break;
                    case "3":
                        // Delete a record - 
                        getUserInputs.DeleteRecord();
                        break;
                    case "4":
                        // Update a record
                        getUserInputs.UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("\nInvalid entry. Please choose between 0 and 4");
                        break;
                }
            }
        }
    }
}