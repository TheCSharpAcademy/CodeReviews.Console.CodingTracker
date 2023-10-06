using CodingTracker.Services;
using CodingTracker.Models;

namespace CodingTracker.Program;

public class CrudSessionController
{
    private static readonly List<string?>? sessionColHeaders = new() { "Id", "Date Created", "Start Time", "End Time", "Duration" };

    public static void NewSessionManual()
    {
        string date = Helpers.GetDate(true);

        Console.Clear();
        Console.WriteLine("----------------------");
        Console.WriteLine("Creating A New Session");
        Console.WriteLine("----------------------");

        Helpers.PrintTimeSheet();

        Console.WriteLine("\nEnter Starting Time With 24-Hour Format HH:MM AM/PM");
        Console.Write("\nYour Entry: ");

        string? input = Console.ReadLine();
        string? startTime = UserValidation.VerifyTimeInput(input);

        Console.WriteLine($"Start Date: {startTime}");
        Console.WriteLine("\nPress SPACEBAR To Stop Timer At Any Time");

        do
        {
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Console.WriteLine("\nEnter Ending Time With 24-Hour Format HH:MM");
                Console.Write("\nYour Entry: ");

                input = Console.ReadLine();
                string? endTime = UserValidation.VerifyTimeInput(input);
                string? duration = Helpers.CalculateDuration(startTime, endTime);

                CodeSession newSession = new()
                {
                    TodaysDate = date,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = duration
                };

                LogQueries.InsertData(newSession);
            }
            else if (Console.ReadKey().Key != ConsoleKey.Spacebar)
            {
                Console.WriteLine("\nOnly The Spacebar Will Stop The Session.");
            }
        }
        while (!(Console.KeyAvailable));
    }

    public static void NewSessionStopWatch()
    {
        string? date = Helpers.GetDate(false);
        string? today = Helpers.GetDate(true);

        Console.Clear();
        Console.WriteLine("-----------------------");
        Console.WriteLine("Recording A New Session");
        Console.WriteLine("-----------------------");

        Console.WriteLine($"\nTimer Started On: {date}");
        string? startTime = Helpers.GetTime();
        Console.WriteLine("\nPress SPACEBAR To Stop Timer At Any Time");

        do
        {
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Console.WriteLine($"\nTimer Stopped On: {date}");
                string? endTime = Helpers.GetTime();

                string? duration = Helpers.CalculateDuration(startTime, endTime);
                Console.WriteLine($"Session Duration: {duration}");

                CodeSession newSession = new()
                {
                    TodaysDate = today,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = duration
                };

                LogQueries.InsertData(newSession);
            }
            else if (Console.ReadKey().Key != ConsoleKey.Spacebar)
            {
                Console.WriteLine("\nOnly The Spacebar Will Stop The Session");
            }
        }
        while (!(Console.KeyAvailable));
    }

    public static void EditSession()
    {

        Console.Clear();
        Console.WriteLine("-----------------");
        Console.WriteLine("Editing A Session");
        Console.WriteLine("-----------------");

        List<CodeSession?>? allSessions = LogQueries.GetAllCodingSessions();
        List<List<object?>?>? tableData = new List<List<object?>?>();

        foreach (CodeSession? session in allSessions)
        {
            List<object?>? item = new List<object?>() { 
                session.Id, session.TodaysDate, session.StartTime, session.EndTime, session.Duration 
            };

            tableData.Add(item);
        }

        TableVisualizationEngine.ShowTable(tableData, sessionColHeaders, "All Coding Sessions");

        Console.WriteLine("\nTo Select An Entry To Edit, Enter It's ID Number");
        Console.Write("Your Selection: ");
        string? input = Console.ReadLine();
        int? selectedId = UserValidation.ValidateNumericInput(input);
        selectedId = UserValidation.ValidateIdSelection(selectedId, allSessions, null);

        CodeSession currentSession = LogQueries.GetCodeSession(selectedId);
        CodeSession updatedSession = StartSessionEdit(currentSession);
        
        LogQueries.UpdateData(updatedSession);
    }

    public static CodeSession? StartSessionEdit(CodeSession? currentSession)
    {
        Console.WriteLine("\nWhen Editing Information, If you want it left the same then press ENTER to skip.");
        Console.WriteLine($"\nCurrent Date Created: {currentSession.TodaysDate}");
        Console.Write("Enter Corrected Date: ");

        string? input = Console.ReadLine();
        bool dateChanged = UserValidation.VerifyEmptyOrChanged(currentSession.TodaysDate, input);
        string? newDate = dateChanged 
            ? UserValidation.VerifyDateInput(input)
            : currentSession.TodaysDate;

        Console.WriteLine($"\nCurrent Start Time: {currentSession.StartTime}");
        Console.Write("Enter Corrected Time: ");

        input = Console.ReadLine();
        bool startTimeChanged = UserValidation.VerifyEmptyOrChanged(currentSession.StartTime, input);
        string? newStartTime = startTimeChanged
            ? UserValidation.VerifyTimeInput(input)
            : currentSession.StartTime;

        Console.WriteLine($"\nCurrent End TIme: {currentSession.EndTime}");
        Console.Write("Enter Corrected Time: ");

        input = Console.ReadLine();
        bool endTimeChanged = UserValidation.VerifyEmptyOrChanged(currentSession.StartTime, input);
        string? newEndTime = endTimeChanged
            ? UserValidation.VerifyTimeInput(input)
            : currentSession.EndTime;

        string? newDuration = Helpers.CalculateDuration(newStartTime, newEndTime);

        CodeSession? updatedSession = new()
        {
            Id = currentSession.Id,
            TodaysDate = newDate,
            StartTime = newStartTime,
            EndTime = newEndTime,
            Duration = newDuration
        };

        Console.WriteLine("------------------------------------");
        Console.WriteLine($"New Date: {newDate}");
        Console.WriteLine($"New Start Time: {newStartTime}");
        Console.WriteLine($"New End Time: {newEndTime}");
        Console.WriteLine($"New Duration: {newDuration}");
        Console.WriteLine("------------------------------------");

        bool satisfied = UserValidation.ValidateYesNo("\nAre You Satisfied With These Changes? Y/N");

        return satisfied ? updatedSession : StartSessionEdit(currentSession);
    }

    public static void DeleteSession()
    {
        Console.Clear();
        Console.WriteLine("------------------");
        Console.WriteLine("Deleting A Session");
        Console.WriteLine("------------------");

        List<CodeSession?>? allSessions = LogQueries.GetAllCodingSessions();
        List<List<object?>?>? tableData = new List<List<object?>?>();

        foreach (CodeSession session in allSessions)
        {
            List<object?>? item = new List<object?>()
            {
                session.Id, session.TodaysDate, session.StartTime, session.EndTime, session.Duration
            };

            tableData.Add(item);
        }

        TableVisualizationEngine.ShowTable(tableData, sessionColHeaders, "All Coding Sessions");

        Console.WriteLine("\nTo Select An Entry To Delete, Enter It's ID Number");
        Console.Write("Your Selection: ");

        string? input = Console.ReadLine();
        int? selectedId = UserValidation.ValidateIdSelection(UserValidation.ValidateNumericInput(input), allSessions, null);

        CodeSession? currentSession = LogQueries.GetCodeSession(selectedId);

        bool selectedAction = UserValidation.ValidateYesNo("\nAre You Sure You Want To Delete This Entry? Y/N");

        if (selectedAction)
        {
            LogQueries.DeleteData(currentSession);
        }
        else
        {
            Console.WriteLine("You Entered No. Press ENTER To Go Back To Main Menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu.ShowMenu();
        }
    }

    public static void ViewAllCodingSessions()
    {
        Console.Clear();

        List<CodeSession?>? allSessions = LogQueries.GetAllCodingSessions();
        List<List<object?>?>? tableData = new List<List<object?>?>();

        double aTime = Helpers.CalculateAverageSession(allSessions);
        double tTime = Helpers.GetTotalSessionTime(allSessions);

        TimeSpan avgTime = TimeSpan.FromSeconds(aTime);
        TimeSpan ttlTime = TimeSpan.FromSeconds(tTime);

        string averageTime = avgTime.ToString(@"hh\:mm\:ss");
        string totalTime = ttlTime.ToString(@"hh\:mm\:ss");

        foreach (CodeSession? session in allSessions)
        {
            List<object?>? item = new List<object?>()
            {
                session.Id, session.TodaysDate, session.StartTime, session.EndTime, session.Duration
            };

            tableData.Add(item);
        }

        TableVisualizationEngine.ShowTable(tableData, sessionColHeaders, "All Coding Sessions");

        Console.WriteLine("\nAverage Coding Time: {0}", averageTime);
        Console.WriteLine("Total Coding Time: {0}\n", totalTime);

        MainMenu.ShowMenu();
    }
}
