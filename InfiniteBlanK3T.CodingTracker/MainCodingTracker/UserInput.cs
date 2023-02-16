namespace CodingTracker;

public class UserInput
{                
    Validation val = new();
    
    public void GetRecordFromUser(string table)
    {
        CrudController action = new();
        string dateInsert = GetDate();
        List<int> timeInsert = GetUserTime();
        action.Insert(table,dateInsert, timeInsert);
    }

    public List<int> GetConcurrentTimeList(string start, string end)
    {            
        var startT = start.Split(":").Select(Int32.Parse).ToList();
        var endT = end.Split(":").Select(Int32.Parse).ToList();
        List<int> timeList = startT.Concat(endT).ToList();
        timeList.Add(val.CalculateDuration(timeList));

        return timeList;
    }

    public string GetDate()
    {            
        Console.Write("Insert the date (Format yyyy-MM-dd): ");
        string dateInput = Console.ReadLine();

        while(!val.CheckDateInput(dateInput))
        {
            Console.Write("\nInvalid date. (Formate: yyyy-MM-dd). Try again: ");
            dateInput = Console.ReadLine();
        }           

        return dateInput;
    }

    internal int GetHour(string input)
    {
        Console.Write(input);
        string numberInput = Console.ReadLine();

        while (!val.CheckHourInput(numberInput))
        {
            Console.Write("Invalid number. Hour has to be between (0-23). Try again: ");
            numberInput = Console.ReadLine();

        }

        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }

    internal int GetMin(string input)
    {
        Console.Write(input);
        string numberInput = Console.ReadLine();

        while (!val.CheckMinInput(numberInput))
        {
            Console.Write("Invalid number. Minute has to be between (0-60). Try again: ");
            numberInput = Console.ReadLine();

        }

        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }

    public List<int> GetUserTime()
    {
        // Set a list in which endTime Hour is less than
        // StartTime then loop if endT still < StartT.
        int[] defaultTime = { 1, 0, 0, 0 };
        List<int> timeInsert = new(defaultTime);
        var endLoop = false;

        //Not sure this is the optimal way to do this?
        while (!endLoop)
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Time format - 24 hour");
            timeInsert.Clear();

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine("Start Time");
                }
                else
                {
                    Console.WriteLine("---------------");
                    Console.WriteLine("End Time");
                }
                int timeInputHour = GetHour("Hour: ");
                int timeInputMinute = GetMin("Minute: ");
                timeInsert.Add(timeInputHour);
                timeInsert.Add(timeInputMinute);
            }
            if (timeInsert[2] > timeInsert[0] || timeInsert[2] == timeInsert[0] && timeInsert[3] > timeInsert[1])
            {
                endLoop = true;                                       
            }
            else
            {
                Console.WriteLine("EndTime and StartTime are invalid. Press ENTER to try again.");
                Console.ReadLine();
                Console.Clear();
            }
        }

        timeInsert.Add(val.CalculateDuration(timeInsert));
        return timeInsert;
    }        

    public void StartingTimerCount(string table)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string startTime = DateTime.Now.ToString("HH:mm");
        string endTime = DateTime.Now.ToString("HH:mm");

        Console.WriteLine($"Timer start now.\nToday: {DateTime.Now.ToString("dd - MM - yyyy")}.");
        Console.WriteLine("-------------------------------\n");
        Console.WriteLine($"Start Time: {startTime}");
        Console.Write("\nPress SPACEBAR to stop the Timer");
        if (Console.ReadKey().Key == ConsoleKey.Spacebar)
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Console.WriteLine($"Stop Time: {endTime}");
            Console.WriteLine("-------------------------------");
            List<int> timerList = GetConcurrentTimeList(startTime, endTime);
            CrudController action = new();
            action.Insert(table, date, timerList);
            Console.WriteLine("Timer added to the record.");                
        }
    }

    public string GetMonthQueryReport()
    {
        Console.Write("Choose a month (01-12) OR press ENTER to choose a whole year: ");
        var input = Console.ReadLine();

        if (input == "") { return ""; }

        while (!val.CheckMonthInputReport(input))
        {
            Console.Write("Invalid month input. Try again: ");
            input = Console.ReadLine();
        }

        return input;
    }

    public string GetYearQueryReport()
    {
        Console.Write($"Choose a year (Format: yyyy) OR press ENTER for Current Year ({DateTime.Now.Year}): ");
        var input = Console.ReadLine();

        if (input == "") { return DateTime.Now.Year.ToString(); }

        while (!val.CheckYearInputReport(input))
        {
            Console.Write("Invalid month input. Try again: ");
            input = Console.ReadLine();
        }

        return input;
    }

    public string? ReportSorting(string prompt)
    {
        Console.Write(prompt);

        var option = Console.ReadLine();

        switch (option)
        {
            case "a":
                return "ASC";
            case "d":
                return "DESC";
            default:
                ReportSorting(prompt);
                //no way it reaches here
                return null;
        }
    }

    public List<string> GetReportFilter()
    {
        List<string> filtersOption = new();
        string yearQuery = GetYearQueryReport(); filtersOption.Add(yearQuery);
        string monthQuery = GetMonthQueryReport(); filtersOption.Add(monthQuery);
        string sortOption = ReportSorting("Sort by: Ascending(a)/Descending(d). (a/d)? "); filtersOption.Add(sortOption);

        return filtersOption;
    }

    public void GetUserGoal(string record)
    {
        CrudController action = new();
        DatabaseCreation goal = new("Goal");
        goal.CheckGoalTableExist();
        Console.Clear();
        Console.WriteLine("-------------------------------");
        bool makeGoal = action.GoalExists(record);
        

        if (makeGoal)
        {
            action.GetGoal(record);
            Console.Write("Do you want to create new goal? (y/n): ");
            var input = Console.ReadLine();
            switch (input)
            {
                case "y":
                    makeGoal = false;
                    break;
                case "n":
                    makeGoal = true;
                    break;
                default:
                    Console.WriteLine("Invalid input. Press ANY to return.");
                    Console.ReadLine();
                    return;
            }
        }
        //IF put else here will skip this part
        if (!makeGoal)
        {
            Console.Clear();
            Console.WriteLine("-------------------------------");
            Console.WriteLine("In MINUTE put down your time (60 MIN = 1 HOUR)");
            var userTimePerDay = val.GetNumber("Time per day: ");

            while (userTimePerDay > 600) 
            {
                Console.Write("Don't spend more than 10 hours on computer! Please, set your time again! - ");
                userTimePerDay = val.GetNumber("Time per day: ");
            }
            var userGoal = val.GetNumber("Goal: ");
            try
            {
                if (action.GoalExists(record))
                {
                    action.UpdateGoal(record, userTimePerDay, userGoal);                        
                }
                else
                {
                    action.InsertGoal(record, userTimePerDay, userGoal);                        
                }                    
                Thread.Sleep(1000);
                action.GetGoal(record);                    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }            
    }
}
