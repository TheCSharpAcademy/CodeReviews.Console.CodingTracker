namespace CodingTracker.kraven88.ConsoleUI;

public static class Helpers
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
}
