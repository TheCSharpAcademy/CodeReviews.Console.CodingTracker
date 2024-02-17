using System.Globalization;

namespace CodingTracker.library.Controller;

internal static class Validations
{
    internal static string GetMenuChoice(string choice)
    {
        while (string.IsNullOrEmpty(choice))
        {
            Console.WriteLine("\nInvalid input\nPlease try again.");
            Console.Write("Your choice: ");
            choice = Console.ReadLine();
        }

        return choice;
    }

    internal static string GetValidDateTime(string dateTime)
    {
        string format = "dd-MM-yyyy HH:mm";

        bool isValid = DateTime.TryParseExact(dateTime, format, null, DateTimeStyles.None, out DateTime date);
        
        while(!isValid || date > DateTime.Now)
        {
            Console.Clear();
            Console.WriteLine("Start date and time can't be in the future or invalid format was inserted.\nPlease try again");
            Console.Write($"Insert date and time in format {format}: ");
            dateTime = Console.ReadLine();
            isValid = DateTime.TryParseExact(dateTime, format, null, DateTimeStyles.None, out date);

        }

        return dateTime;
    }

    internal static string GetValidDateTime(string startTime, string endTime)
    {
        string format = "dd-MM-yyyy HH:mm";

        DateTime startDate = DateTime.ParseExact(startTime, format, new CultureInfo("en-US"), DateTimeStyles.None);

        bool isValid = DateTime.TryParseExact(endTime, format, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime endDate);
        

        while (!isValid || endDate > DateTime.Now || endDate < startDate)
        {
            Console.Clear();
            Console.WriteLine("End date and time can't be in the future or before start time or invalid format was inserted.\nPlease try again");
            Console.Write($"Insert date and time in format { format}: ");
            endTime = Console.ReadLine();
            isValid = DateTime.TryParseExact(endTime, format, null, DateTimeStyles.None, out endDate);
        }

        return endTime;
    }

    internal static int GetValidSessionId(string session, string message) 
    {
        int sessionId = 0;

        while(!int.TryParse(session, out sessionId))
        {
            Console.WriteLine("Invalid input.");
            Console.Write($"\nEnter id of a session you want to {message}: ");
            session = Console.ReadLine();
        }

        return sessionId;
    }

    internal static string TerminateAutomaticSession(string choice)
    {
        while (choice.ToLower().Trim() != "stop")
        {
            Console.WriteLine("\nInvalid input\nPlease try again.");
            Console.Write("Your input: ");
            choice = Console.ReadLine();
        }

        return choice;
    }

    internal static string InsertAutomaticSessionChoice(string choice)
    {
        while(choice.ToLower().Trim() != "yes" &&  choice.ToLower().Trim() != "no")
        {
            Console.WriteLine("\nInvalid input\nPlease try again.");
            Console.Write("Your input: ");
            choice = Console.ReadLine();
        }

        return choice;
    }

    internal static string ValidateOrder(string order) 
    {
        while(order.ToLower().Trim() != "id" &&  order.ToLower().Trim() != "duration" && order.ToLower().Trim() != "unordered")
        {
            Console.WriteLine("\nInvalid input.");
            Console.Write("Do you wish to order session by id or duration or to view unordered data? ");
            order = Console.ReadLine();
        }

        return order;
    }

    internal static string ValidateOrderType(string orderType)
    {
        while (orderType.ToLower().Trim() != "ascending" && orderType.ToLower().Trim() != "descending")
        {
            Console.WriteLine("\nInvalid input.");
            Console.Write("Do you wish to order session ascending or descending? ");
            orderType = Console.ReadLine();
        }

        return orderType;
    }

    internal static double ValidateCodingGoalHour(string codingGoalHour) 
    {
        double goalHours = 0;
      
        while(!double.TryParse(codingGoalHour, out goalHours)) 
        {
            Console.Write("Please enter valid goal hours (no decimals): ");
            codingGoalHour = Console.ReadLine();
        }

        return goalHours;
    }
}
