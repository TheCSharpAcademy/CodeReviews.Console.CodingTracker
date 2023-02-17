namespace CodingTracker;

class Program
{
    // DONT MOVE INTO MainMenu()
    //this make here so that when creating a new entry
    // the default would not overwrite the new table
    static DatabaseCreation database = new();

    static void Main()
    {
        MainMenu();
    }

    static void MainMenu()
    {
       
        CrudController action = new();
        UserInput input = new();
        
        bool endApp = false;

        while (!endApp)
        {
            
            Console.Clear();
            Console.WriteLine("\n-------------------------------\n");
            Console.WriteLine("\tCODING TRACKER");
            Console.WriteLine("\n-------------------------------\n");
            Console.WriteLine($"Welcome, {database.Name.Replace("_", " ")}!. Today {(DateTime.Now).ToString("dd - MM - yyyy")}");
            Console.WriteLine("\n0*. Start Tracking. . .");
            Console.WriteLine("-------------------------------");            
            Console.WriteLine("1. View Records.");
            Console.WriteLine("2. Insert Records.");
            Console.WriteLine("3. Delete Records.");
            Console.WriteLine("4. Update Records.");
            Console.WriteLine("5*. See Record Report.");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("6. CREATE YOUR OWN TRACKER !");            
            Console.WriteLine("-------------------------------\n");
            Console.WriteLine("7. Exit.\n");
            Console.Write("Your option: ");

            var userOption = Console.ReadLine();
            Console.Clear();

            switch (userOption)
            {
                case "0":
                    input.StartingTimerCount(database.Name);
                    TaskComplete();                                        
                    break;
                case "1":
                    action.GetAllRecords(database.Name);
                    TaskComplete();
                    break;
                case "2":
                    input.GetRecordFromUser(database.Name);
                    TaskComplete();
                    break;
                case "3":
                    action.Delete(database.Name);
                    TaskComplete();
                    break;
                case "4":
                    action.Update(database.Name);
                    TaskComplete();
                    break;
                case "5":
                    ReportTable(database.Name);
                    TaskComplete();
                    break;
                case "6":
                    database.Name = database.CreateNewRecord();                    
                    TaskComplete();
                    break;
                case "7":
                    Console.WriteLine("\nGoodbye!\n");
                    Environment.Exit(0);
                    break;
                default:
                    Console.Write("\nInvalid input! Please try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    //Maybe a better way to do this but in the meantime this method looks good
    internal static void TaskComplete()
    {
        Thread.Sleep(500);
        Console.WriteLine("-------------------------------");
        Console.Write("Task completed. Press ENTER to continue.");
        Console.ReadLine();
        MainMenu();
    }

    static void ReportTable(string table)
    {
        CrudController action = new();
        UserInput input = new();

        bool reportable = action.Report(table);
        bool endReport = false;

        while (reportable && !endReport)
        {
            action.GetAllRecords(table);
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"\tREPORT OPTION\r");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. Filter Record & see Report");            
            Console.WriteLine("2. Set your own goals");
            Console.WriteLine("3. Return to Menu");
            Console.WriteLine("-------------------------------");
            Console.Write("Your option: ");
            var userOption = Console.ReadLine();
                        
            switch(userOption)
            {
                case "1":
                    Console.Clear();
                    action.GetAllRecords(table);
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine($"FILTER");
                    Console.WriteLine("-------------------------------");
                    action.ReportWithTimeFilter(table);                    
                    break;
                case "2":
                    input.GetUserGoal(table);                    
                    break;
                case "3":                    
                    break;
                default:
                    Console.Write("\nInvalid input! Please try again.");
                    Console.ReadLine();
                    ReportTable(table);
                    break;
            }
            return;
        }        
    }    
}