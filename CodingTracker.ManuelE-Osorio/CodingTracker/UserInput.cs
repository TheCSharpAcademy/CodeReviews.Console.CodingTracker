class UserInput
{
    public static string? GetDate()
    {
        bool startDateInvalid = true;
        string? startDate;

        Console.Clear();
        Console.WriteLine("Please write the starting date of the session in the format yyyy/MM/dd");
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
        return startDate;
    }

    public static string? GetDate(string? startDate)
    {
        bool endDateInvalid = true;
        string? endDate;

        Console.Clear();        
        Console.WriteLine("Please write the end date of the session in the format yyyy/MM/dd");
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

        return endDate;
    }

    public static string? GetTime()
    {
        bool startTimeInvalid = true;
        string? startTime;

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
        
        return startTime;
    }

    public static string? GetTime(string? startDate, string? startTime, string? endDate)
    {
        bool endTimeInvalid = true;
        string? endTime;

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

        return endTime;
    }

    public static string? GetYesNoAnswer(string question)
    {
        bool answerInvalid = true;
        string? answer;

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

        return answer;
    }
}