namespace CodingTracker;

class UserInput
{
    public static string? GetDate(string option)
    {
        bool startDateInvalid = true;
        string? startDate;

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        Console.WriteLine($"Please write the start date {option} in the format yyyy/MM/dd");
        do
        {
            startDate = Console.ReadLine();
            if(DataValidation.ValidateDate(startDate))
            {
                startDateInvalid = false;
            }
            else
            {
                Console.WriteLine("The date you entered is invalid. Please enter a date in the format yyyy/MM/dd");
            }
        }
        while(startDateInvalid);

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return startDate;
    }

    public static string? GetDate(string option, string? startDate)
    {
        bool endDateInvalid = true;
        string? endDate;

        Console.Clear(); 
        Console.WriteLine("\x1b[3J");
        Console.Clear();       
        Console.WriteLine($"Please write the end date {option} in the format yyyy/MM/dd");
        do
        {
            endDate = Console.ReadLine();
            if(DataValidation.ValidateDate(startDate, endDate))
            {
                endDateInvalid = false;
            }
            else
            {
                Console.WriteLine("The date you entered is invalid. Please enter a date in the format yyyy/MM/dd");
            }
        }
        while(endDateInvalid);

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return endDate;
    }

    public static string? GetTime()
    {
        bool startTimeInvalid = true;
        string? startTime;

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        Console.WriteLine("Please write the starting time of the session in the format HH:mm");
        do
        {
            startTime = Console.ReadLine();
            if(DataValidation.ValidateTime(startTime))
            {
                startTimeInvalid = false;
            }
            else
            {
                Console.WriteLine("The time you entered is invalid. Please enter a time in the format HH:mm");            
            }
        }
        while(startTimeInvalid);
        
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return startTime;
    }

    public static string? GetTime(string? startDate, string? startTime, string? endDate)
    {
        bool endTimeInvalid = true;
        string? endTime;

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
        Console.WriteLine("Please write the end time of the session in the format HH:mm");
        do
        {
            endTime = Console.ReadLine();
            if(DataValidation.ValidateTime(startDate, startTime, endDate, endTime))
            {
                endTimeInvalid = false;
            }
            else
            {
                Console.WriteLine("The time you entered is invalid. Please enter a time in the format HH:mm");            
            }
        }
        while(endTimeInvalid);

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return endTime;
    }

    public static string? GetYesNoAnswer(string question)
    {
        bool answerInvalid = true;
        string? answer;

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        Console.WriteLine($"Do you want to {question} y/n?");
        do
        {
            answer = Console.ReadLine();
            if(DataValidation.ValidateYesNoQuestion(answer))
            {
                answerInvalid = false;
            }
            else
            {
                Console.WriteLine("The answer is invalid. Please answer y/n");
            }
        }
        while(answerInvalid);

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return answer;
    }

    public static string? GetSortOperation()
    {
        bool sortOperationInvalid = true;
        string? sortOperation;

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        Console.WriteLine("Please select one of the following values\n"+
            "1) Ascending\n"+
            "2) Descending\n");
        
        do
        {
            sortOperation = Console.ReadLine();
            if(DataValidation.ValidateInteger(sortOperation, 1, 2))
            {
                sortOperationInvalid = false;   
            }
            else
            {
                Console.WriteLine("The value you entered is invalid. Please enter a valid option:");
            }
        }
        while(sortOperationInvalid);

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return sortOperation;
    }

    public static string? GetID(List<CodingSession> modifyCodingSession, string option)
    {
        string? selectedID;
        bool IDInvalid = true;

        Console.WriteLine($"Please enter the ID you want to {option}");

        do
        {
            selectedID = Console.ReadLine();
            if(DataValidation.ValidateInteger(selectedID) && modifyCodingSession.Any(
            modifyCodingSession=>modifyCodingSession.ID==Convert.ToInt32(selectedID)))
            {
                IDInvalid = false;
            }
            else
            {
                Console.WriteLine("The value you entered is invalid. Please enter a valid value:");
            }
        }
        while(IDInvalid);

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return selectedID;
    }

    public static string? GetTotalHours()
    {
        string? totalHours;
        bool totalHoursInvalid = true;
        
        Console.Clear(); 
        Console.WriteLine("\x1b[3J");
        Console.Clear();       
        Console.WriteLine("Please write the total hours of the coding goal in the format d.HH:mm");
        do
        {
            totalHours = Console.ReadLine();
            if(DataValidation.ValidateTotalHours(totalHours))
            {
                totalHoursInvalid = false;
            }
            else
            {
                Console.WriteLine("The total hours you entered is invalid. Please enter the hours in the format d.HH:mm");
            }
        }
        while(totalHoursInvalid);

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        return totalHours;
    }
}