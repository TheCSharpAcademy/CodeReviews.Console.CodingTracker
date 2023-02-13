namespace CodingTracker.kraven88.ConsoleUI;

public static class UserInput
{
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

    public static string AskForDate(string dateType)
    {
        var format = "yyyy-MM-dd";

        Console.Write($"Please enter {dateType} date in the [{format}] format: ");
        
        return Console.ReadLine()!.Trim().ValidateDate(format);
    }

    public static string AskForTime()
    {
        var format = "HH:mm";

        Console.Write($"Please enter time in [{format}], 24h format: ");

        return Console.ReadLine()!.Trim()!.ValidateTime(format);
    }
}
