using ConsoleTableExt;

namespace SinghxRaj.CodingTracker;

internal class CodingTrackerController
{
    public static void Run()
    {
        HandleStartUp();
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

    private static void HandleStartUp()
    {
        DatabaseManager.CreateTable();
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

        var convertedList = sessions.Select(session => new List<object>
        {
            session.Id!,
            session.StartTime.ToString(TimeFormat.SessionTimeStampFormat),
            session.EndTime.ToString(TimeFormat.SessionTimeStampFormat),
            session.Duration
        }).ToList();

        if (convertedList.Count == 0)
        {
            Console.WriteLine("No coding session entries.");
        } else
        {
            ConsoleTableBuilder.From(convertedList).ExportAndWriteLine();
        }

    }

    private static void AddNewCodingSession()
    {
        CodingSession session = UserInput.GetCodingSessionInfo();

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
