namespace CodingTrackerConsole;

public static class ProgramController
{
    private static readonly DatabaseManager DbManager = new();
    private static readonly UserInput Input = new();
    private static readonly TableVisualizationEngine DisplayRecords = new();
    private static List<CodingTrackerModel> _currentData = new();
    private static readonly FilterController FilterController = new();

    private static void DisplayMenu()
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("---------------------------\n");

        Console.WriteLine("What would you like to do?");
        Console.WriteLine("c to Close Application");
        Console.WriteLine("v to View All Records");
        Console.WriteLine("i to Insert a Record");
        Console.WriteLine("d to Delete a Record");
        Console.WriteLine("u to Update a Record\n");

        Console.WriteLine("Enter choice and hit Enter");
        Console.Write("Your choice? ");
    }

    public static void StartProgram()
    {
        DbManager.CreateDatabase();

        DisplayMenu();
        string choice = Input.GetChoice();

        while (choice != "c")
        {
            switch (choice)
            {
                case "v":
                    ViewRecords();
                    break;
                case "i":
                    GetRecordsToInsert();
                    break;
                case "d":
                    GetRecordsToDelete();
                    break;
                case "u":
                    SelectRecordToUpdate();
                    break;
                default:
                    Console.WriteLine("Wrong input!");
                    break;
            }

            DisplayMenu();
            choice = Input.GetChoice();
        }
    }

    private static void GetRecordsToInsert()
    {
        Console.Clear();

        var modelRecord = new CodingTrackerModel
        {
            Date = Input.GetDate(),
            StartTime = Input.GetTime(),
            EndTime = Input.GetTime(),
        };
        modelRecord.Duration = Input.GetDuration(modelRecord.StartTime, modelRecord.EndTime);

        DbManager.InsertRecord(modelRecord);

        DisplayTable();
    }

    private static void DisplayDeleteContextMenu()
    {
        Console.WriteLine("b to Go Back");
        Console.WriteLine("d to Delete Record: ");
    }

    private static void GetRecordsToDelete()
    {
        Console.Clear();
        DisplayTable();

        DisplayDeleteContextMenu();
        string choice = Input.GetChoice();
        while (choice != "b")
        {
            switch (choice)
            {
                case "d":
                    DbManager.DeleteRecord(Input.GetDate());
                    break;
                default:
                    Console.WriteLine("Invalid Choice!");
                    break;
            }
            
            DisplayTable();
            DisplayDeleteContextMenu();
            choice = Input.GetChoice();
        }
    }

    private static void DisplayTable()
    {
        LoadCurrentDataFromDb();
        DisplayRecords.BuildTable(_currentData);
    }

    private static void DisplayUpdateContextMenu()
    {
        Console.WriteLine("Choose what to update: ");
        Console.WriteLine("d to Update Date");
        Console.WriteLine("t to Update StartTime/EndTime");
        Console.WriteLine("a to Update All");
        Console.WriteLine("b to Go Back");

        Console.Write("Your choice? ");
    }

    private static void SelectRecordToUpdate()
    {
        Console.Clear();
        DisplayTable();

        Console.WriteLine("Select which record to update (by date)");
        string oldDate = Input.GetDate();

        DisplayUpdateContextMenu();
        string choice = Input.GetChoice();

        while (choice != "b")
        {
            CodingTrackerModel newRecord = new CodingTrackerModel();
            switch (choice)
            {
                case "d":
                    newRecord.Date = Input.GetDate();
                    DbManager.UpdateRecord(oldDate, newRecord);
                    break;
                case "t":
                    newRecord.StartTime = Input.GetTime();
                    newRecord.EndTime = Input.GetTime();
                    newRecord.Duration = Input.GetDuration(newRecord.StartTime, newRecord.EndTime);
                    DbManager.UpdateRecord(oldDate, newRecord);
                    break;
                case "a":
                    newRecord.Date = Input.GetDate();
                    newRecord.StartTime = Input.GetTime();
                    newRecord.EndTime = Input.GetTime();
                    newRecord.Duration = Input.GetDuration(newRecord.StartTime, newRecord.EndTime);
                    DbManager.UpdateRecord(oldDate, newRecord);
                    break;
                default:
                    Console.WriteLine("Invalid choice! Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }

            DisplayTable();
            DisplayUpdateContextMenu();
            choice = Input.GetChoice();
        }

        Console.Clear();
    }

    private static void ViewRecords()
    {
        LoadCurrentDataFromDb();
        Console.WriteLine("Do you wish to filter (yes/no)");
        var ans = Input.GetChoice();
        if (ans == "no")
        {
            DisplayRecords.View(_currentData);
            return;
        }

        var filteredList = FilterController.Filter(_currentData);
        DisplayRecords.View(filteredList);
    }

    private static void LoadCurrentDataFromDb()
    {
        _currentData = DataAccessService.LoadData();
    }
}