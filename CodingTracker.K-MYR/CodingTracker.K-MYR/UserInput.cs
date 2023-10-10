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
            Console.WriteLine("0  - Show All Records");
            Console.WriteLine("1  - Show Specific Records");
            Console.WriteLine("2  - Show Report");
            Console.WriteLine("3  - Add Record");
            Console.WriteLine("4  - Update Record");
            Console.WriteLine("5  - Delete Record");
            Console.WriteLine("6  - Stopwatch");
            Console.WriteLine("7  - Show All Goals");
            Console.WriteLine("8  - Show Current Goals");
            Console.WriteLine("9  - Add Goal");
            Console.WriteLine("10 - Update Goal");
            Console.WriteLine("11 - Delete Goal");
            Console.WriteLine("12 - Exit Application");
            Console.WriteLine("----------------------");

            Console.WriteLine("What do you want to do?");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    ShowAllRecords();
                    break;
                case "1":
                    ShowSpecificRecords();
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
                    ShowGoals();
                    break;
                case "8":
                    ShowCurrentGoals();
                    break;
                case "9":
                    InsertGoal();
                    break;
                case "10":
                    UpdateGoal();
                    break;
                case "11":
                    DeleteGoal();
                    break;
                case "12":
                    Console.Clear();
                    Console.WriteLine("Goodbye!");
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
        DateTime startTime = GetDateInput("Please enter the start time in a valid format (hh:mm dd-mm-yyyy)");
        DateTime endTime = GetDateInput("Please enter the end time in a valid format (hh:mm dd-MM-yyyy)");

        while (startTime > endTime)
        {
            Console.WriteLine("The start can't be after the end!");
            startTime = GetDateInput("Please enter the start time in a valid format (hh:mm dd-mm-yyyy)");
            endTime = GetDateInput("Please enter the end time in a valid format (hh:mm dd-mm-yyyy)");
        }

        TimeSpan duration = endTime - startTime;

        SQLiteOperations.InsertRecord(startTime.ToString("HH:mm:ss dd-MM-yyyy"), endTime.ToString("HH:mm:ss dd-MM-yyyy"), duration.ToString("dd\\:hh\\:mm\\:ss"));

        Helpers.AdjustElapsedTime(startTime);
    }

    private static void UpdateRecord()
    {
        Helpers.PrintAllRecords();

        while (true)
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
                    startTime = GetDateInput("Please enter the start time in a valid format (hh:mm dd-mm-yyyy)");
                    endTime = GetDateInput("Please enter the end time in a valid format (hh:mm dd-mm-yyyy)");

                } while (startTime >= endTime);

                TimeSpan duration = startTime - endTime;

                SQLiteOperations.UpdateRecord(input, startTime.ToString("HH:mm:ss dd-MM-yyyy"), endTime.ToString("HH:mm:ss dd-MM-yyyy"), duration.ToString("dd\\:hh\\:mm\\:ss"));

                Helpers.AdjustElapsedTime(startTime);

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
        var records = Helpers.PrintAllRecords();

        while (true)
        {
            int input = GetNumberInput("\nPlease enter the id of the record you want to delete or enter 0 to return to the main menu\n");

            if (input == 0)
                break;

            if (SQLiteOperations.RecordExists(input) == 1)
            {
                SQLiteOperations.DeleteRecord(input);
                var record = records.Where(x => x.Id == input).ToList();
                Helpers.AdjustElapsedTime(record[0].StartTime);

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

    private static void ShowAllRecords()
    {
        Helpers.PrintAllRecords();
        Console.WriteLine("\nPress enter to return to the main menu");
        Console.ReadLine();
    }

    private static void ShowSpecificRecords()
    {
        string unit = GetUnitOfTime();
        int numberOfTimeUnit = GetNumberInput("Please enter the number of the requested unit: ");
        bool reverseSorting = GetSortingOrder();

        var records = Helpers.GetRecords(unit, numberOfTimeUnit);
        Helpers.PrintRecords(records, reverseSorting);

        Console.WriteLine("\nPress enter to return to the main menu");
        Console.ReadLine();
    }

    private static void ShowReport()
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

        int numberOfTimeUnit = GetNumberInput("Please enter the number of the requested unit: ");

        var tableData = Helpers.GetRecords(unitChar, numberOfTimeUnit);

        Console.Clear();

        if (tableData.Count > 0)
        {
            var totalSpan = new TimeSpan(tableData.Sum(x => x.Duration.Ticks));
            var averageSpan = totalSpan / tableData.Count;
            var maxSpan = tableData.Max(x => x.Duration);

            Console.Clear();
            Console.WriteLine($"Your statistics for the last {numberOfTimeUnit} {unit}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Total number of sessions: {tableData.Count}");
            Console.WriteLine($"Total amount of time    : {totalSpan}");
            Console.WriteLine($"Average amount of time  : {averageSpan}");
            Console.WriteLine($"Longest session         : {maxSpan}");
            Console.WriteLine("----------------------------------");
        }
        else
        {
            Console.WriteLine("No records were found");
        }

        Console.WriteLine("\nPress enter to go back to the main menu");
        Console.ReadLine();
    }

    private static void Stopwatch()
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
                    Helpers.PrintStopwatchMenu(startDate.ToString("T"));
                }
                else if (endDate == DateTime.MinValue)
                {
                    endDate = DateTime.Now;
                    duration = endDate - startDate;
                    Helpers.PrintStopwatchMenu(startDate.ToString("T"), endDate.ToString("T"), duration.ToString("dd\\:hh\\:mm\\:ss"));
                }
                else
                {
                    startDate = DateTime.Now;
                    endDate = DateTime.MinValue;
                    duration = TimeSpan.MinValue;
                    Helpers.PrintStopwatchMenu(startDate.ToString("T"));
                }
            }

            else if (input == "1")
            {
                if (endDate != DateTime.MinValue)
                {
                    SQLiteOperations.InsertRecord(startDate.ToString("HH:mm:ss dd-MM-yyyy"), endDate.ToString("HH:mm:ss dd-MM-yyyy"), duration.ToString("dd\\:hh\\:mm\\:ss"));
                    Helpers.AdjustElapsedTime(startDate);
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

    private static void ShowGoals()
    {
        Console.Clear();
        Helpers.PrintAllGoals();
        Console.WriteLine("\nPress enter to return to the main menu");
        Console.ReadLine();
    }

    private static void ShowCurrentGoals()
    {
        var tableData = Helpers.GetActiveGoals();
        Helpers.PrintGoals(tableData);

        Console.WriteLine("\nPress enter to return to the main menu");
        Console.ReadLine();
    }

    private static void InsertGoal()
    {
        DateTime startDate;
        DateTime deadline;
        TimeSpan goal;

        Console.Clear();

        string name = GetStringInput("Please enter the name of the goal\n");

        do
        {
            startDate = GetDateInput("Please enter a start date for your goal in a valid format (dd-mm-yyyy)", format: "dd-MM-yyyy");
            deadline = GetDateInput("Please enter a end date for your goal in a valid format (dd-mm-yyyy)", format: "dd-MM-yyyy");
        } while (startDate.Date >= deadline.Date);

        goal = GetTimeSpanInput("Please enter your time goal in a valid format (dd:hh:mm)");

        TimeSpan elapsedTime = Helpers.CalculateElapsedTime(startDate, deadline);

        SQLiteOperations.InsertGoal(name, startDate.ToString("dd-MM-yyyy"), deadline.ToString("dd-MM-yyyy"), goal.ToString("dd\\:hh\\:mm\\:ss"), elapsedTime.ToString("dd\\:hh\\:mm\\:ss"));
    }

    private static void UpdateGoal()
    {
        Helpers.PrintAllGoals();

        DateTime startDate;
        DateTime deadline;
        TimeSpan goal;

        while (true)
        {
            int input = GetNumberInput("\nPlease enter the id of the goal you want to update or enter 0 to return to the main menu\n");

            if (input == 0)
                break;

            if (SQLiteOperations.GoalExists(input) == 1)
            {
                string name = GetStringInput("Please enter the name of the goal\n");

                do
                {
                    startDate = GetDateInput("Please enter a start date for your goal in a valid format (dd-mm-yyyy)", format: "dd-MM-yyyy");
                    deadline = GetDateInput("Please enter a end date for your goal in a valid format (dd-mm-yyyy)", format: "dd-MM-yyyy");
                } while (startDate.Date >= deadline.Date);

                goal = GetTimeSpanInput("Please enter your goal in a valid format (dd:hh:mm)");

                TimeSpan elapsedTime = Helpers.CalculateElapsedTime(startDate, deadline);

                SQLiteOperations.UpdateGoal(input, name, startDate.ToString("dd-MM-yyyy"), deadline.ToString("dd-MM-yyyy"), goal.ToString("dd\\:hh\\:mm\\:ss"), elapsedTime.ToString("dd\\:hh\\:mm\\:ss"));

                Console.Clear();
                Helpers.PrintAllGoals();
                Console.WriteLine($"Goal with the Id {input} has been updated");
            }
            else
            {
                Console.WriteLine($"Goal with the id {input} doesn't exist!");
            }
        }
    }

    private static void DeleteGoal()
    {
        Helpers.PrintAllGoals();

        while (true)
        {
            int input = GetNumberInput("\nPlease enter the id of the goal you want to delete or enter 0 to return to the main menu\n");

            if (input == 0)
                break;

            if (SQLiteOperations.GoalExists(input) == 1)
            {
                SQLiteOperations.DeleteGoal(input);
                Console.Clear();
                Helpers.PrintAllGoals();
                Console.WriteLine($"Goal with the id {input} has been deleted!");
            }
            else
            {
                Console.WriteLine($"Goal with the id {input} doesn't exist!");
            }
        }
    }

    private static string GetUnitOfTime()
    {
        Console.Clear();
        Console.WriteLine("Please specify a unit of time");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("D - Days");
        Console.WriteLine("W - Weeks");
        Console.WriteLine("Y - Years");
        Console.WriteLine("-----------------------------\n");

        string unit = Console.ReadLine().Trim().ToLower();

        while (unit != "d" && unit != "w" && unit != "y")
        {
            Console.WriteLine("Invalid input!");
            unit = Console.ReadLine().Trim().ToLower();
        }

        return unit;
    }

    private static bool GetSortingOrder()
    {

        Console.WriteLine("Do you want to sort the records ascending? (y/n)");
        string sortingOrder = Console.ReadLine().Trim().ToLower();

        while (sortingOrder != "y" && sortingOrder != "n")
        {
            Console.WriteLine("Invalid Input!");
            sortingOrder = Console.ReadLine().Trim().ToLower();
        }

        bool reverseSorting = (sortingOrder == "n");

        return reverseSorting;
    }

    private static DateTime GetDateInput(string message, string format = "HH:mm dd-MM-yyyy")
    {
        DateTime date;

        Console.WriteLine(message);
        string? input = Console.ReadLine();


        while (!DateTime.TryParseExact(input, format, new CultureInfo("de-DE"), DateTimeStyles.None, out date))
        {
            Console.WriteLine("Invalid Input! " + message);
            input = Console.ReadLine();
        }

        return date;
    }

    private static TimeSpan GetTimeSpanInput(string message)
    {
        TimeSpan timeSpan;

        Console.WriteLine(message);
        string? input = Console.ReadLine();

        while (!TimeSpan.TryParseExact(input, "dd\\:hh\\:mm", new CultureInfo("de-DE"), TimeSpanStyles.None, out timeSpan) || timeSpan.Ticks <= 0)
        {
            Console.WriteLine("Invalid Input! " + message);
            input = Console.ReadLine();
        }

        return timeSpan;
    }

    private static int GetNumberInput(string message)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        while (!int.TryParse(input, out _) || Convert.ToInt32(input) < 0)
        {
            Console.Write("Invalid Input! " + message);
            input = Console.ReadLine();
        }

        return Convert.ToInt32(input);
    }

    private static string GetStringInput(string message)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        while (string.IsNullOrEmpty(input))
        {
            Console.Write("Invalid Input!" + message);
            input = Console.ReadLine();
        }

        return input;
    }
}
