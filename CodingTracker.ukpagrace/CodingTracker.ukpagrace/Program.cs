using System.Globalization;
using DatabaseLibrary;
using Application.Entities;

class CodingController
{
    readonly Database database = new();
    bool stop = false;
    static void Main()
    {
        CodingController controller = new CodingController();
        controller.CreateTable();
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
            Console.WriteLine("Input your start time in the format of YYYY-MM-DD HH-MM");
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
            case 1:
                Console.WriteLine("Range or a specific year");
                Console.WriteLine("1 - range");
                Console.WriteLine("2 - specific year");
                option = Console.ReadLine();
                if (option == "1")
                {
                    Console.WriteLine("Enter the first range value");
                    var firstRange = Console.ReadLine();
                    Console.WriteLine("Enter the second range value");
                    var firstRange = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Enter the year to filter");
                    var firstRange = Console.ReadLine();
                }
                break;
            case 2:
                Console.WriteLine("Range or a specific month");
                Console.WriteLine("1 - range");
                Console.WriteLine("2 - specific month");
                option = Console.ReadLine();
                if (option == "1")
                {
                    Console.WriteLine("Enter the first range value, in the format of 'yyyy-mm'");
                    var firstRange = Console.ReadLine();
                    Console.WriteLine("Enter the second range value, in the format of 'yyyy-mm'");
                    var firstRange = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Enter the month to filter, in the format of 'yyyy-mm'");
                    var firstRange = Console.ReadLine();
                }
                break;
        }
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
}
