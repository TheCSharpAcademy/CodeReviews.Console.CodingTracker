namespace CodingTracker.kmakai.Controllers;

public class InputController
{
    static public string GetDateInput()
    {
        Console.WriteLine("Enter date (yyyy-mm-dd): ");
        DateOnly date;
        while (!DateOnly.TryParse(Console.ReadLine(), out date))
        {
            Console.WriteLine("Invalid date format. Try again (yyyy-mm-dd): ");
        }

        return date.ToString("yyyy-MM-dd");
    }

    static public string GetStartTimeInput()
    {
        Console.WriteLine("Enter Start time (hh:mm) this is in 24hrs formatt: ");
        TimeOnly time;
        while (!TimeOnly.TryParse(Console.ReadLine(), out time))
        {
            Console.WriteLine("Invalid time format. Try again (hh:mm) this is in 24hrs format: ");
        }

        return time.ToString("hh\\:mm");
    }

    static public string GetEndTimeInput(string startTime)
    {
        Console.WriteLine("Enter End time(hh:mm) this is in 24hrs formatt: ");
        TimeOnly time;
        while (!TimeOnly.TryParse(Console.ReadLine(), out time))
        {
            Console.WriteLine("Invalid time format. Try again (hh:mm) this is in 24hrs format: ");
        }

        if(Convert.ToDateTime(time.ToString("hh\\:mm")) <= Convert.ToDateTime(startTime))
        {
            Console.WriteLine("End time must be greater than start time. Try again (hh:mm) this is in 24hrs format: ");
            return GetEndTimeInput(startTime);
        }

        return time.ToString("hh\\:mm");
    }

    static public int GetIdInput()
    {
        Console.Write("Id: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid id. Try again: ");
        }

        return id;
    }

    static public int GetMainMenuInput()
    {
        Console.WriteLine("Enter your choice: ");
        int input;
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Invalid input. Try again: ");
        }

        return input;
    }
}

