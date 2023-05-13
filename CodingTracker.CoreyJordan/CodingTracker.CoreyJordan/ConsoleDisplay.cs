using CodingTrackerLibrary;
using ConsoleTableExt;

namespace CodingTracker.CoreyJordan;
internal class ConsoleDisplay
{
    internal string Bar { get; set; } = "*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\n";


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

    internal void DisplaySessions(List<CodingSessionModel> sessions, string title)
    {
        Console.Clear();

        if (sessions.Count == 0)
        {
            Console.WriteLine("No records found");
            return;
        }

        sessions.Sort((x, y) => x.Status.CompareTo(y.Status));

        ConsoleTableBuilder
            .From(sessions)
            .WithTitle(title)
            .ExportAndWriteLine();
        Console.WriteLine();
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
        Console.WriteLine("Press anykey to continue...");
        Console.ReadKey();
    }

    internal void Success(string prompt)
    {
        Console.WriteLine($"{prompt} successfully");
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }
}
