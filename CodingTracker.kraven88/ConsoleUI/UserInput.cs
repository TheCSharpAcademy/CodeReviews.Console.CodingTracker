using CodingTracker.kraven88.Models;

namespace CodingTracker.kraven88.ConsoleUI;

public static class UserInput
{
    public static string AskForDate(string dateType)
    {
        var format = "yyyy-MM-dd";

        Console.Write($"Please enter {dateType} date in the [{format}] format: ");
        
        return Console.ReadLine()!.Trim().ValidateDate(format);
    }
    
    public static string AskForID(List<CodingSession> list)
    {
        Console.Write("Please select the 'Id' of the session you want to delete (0 to return): ");
        var input = Console.ReadLine()!.Trim();
        if (input == "0") return "0";
        else return input.ValidateID(list);
    }

    public static string AskForTime()
    {
        var format = "HH:mm";

        Console.Write($"Please enter time in [{format}], 24h format: ");

        return Console.ReadLine()!.Trim().ValidateTime(format);
    }

    public static string SelectMenuItem(string availableItems)
    {
        Console.Write("Select: ");
        var selection = Console.ReadLine()!.Trim();
        
        if (availableItems.Contains(selection) == false || selection.Length > 1)
        {
            Console.WriteLine("Invalid input. Please select from the available options" + MainMenu.nl);
            selection = SelectMenuItem(availableItems);
        }

        return selection;
    }
}
