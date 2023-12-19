using ConsoleTableExt;
using Microsoft.VisualBasic.FileIO;

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
            case (int)Option.ExitApplication:
                return false;
            case (int)Option.AddNewCodingSession:
                AddNewCodingSession();
                break;
            case (int)Option.ShowAllCodingSessions:
                ShowAllCodingSessions();
                break;
            case (int)Option.DeleteCodingSession:
                DeleteCodingSession();
                break;
            case (int)Option.UpdateCodingSession:
                UpdateCodingSession();
                break;
            default:
                Console.WriteLine($"{option} is not a valid option.");
                break;
        }
        return true;
    }

    private static void UpdateCodingSession()
    {
        int codingSessionId = UserInput.GetId();

        var codingSession = UserInput.GetUpdatedCodingSessionInfo(codingSessionId);

        if (codingSession == null)
        {
            Console.WriteLine($"Session does not exist with id {codingSessionId}");
            return;
        }

        bool successfullyUpdated = DatabaseManager.UpdateCodingSession(codingSession);

        if (successfullyUpdated)
        {
            Console.WriteLine($"Coding Session with id {codingSession.Id} was successfully updated.");
        } else
        {
            Console.WriteLine($"Coding Session with id {codingSession.Id} was not able to be updated");
        }
    }

    private static void DeleteCodingSession()
    {
        int codingSessionId = UserInput.GetId();
        bool successfullyDeleted = DatabaseManager.DeleteCodingSession(codingSessionId);

        if (successfullyDeleted)
        {
            Console.WriteLine($"Coding Session with id {codingSessionId} was successfully deleted.");
        } else
        {
            Console.WriteLine($"Coding Session with id {codingSessionId} does not exist.");
        }
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
