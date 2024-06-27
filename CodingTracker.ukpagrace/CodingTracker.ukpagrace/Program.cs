using System.Globalization;
using DatabaseLibrary;
using Application.Entities;

class CodingController
{
    readonly Database database = new();
    readonly Goal goal = new();
    bool stop = false;
    static void Main()
    {
        CodingController controller = new CodingController();
        controller.CreateTable();
        controller.CreateGoalTable();
        Console.WriteLine("Welcome to Coding Tracker, where we will track your coding hours");
        bool endApp = false;
        while (!endApp)
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("1 - Insert");
            Console.WriteLine("2 - Update");
            Console.WriteLine("3 - List");
            Console.WriteLine("4 - Delete");
            Console.WriteLine("5 - StopWatch");
            Console.WriteLine("6 - Filter records");
            Console.WriteLine("7 - Get Report");
            Console.WriteLine("8 - Set A goal");
            Console.WriteLine("9 - See Goal progress");
            Console.WriteLine("0 - Quit");

            Console.WriteLine("Select an option from the menu");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    controller.InsertTable();
                    break;
                case "2":
                    controller.UpdateTable();
                    break;
                case "3":
                    controller.ListTable();
                    break;
                case "4":
                    controller.DeleteTable();
                    break;
                case "5":
                    controller.StartWatch();
                    break;
                case "6":
                    controller.FilterRecords();
                    break;
                case "7":
                    controller.GetReport();
                    break;
                case "8":
                    controller.SetGoal();
                    break;
                case "9":
                    controller.SeeGoalProgress();
                    break;
                case "0":
                    Console.WriteLine("Goodbye\n");
                    endApp = true;
                    break;
                default:
                    Console.WriteLine("Invalid option, please select an option from the menu");
                    break;
            }
        }
    }

   DateTime GetStartDate()
   {
        Console.WriteLine("Input your start time in the format of YYYY-MM-DD HH:MM");
        var dateInput1 = Console.ReadLine();

        DateTime startDate;


        while (!DateTime.TryParseExact(dateInput1, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            Console.WriteLine("Input your start time in the format of YYYY-MM-DD HH:MM");
            dateInput1 = Console.ReadLine();
        }

        return startDate;
   }

    DateTime GetEndDate()
    {
        Console.WriteLine("Input your end time in the format of YYYY-MM-DD HH:MM");
        var dateInput2 = Console.ReadLine();

        DateTime endDate;
        while (!DateTime.TryParseExact(dateInput2, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
        {
            Console.WriteLine("Input your end time in the format of YYYY-MM-DD HH-MM");
            dateInput2 = Console.ReadLine();
        }
        return endDate;
    }

    int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        var input = Console.ReadLine();

        while (!int.TryParse(input, out _) && Convert.ToInt32(input) < 0)
        {
            Console.WriteLine("Enter a valid integer");
            input = Console.ReadLine();
        }

        return Convert.ToInt32(input);
    }

    void CreateTable()
    {
        database.Create();
    }
    void CreateGoalTable()
    {
        goal.Create();
    }

    void SetGoal()
    {
        Console.WriteLine("Set a coding goal for this month, goal must be an integer");

        var userInput = Console.ReadLine();
        int goalInput;

        while(!int.TryParse(userInput, out goalInput) || Convert.ToInt32(userInput) < 0)
        {
            Console.WriteLine("Enter a valid goal");
            userInput = Console.ReadLine();
        }
        DateTime date = DateTime.Now;
        string month = $"{date.Year} - {date.Month:D2}";

        goal.Insert(month, goalInput);

        GetAverageCodePerDay();

    }

    void SeeGoalProgress()
    {
        DateTime date = DateTime.Now;
        string month = $"{date.Year} - {date.Month:D2}";
        goal.GoalProgress(month);
    }

    void GetAverageCodePerDay()
    {
        DateTime date = DateTime.Now;
        string month = $"{date.Year} - {date.Month:D2}";
        int days = Convert.ToInt32(DateTime.DaysInMonth(date.Year, date.Month));
        double codePerDay = goal.AverageTimePerDay(month, days);
        TimeSpan timespan = TimeSpan.FromHours(codePerDay);
        string timespanString = FormatTimeSpan(timespan);

        Console.WriteLine("------------------------------------------------\n"); ;
        Console.WriteLine($"To achieve your goal this month you have to code a minimun of {timespanString} hours everdays, GoodLuck");
        Console.WriteLine("------------------------------------------------\n"); ;
    }
    void InsertTable()
    {
        DateTime startDate = GetStartDate();
        DateTime endDate = GetEndDate();

        while (startDate > endDate) {
            Console.WriteLine("start date cannot be greater than end date");
            endDate = GetEndDate();

        }
        TimeSpan duration = endDate - startDate;
        database.Insert(startDate, endDate, duration);
    }

    void ListTable()
    {
        database.List();
    }

    void UpdateTable()
    {
        database.List();
        int id = GetNumberInput("Enter the Id your want to update");

        UserEntity userEntity = database.GetOne(id);
        Console.WriteLine($"{userEntity.Id}, {userEntity.StartDate}, {userEntity.EndDate}, {userEntity.Duration}");
        Console.WriteLine("select the column you want to update");
        Console.WriteLine("1 - update start Date");
        Console.WriteLine("2 - update end Date");
        var option = Console.ReadLine();


        if (option == "1")
        {
            DateTime startDate = GetStartDate();
            DateTime endDate;

            DateTime.TryParseExact(userEntity.EndDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

            while (startDate > endDate)
            {
                Console.WriteLine("Start Date cannot greater than Start Date");
                startDate = GetStartDate();
            }

            TimeSpan duration = endDate - startDate;

            database.Update(id, startDate, endDate, duration);

        }
        else if (option == "2") {
            DateTime endDate = GetEndDate();
            DateTime startDate;

            DateTime.TryParseExact(userEntity.StartDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            while (endDate < startDate)
            {
                Console.WriteLine("End Date cannot be less than Start Date");
                endDate = GetEndDate();
            }

            TimeSpan duration = endDate - startDate;

            database.Update(id, startDate, endDate, duration);
        }
        else
        {
            Console.WriteLine("Select an option from the menu");
        }
    }

    public void DeleteTable()
    {
        database.List();
        int id = GetNumberInput("Enter the Id your want to delete");
        database.Delete(id);
    }

    public void FilterRecords()
    {
        Console.WriteLine("Select a filter option");
        Console.WriteLine("1 - year");
        Console.WriteLine("2 - month");
        Console.WriteLine("3 - days");

        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                FilterTable("%Y", "year", "yyyy eg 2024");
                break;
            case "2":
                FilterTable("%Y-%m", "months", "yyyy-mm eg 2024-06");
                break;
            case "3":
                FilterTable("%Y-%m-%d", "day", "yyyy-mm-dd eg 2024-06-24");
                break;
            default:
                Console.WriteLine("InvalidInput, Select an option from menu");
                FilterRecords();
                break;
        }
    }

    public void FilterTable (string format, string filter, string filterFormat)
    {
        Console.WriteLine($"Range or a specific {filter}");
        Console.WriteLine("1 - range");
        Console.WriteLine($"2 - specific {filter}");
        var option = Console.ReadLine();
        if (option == "1")
        {
            Console.WriteLine($"Enter the first range value in the format of {filterFormat}");
            var firstRange = Console.ReadLine();
            Console.WriteLine($"Enter the second range value {filterFormat}");
            var secondRange = Console.ReadLine();
            string order = GetOrderInput();
            database.Filter(format, firstRange, secondRange, order);
        }
        else
        {
            Console.WriteLine($"Enter the {filter} to filter in the format of {filterFormat}");
            var firstRange = Console.ReadLine();
            string order = GetOrderInput();
            database.Filter(format, firstRange, order);
        }
    }


    void GetReport()
    {

        DateTime dateTime = DateTime.Now;
        string month = $"{dateTime.Year}-{dateTime.Month:D2}";

        database.Analyze("%Y-%m", month);
    }

    string GetOrderInput()
    {
        Console.WriteLine("1 - ascending order");
        Console.WriteLine("2 - descending order");
        var orderInput = Console.ReadLine();

        while (orderInput != "1" && orderInput != "2")
        {
            Console.WriteLine("Select an option from the menu");
            orderInput = Console.ReadLine();
        }
        string order = (orderInput == "1") ? "ASC" : "DESC";
        return order;
    }

    public void StartWatch()
    {
        DateTime startDate = DateTime.Now;
        DateTime interval;
        Console.WriteLine("To Stop watch enter 0");
        Thread inputThread = new Thread(new ThreadStart(StopWatch));
        inputThread.Start();

        while (!stop) {
            interval = DateTime.Now;
            Console.Clear();
            Console.WriteLine($"{interval.ToString("yyyy-MM-dd hh:mm:ss")}");
            Thread.Sleep(1000);
        }
        DateTime endDate = DateTime.Now;
        TimeSpan duration = GetDuration(startDate, endDate);

        database.Insert(startDate, endDate, duration);
    }

    public TimeSpan GetDuration(DateTime startDate, DateTime endDate)
    {
        return endDate - startDate;
    }

    public void StopWatch()
    {
        while (!stop)
        {
            var input = Console.ReadLine();

            if (input == "0")
            {
                stop = true;
            }
        }
    }

    public string FormatTimeSpan(TimeSpan duration)
    {
        List<string> parts = new List<string>();
        if (duration.Days > 0)
        {
            parts.Add($"{duration.Days} {(duration.Days == 1 ? "day" : "days")}");
        }
        if (duration.Hours > 0)
        {
            parts.Add($"{duration.Hours} {(duration.Hours == 1 ? "hour" : "hours")}");
        }
        if (duration.Minutes > 0)
        {
            parts.Add($"{duration.Minutes} {(duration.Minutes == 1 ? "minute" : "minutes")}");
        }
        if (duration.Seconds > 0)
        {
            parts.Add($"{duration.Seconds} {(duration.Seconds == 1 ? "second" : "seconds")}");
        }
        return string.Join(", ", parts);
    }
}
