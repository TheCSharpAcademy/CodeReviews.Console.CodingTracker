namespace CodingTracker.CoreyJordan;
public class ConsoleDisplay
{
    public string Bar { get; set; } = "*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\n";

    internal void DisplayMainMenu()
    {
        Console.WriteLine($"{Bar}\t\tMAIN MENU\n{Bar}");
        Console.WriteLine("\tQ: Quit Coding Tracker");
        Console.WriteLine("\tV: View previous sessions");
        Console.WriteLine("\tN: Start new coding session");
        Console.WriteLine("\tE: End ongoing coding session");
        Console.WriteLine("\tD: Delete coding session");
        Console.Write("\n\tWhat would you like to do? ");
    }

    internal void DisplayOpenSessions()
    {
        Console.WriteLine($"{Bar}\tPlaceholder Open Session\n{Bar}");
    }

    internal void InvalidInput(string input)
    {
        if (input == string.Empty)
        {
            Console.Write("\nMust enter a value. ");
        }
        else
        {
            Console.Write($"\n\"{input}\" is not valid. ");
        }
        Console.WriteLine("Please try again.");
        Console.Write("Press anykey to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}
