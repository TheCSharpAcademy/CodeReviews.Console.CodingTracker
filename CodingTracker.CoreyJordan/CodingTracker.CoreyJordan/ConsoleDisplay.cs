using CodingTrackerLibrary;
using ConsoleTableExt;

namespace CodingTracker.CoreyJordan;
internal class ConsoleDisplay
{
    internal string Bar { get; set; } = "*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*\n";


    internal void DisplayMainMenu()
    {
        Console.WriteLine($"{Bar}\t\tMAIN MENU\n{Bar}");
        Console.WriteLine("\tQ: Quit Coding Tracker");
        Console.WriteLine("\tV: View previous sessions");
        Console.WriteLine("\tN: Start new coding session");
        Console.WriteLine("\tE: End ongoing coding session");
        Console.WriteLine("\tL: Record live coding session");
        Console.WriteLine("\tD: Delete coding session");
        Console.WriteLine("\tS: Set new coding goal (Overwrites existing)");
        Console.Write("\n\tWhat would you like to do? ");
    }

    internal void DisplayReport(CodingReportModel report, CodingGoalModel goal)
    {
        Console.Clear();

        Console.WriteLine($"{Bar}\t\tCODING REPORT\n{Bar}");
        Console.WriteLine($"\tCoding Sessions: {report.ReportCount}");

        Console.Write($"\tTotal time coding: {report.Time.Days} Days ");
        Console.Write($"{report.Time.Hours} Hrs ");
        Console.WriteLine($"{report.Time.Minutes} Min");

        Console.Write($"\tAverage/session: {report.Average.Days} Days ");
        Console.WriteLine($"{report.Average.Hours} Hrs {report.Average.Minutes} Min");

        Console.WriteLine($"\n{Bar}\t\tCODING GOAL\n{Bar}");
        Console.Write($"\t{goal.Name}: {goal.Goal.Days} Days ");
        Console.WriteLine($"{goal.Goal.Hours} Hrs {goal.Goal.Minutes} Min");

        Console.WriteLine($"\tPercenatge complete: {report.Time / goal.Goal:p0}");
        TimeSpan remaining = goal.Goal - report.Time;
        Console.WriteLine($"\tTime remaining: {remaining.Days} Days {remaining.Hours} Hrs {remaining.Minutes} Min");

        Console.WriteLine($"\n{Bar}");
        Console.WriteLine("\n\tPress any key to return...");
        Console.ReadKey();
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
            Console.Write($"\n{input} is not valid. ");
        }
        Console.WriteLine("Please try again.");
        Console.WriteLine("Press anykey to continue...");
        Console.ReadKey();
    }

    internal void RangeMenu()
    {
        Console.WriteLine($"{Bar}\t\tFILTER BY\n{Bar}");
        Console.WriteLine("\tD: Days");
        Console.WriteLine("\tW: Weeks");
        Console.WriteLine("\tM: Months");
        Console.WriteLine("\tY: Years");
        Console.WriteLine("\n\tWhat would you like to do? ");
    }

    internal void SessionMenu()
    {
        Console.WriteLine($"{Bar}\t\tSESSION MENU\n{Bar}");
        Console.WriteLine("\tX: Return to Main Menu");
        Console.WriteLine("\tG: Get report");
        Console.WriteLine("\tF: Filter by date");
        Console.WriteLine("\tR: Filter by range");
        Console.WriteLine("\tD: Delete coding session");
        Console.WriteLine("\n\tWhat would you like to do? ");
    }

    internal void Success(string prompt)
    {
        Console.WriteLine($"{prompt} successfully");
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    internal void ContinuePrompt()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
