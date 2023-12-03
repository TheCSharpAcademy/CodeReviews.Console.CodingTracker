namespace CodingTracker;

class UserDisplay
{    
    public static string? MainMenu(SessionStopWatch currentSession, bool IsGoalSet)
    {
        string? userSelection;
        string currentSessionString;
        if(currentSession.IsRunning)
        {
            currentSessionString = $"5) Stop the current coding session. Currently you have been coding for {currentSession.SessionTimer:d\\.hh\\:mm}\n";
        }
        else
        {
            currentSessionString = "5) Start a new coding session\n";
        }

        string codingGoal;
        if(IsGoalSet)
        {
            codingGoal = "7) Check coding goal progress\n"+
                        "8) Delete coding goal\n";
        }
        else
        {
            codingGoal = "7) Create a coding goal\n";
        }

        Console.WriteLine("Please enter one of the following options:\n\n"+
            "1) Insert a new coding session record\n"+
            "2) Modify a coding session record\n"+
            "3) Delete a coding session record\n"+
            "4) Display records\n"+
            $"{currentSessionString}"+
            "6) Display a coding session report\n"+
            $"{codingGoal}"+
            "0) Exit the application\n"
        );

        userSelection = Console.ReadLine();

        return userSelection;
    }

    public static string?[] GetData()
    {
        string? [] data = new string[4];
        data[0] = UserInput.GetDate("of the coding session");
        data[1] = UserInput.GetTime();
        data[2] = UserInput.GetDate("of the coding session", data[0]);
        data[3] = UserInput.GetTime(data[0], data[1],data[2]);
        return data;
    }
    
    public static void InsertCodingSession()
    {
        string?[] data = GetData();
        CodingSession newCodingSession = new("1", data[0], data[1], data[2], data[3]);   
        DBOperations.InsertValue(newCodingSession.GetString());
        Console.WriteLine("Record succesfully inserted!\n");
    }
    
    public static string? FilterByDate()
    {
        string? answer = UserInput.GetYesNoAnswer("filter by date");
        return answer;
    }

    public static string? SortResults()
    {
        string? answer = UserInput.GetYesNoAnswer("sort the results");
        return answer;
    }
    
    public static void MofidyCodingSession()
    {
        List<CodingSession> modifyCodingSession = DisplayRecords();
        string? modifyCodingSessionID = UserInput.GetID(modifyCodingSession, "modify");
        string?[] data = GetData();
        
        CodingSession codingSession = new(modifyCodingSessionID, data[0],
        data[1], data[2], data[3]);
        DBOperations.UpdateValue(codingSession.GetString());
        Console.WriteLine("Record succesfully modified!\n");
    }

    public static List<CodingSession> DisplayRecords()
    {
        List<CodingSession> codingSessions = new();
        string? startDate;
        string? endDate;
        string? sortOperation;        

        string? modifyByDate = FilterByDate();
        if(modifyByDate == "y")
        {
            startDate = UserInput.GetDate("of the session");
            endDate = UserInput.GetDate("of the session",startDate);
        }   
        else
        {
            startDate = null;
            endDate = null;
        }

        string? sort = SortResults();
        if (sort == "y")
        {
            sortOperation = UserInput.GetSortOperation();
        }
        else
        {
            sortOperation = null;
        }

        codingSessions = DBOperations.SelectValue(startDate, endDate, sortOperation);
        DataVisualization.PrintTable(codingSessions);
        return codingSessions;
    }

    public static void DeleteCodingSession()
    {
        List<CodingSession> deleteCodingSession = DisplayRecords();
        string? deleteCodingSessionID = UserInput.GetID(deleteCodingSession, "delete");
        
        DBOperations.DeleteValue(deleteCodingSessionID);
        Console.WriteLine("Record succesfully deleted!\n");
    }

    public static SessionStopWatch StartCodingSession(SessionStopWatch currentSession)
    {
        currentSession.StartSession();
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        return currentSession;
    }

    public static SessionStopWatch StopCodingSession(SessionStopWatch currentSession)
    {
        string? stopCodingsession = UserInput.GetYesNoAnswer("finish your current session and save it");
        if(stopCodingsession == "y")
        {
            currentSession.EndSession();
            CodingSession codingSession = new(1, currentSession);
            DBOperations.InsertValue(codingSession.GetString());
            Console.WriteLine("The running session has been finished and stored!");
        }

        return currentSession;
    }
    public static void DisplayCodingReport()
    {
        string[] maxAndAverage;
        string? startDate;
        string? endDate;

        string? reportByDate = FilterByDate();
        if(reportByDate == "y")
        {
            startDate = UserInput.GetDate("of the coding report");
            endDate = UserInput.GetDate("of the coding report", startDate);
            maxAndAverage = DBOperations.GetTotalAndAverageValue(startDate,endDate);
        }   
        else
        {
            maxAndAverage = DBOperations.GetTotalAndAverageValue(null, null);
        }

        Console.WriteLine($"Your average length per coding session is {maxAndAverage[0]}");
        Console.WriteLine($"The total length of the coding sessions is {maxAndAverage[1]}\n");
    }

    public static CodingGoals SetCodingGoal()
    {
        string? startDate;
        string? endDate;
        string? codingGoalHours;

        startDate = UserInput.GetDate("of the coding goal");
        endDate = UserInput.GetDate("of the coding goal", startDate);
        codingGoalHours = UserInput.GetTotalHours();
        CodingGoals codingGoal = new(startDate, endDate, codingGoalHours);
        return codingGoal;
    }

    public static void DisplayCodingGoal(CodingGoals codingGoal)
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        string[] totalAndAverage;
        string[] codingGoalString = codingGoal.GetString();
        totalAndAverage = DBOperations.GetTotalAndAverageValue(codingGoalString[0], codingGoalString[1]);
        bool goalAchieved = codingGoal.GoalAchieved(totalAndAverage[1]);

        Console.WriteLine($"Coding Goal Start date: {codingGoalString[0]} End date: {codingGoalString[1]} Total Hours: {codingGoalString[2]}");

        if(goalAchieved)
        {
            Console.WriteLine("Congratulations, you have achieved your coding goal");
        }
        else
        {
            TimeSpan averageHours = codingGoal.DailyAverage(totalAndAverage[1]);
            if(averageHours<TimeSpan.Zero)
            {
                Console.WriteLine("You didn't reached your coding goal");
            }
            else
            {
                Console.WriteLine($"You need to code on average {averageHours:d\\.hh\\:mm} each day to achieve your goal\n");
            }
        }
    }

    public static bool DeleteCodingGoal()
    {
        bool setCodingGoal = true;
        string? answer = UserInput.GetYesNoAnswer("delete the current coding goal");
        if(answer == "y")
        {
            setCodingGoal = false;
        }
        return setCodingGoal; 
    }
}