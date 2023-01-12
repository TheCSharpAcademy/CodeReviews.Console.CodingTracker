using ConsoleTableExt;
using SinghxRaj.CodingTracker;

namespace SinghxRaj.CodingTracker;

internal class CodingTrackerController
{
    public static void Run()
    {
        bool isRunning = true;
        Display.PrintIntro();
        while (isRunning)
        {
            Display.PrintOptions();

            int option = UserInput.GetOption();
            isRunning = HandleOption(option);
        }
        Display.PrintOutro();
    }

    private static bool HandleOption(int option)
    {
        switch (option)
        {
            case 0:
                return false;
            case 1:
                AddNewCodingSession();
                break;
            case 2:
                ShowAllCodingSessions();
                break;
            default:
                Console.WriteLine($"{option} is not a valid option.");
                break;
        }
        return true;
    }

    private static void ShowAllCodingSessions()
    {
        var sessions = DatabaseManager.GetCodingSessions();

        var convertedList = sessions.Select(s => new List<object>
        {
            s.Id,
            s.StartTime,
            s.EndTime,
            s.Duration
        }).ToList();

        ConsoleTableBuilder.From(convertedList).ExportAndWriteLine();

    }

    private static void AddNewCodingSession()
    {
        CodingSession session = UserInput.NewCodingSessionInfo();

        bool success = DatabaseManager.AddNewCodingSession(session);

        if (success)
        {
            Console.WriteLine("New coding session was added.");
        } else
        {
            Console.WriteLine("New coding session could not be added.");
        }
    }
}
