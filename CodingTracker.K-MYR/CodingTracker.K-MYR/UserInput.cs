using System.Globalization;

namespace CodingTracker.K_MYR;

internal class UserInput
{
    internal static void MainMenu()
    {
        bool endApp = false;
        while (!endApp)
        {
            Console.Clear();
            Console.WriteLine("Coding Tracker".PadLeft(18));
            Console.WriteLine("----------------------");
            Console.WriteLine("0 - Show All Records");
            Console.WriteLine("1 - Show Specific Records");
            Console.WriteLine("2 - Show Report");
            Console.WriteLine("3 - Insert Record");
            Console.WriteLine("4 - Update Record");
            Console.WriteLine("5 - Delete Record");            
            Console.WriteLine("6 - Stopwatch");
            Console.WriteLine("7 - Exit Application");
            Console.WriteLine("----------------------");

            Console.WriteLine("What do you want to do?");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "0":                    
                    Helpers.PrintAllRecords();
                    Console.WriteLine("\nPress enter to return to the main menu");
                    Console.ReadLine();
                    break;
                case "1":
                    ShowSpecificRecords();
                    Console.WriteLine("\nPress enter to return to the main menu");
                    Console.ReadLine();
                    break;
                case "2":
                    ShowReport();                    
                    break;
                case "3":
                    InsertRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    DeleteRecord();
                    break;
                case "6":
                    Stopwatch();
                    break;
                case "7":
                    Console.Clear();
                    Console.WriteLine("Goodbye");
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
        
    private static void InsertRecord()
    {
        Console.Clear();
        DateTime startTime;
        DateTime endTime;

        do
        {
            startTime = GetDateInput("Please enter the start time in a valid format (HH:mm dd-MM-yyyy)");
            endTime = GetDateInput("Please enter the end time in a valid format (HH:mm dd-MM-yyyy)");

        } while (startTime > endTime);       

        TimeSpan duration = endTime - startTime;

        SQLiteOperations.Insert(startTime.ToString("HH:mm dd-MM-yyyy"), endTime.ToString("HH:mm dd-MM-yyyy"), duration.ToString("hh\\:mm"));
    }

    private static void UpdateRecord()
    {
        Helpers.PrintAllRecords();

        while(true)
        {
            int input = GetNumberInput("\nPlease enter the id of the record you want to update or enter 0 to return to the main menu\n");

            if (input == 0) 
                break;

            if (SQLiteOperations.RecordExists(input) == 1)
            {
                DateTime startTime;
                DateTime endTime;

                do
                {
                    startTime = GetDateInput("Please enter the start time in a valid format (HH:mm dd-MM-yyyy)");
                    endTime = GetDateInput("Please enter the end time in a valid format (HH:mm dd-MM-yyyy)");

                } while (startTime >= endTime);

                TimeSpan duration = startTime - endTime;

                SQLiteOperations.Update(input, startTime.ToString("HH:mm dd-MM-yyyy"), endTime.ToString("HH:mm dd-MM-yyyy"), duration.ToString("hh\\:mm"));
                
                Console.Clear();
                Helpers.PrintAllRecords();
                Console.WriteLine($"Record with the Id {input} has been updated");
            }
            else
            {
                Console.WriteLine($"Record with the id {input} doesn't exist!");
            }
        }
    }

    private static void DeleteRecord()
    {
        Helpers.PrintAllRecords();

        while (true)
        {
            int input = GetNumberInput("\nPlease enter the id of the record you want to delete or enter 0 to return to the main menu\n");

            if (input == 0)
                break;

            if (SQLiteOperations.RecordExists(input) == 1)
            {
                SQLiteOperations.Delete(input);
                Console.Clear();
                Helpers.PrintAllRecords();
                Console.WriteLine($"Record with the id {input} has been deleted!");                
            }
            else
            {
                Console.WriteLine($"Record with the id {input} doesn't exist!");
            }
        }
    }

    private static void ShowSpecificRecords()
    {
        string unit = GetUnitOfTime();
        int numberOfTimeUnit = GetNumberOfTimeUnit();
        bool reverseSorting = GetSortingOrder();

        var records = Helpers.GetRecords(unit, numberOfTimeUnit);
        Helpers.PrintRecords(records, reverseSorting);
    }

    private static string GetUnitOfTime()
    {
        string unit;
        do
        {
            Console.Clear();
            Console.WriteLine("Please specify a unit of time");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("D - Days");
            Console.WriteLine("W - Weeks");
            Console.WriteLine("Y - Years");
            Console.WriteLine("-----------------------------\n");

            unit = Console.ReadLine().Trim().ToLower();

        } while (unit != "d" && unit != "w" && unit != "y");

        return unit;
    }

    private static bool GetSortingOrder()
    {
        string sortingOrder;

        Console.WriteLine("Do you want to sort the records ascending? (y/n)");

        do
        {
            sortingOrder = Console.ReadLine();
        } while (sortingOrder != "y" && sortingOrder != "n");

        bool reverseSorting = (sortingOrder == "y");

        return reverseSorting;
    }

    private static int GetNumberOfTimeUnit()
    {
        int timespanNumber = GetNumberInput("Please enter the number of the requested unit: ");

        return timespanNumber;
    }             

    internal static void ShowReport()
    {
        string unitChar = GetUnitOfTime();
        string unit = "";

        switch (unitChar)
        {
            case "d":
                unit = "day(s)";
                break;
            case "y":
                unit = "year(s)";
                break;
            case "w":
                unit = "week(s)";
                break;
        }

        int numberOfTimeUnit = GetNumberOfTimeUnit();

        var tableData = Helpers.GetRecords(unitChar, numberOfTimeUnit);
        var totalSpan = new TimeSpan(tableData.Sum(x => x.Duration.Ticks));
        var averageSpan = totalSpan / tableData.Count;
        var maxSpan = tableData.Max(x => x.Duration);


        Console.Clear();
        Console.WriteLine($"Your Statistics for the last {numberOfTimeUnit} {unit}");
        Console.WriteLine("----------------------------------");
        Console.WriteLine($"Total number of sessions: {tableData.Count}");
        Console.WriteLine($"Total amount of time    : {totalSpan}");
        Console.WriteLine($"Average amount of time  : {averageSpan}");
        Console.WriteLine($"Longest session         : {maxSpan}");
        Console.WriteLine("----------------------------------");
        Console.WriteLine("\nPress enter to go back to the main menu");
        Console.ReadLine();


    }

    internal static void Stopwatch()
    {
        string? input;
        DateTime startDate = new();
        DateTime endDate = new();
        TimeSpan duration = new(); 
        
        Helpers.PrintStopwatchMenu();

        do
        {            
            input = Console.ReadLine();

            if (input == "0")
            {
                if (startDate == DateTime.MinValue)
                {
                    startDate = DateTime.Now;
                    Helpers.PrintStopwatchMenu(startDate.ToString("t"));
                }
                else if (endDate == DateTime.MinValue)
                {
                    endDate = DateTime.Now;
                    duration = endDate - startDate;
                    Helpers.PrintStopwatchMenu(startDate.ToString("t"), endDate.ToString("t"), duration.ToString("hh\\:mm"));                    
                }
                else
                {
                    startDate = DateTime.Now;
                    endDate = DateTime.MinValue;
                    duration = TimeSpan.MinValue;
                    Helpers.PrintStopwatchMenu(startDate.ToString("t"));
                }
            }
            
            else if (input == "1")
            {
                if (endDate != DateTime.MinValue)
                {
                    SQLiteOperations.Insert(startDate.ToString("HH:mm dd-MM-yyyy"), endDate.ToString("HH:mm dd-MM-yyyy"), duration.ToString("hh\\:mm"));
                    Console.WriteLine("Session has beend saved!");
                }
                else
                {
                    Console.WriteLine("Please start and stop a session first!");
                }
            }

            else
            {
                Console.WriteLine("Invalid Input!");
            }

        } while (input != "2");
    }

    private static DateTime GetDateInput(string message)
    {
        Console.WriteLine(message);
        string? input = Console.ReadLine();
        DateTime date;
        while (!DateTime.TryParseExact(input, "HH:mm dd-MM-yyyy", new CultureInfo("de-DE"), DateTimeStyles.None, out date))
        {
            Console.WriteLine(message);
            input = Console.ReadLine();
        }
        return date;
    }

    private static int GetNumberInput(string message)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        while (!int.TryParse(input, out _) || Convert.ToInt32(input) < 0)
        {
            Console.Write(message);
            input = Console.ReadLine();
        }

        return Convert.ToInt32(input);
    }

}
