using CodingTracker.library.Model;
using System.Globalization;
using static CodingTracker.library.Controller.Reports;
using CodingTracker.library.View;

namespace CodingTracker.library.Controller;

internal static class Helpers
{
    internal static bool GetBackToMainMenu(string message, string message2)
    {
        Console.WriteLine($"Type '{message}' to {message2} or type 'exit' to get back to main menu");
        Console.Write("Your choice: ");
        string getBack = Console.ReadLine();

        while (getBack.ToLower().Trim() != message && getBack != "exit")
        {
            Console.WriteLine($"\nInvalid input.\nType '{message}' to {message2} or type 'exit' to get back to main menu");
            Console.Write("Your choice: ");
            getBack = Console.ReadLine();
        }

        if (getBack == "exit") return true;

        else return false;
    }

    internal static string GetDateTime(string message, string startTime = "", string operation = "")
    {
        Console.WriteLine($"\nEnter {message} date and time in format dd-MM-yyyy HH:mm");
        string dateTime;

        if (operation == "endTime")
        {
            dateTime = Validations.GetValidDateTime(startTime, Console.ReadLine());
        }

        else
        {
            dateTime = Validations.GetValidDateTime(Console.ReadLine());
        }

        return dateTime;
    }

    internal static double Duration(string startTime, string endTime)
    {
        DateTime start = DateTime.ParseExact(startTime, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None);
        DateTime end = DateTime.ParseExact(endTime, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None);

        TimeSpan durationSpan = end.Subtract(start);

        double duration = durationSpan.TotalMinutes;

        return duration;
    }

    internal static int GetSessionId(string message)
    {
        Console.WriteLine($"\nEnter id of a session you want to {message}");
        int sessionId = Validations.GetValidSessionId(Console.ReadLine(), message);

        return sessionId;
    }

    internal static bool GetBackToReportMenu(string message, string message2)
    {
        Console.WriteLine($"Type '{message}' to {message2} or type 'report menu' to get back to main menu");
        Console.Write("Your choice: ");
        string getBack = Console.ReadLine();

        while (getBack.ToLower() != message && getBack != "report menu")
        {
            Console.WriteLine($"\nInvalid input.\nType '{message}' to {message2} or type 'report menu' to get back to main menu");
            Console.Write("Your choice: ");
            getBack = Console.ReadLine();
        }

        if (getBack == "report menu") return true;

        else return false;
    }

    internal static string GetYear()
    {
        Console.Clear();
        Console.Write("Enter year in format 'yyyy': ");
        string year = Console.ReadLine();

        while (!int.TryParse(year, out _) || year.Length != 4)
        {
            Console.WriteLine("\nInvalid input.\nPlease try again");
            Console.Write("Enter year in format yyyy: ");
            year = Console.ReadLine();
        }

        return year;
    }

    internal static string GetMonth()
    {
        Console.Clear();
        Console.Write("Enter month in format 'MM' in digits: ");
        string month = Console.ReadLine();

        bool isValid = int.TryParse(month, out int monthNumber);

        while (!isValid || monthNumber < 0 || monthNumber > 12)
        {
            Console.WriteLine("\nInvalid input.\nPlease try again");
            Console.Write("Enter month in format 'MM' in digits: ");
            month = Console.ReadLine();
            isValid = int.TryParse(month, out monthNumber);
        }

        return month;
    }

    internal static string GetDay(string monthName)
    {
        Console.Clear();
        Console.Write("Enter day in format 'dd' in digits: ");
        string day = Console.ReadLine();

        bool isValid = int.TryParse(day, out int dayNumber);

        if (monthName == "February")
        {
            while (!isValid || dayNumber < 0 || dayNumber > 28)
            {
                Console.WriteLine("\nInvalid input.\nPlease try again");
                Console.Write("Enter day in format 'dd' in digits: ");
                day = Console.ReadLine();
                isValid = int.TryParse(day, out dayNumber);
            }
        }

        else if(monthName == "April" || monthName == "June" || monthName == "September" || monthName == "November")
        {
            while (!isValid || dayNumber < 0 || dayNumber > 30)
            {
                Console.WriteLine("\nInvalid input.\nPlease try again");
                Console.Write("Enter day in format 'dd' in digits: ");
                day = Console.ReadLine();
                isValid = int.TryParse(day, out dayNumber);
            }
        }

        else
        {
            while (!isValid || dayNumber < 0 || dayNumber > 31)
            {
                Console.WriteLine("\nInvalid input.\nPlease try again");
                Console.Write("Enter day in format 'dd' in digits: ");
                day = Console.ReadLine();
                isValid = int.TryParse(day, out dayNumber);
            }
        }

        return day;
    }

    internal static string GetMonthName(string month)
    {
        string monthName = "";

        switch (Convert.ToInt32(month))
        {
            case 1:
                monthName = MonthsName.January.ToString();
                break;

            case 2:
                monthName = MonthsName.February.ToString();
                break;

            case 3:
                monthName = MonthsName.March.ToString();
                break;

            case 4:
                monthName = MonthsName.April.ToString();
                break;

            case 5:
                monthName = MonthsName.May.ToString();
                break;

            case 6:
                monthName = MonthsName.June.ToString();
                break;

            case 7:
                monthName = MonthsName.July.ToString();
                break;

            case 8:
                monthName = MonthsName.August.ToString();
                break;

            case 9:
                monthName = MonthsName.September.ToString();
                break;

            case 10:
                monthName = MonthsName.October.ToString();
                break;

            case 11:
                monthName = MonthsName.November.ToString();
                break;

            case 12:
                monthName = MonthsName.December.ToString();
                break;
        }

        return monthName;
    }

    internal static void GetOrderedList(List<CodingSessions> sessions)
    {
        Console.Clear();

        List<CodingSessions> records;

        Console.WriteLine("Do you wish to order session by id or duration?\nType 'unordered'to view unordered data.");
        string order = Validations.ValidateOrder(Console.ReadLine());

        if (order.ToLower().Trim() == "id")
        {
            Console.WriteLine("\nDo you wish to order session ascending or descending?");
                string orderType = Validations.ValidateOrderType(Console.ReadLine());

            if (orderType.ToLower().Trim() == "ascending")
            {
                records = sessions.OrderBy(session => session.SessionId).ToList();
                TableVisualizationEngine.PrintSessions(records);
            }

            else
            {
                records = sessions.OrderByDescending(session => session.SessionId).ToList();
                TableVisualizationEngine.PrintSessions(records);
            }

        }
    
        else if (order.ToLower().Trim() == "duration")
        {
            Console.WriteLine("\nDo you wish to order session ascending or descending");
            string orderType = Validations.ValidateOrderType(Console.ReadLine());

            if (orderType.ToLower().Trim() == "ascending")
            {
                records = sessions.OrderBy(session => session.Duration).ToList();
                TableVisualizationEngine.PrintSessions(records);
            }

            else
            {
               records = sessions.OrderByDescending(session => session.Duration).ToList();
               TableVisualizationEngine.PrintSessions(records);
            }

        }

        else
        {
            TableVisualizationEngine.PrintSessions(sessions);
        }
        
    }

    internal static string GetMonthYear()
    {
        DateOnly date = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
        string dateString = date.ToString("dd-MM-yyyy");

        string monthYear = dateString.Substring(dateString.IndexOf('-') + 1);

        return monthYear;
    }

    internal static double GetGoalHours()
    {
        Console.WriteLine("Enter a coding sessions goal for the week in hours: ");
        double hours = Validations.ValidateCodingGoalHour(Console.ReadLine());

        return hours;
    }

}