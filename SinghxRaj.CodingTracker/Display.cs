namespace SinghxRaj.CodingTracker;

internal class Display
{
    internal static void PrintIntro()
    {
        Console.WriteLine("Welcome to Coding Tracker!");
        Console.WriteLine("With Coding Tracker, you can " +
                    "keep track of all your coding sessions.");
        Console.WriteLine();
    }

    internal static void PrintOptions()
    {
        Console.WriteLine("Type 0 - Exit application");
        Console.WriteLine("Type 1 - Add new coding session");
        Console.WriteLine("Type 2 - View coding sessions");
        Console.WriteLine("Type 3 - Delete a coding session");
        Console.WriteLine("Type 4 - Update a coding session");
    }

    internal static void PrintOutro()
    {
        Console.WriteLine("Exiting Coding Tracker.");
        Console.WriteLine("Press any key to close...");
        Console.ReadKey();
    }
}

