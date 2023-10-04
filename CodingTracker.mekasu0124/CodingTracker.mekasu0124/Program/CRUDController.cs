using CodingTracker.Models;
using CodingTracker.Services;
using System.Configuration;
using System.Diagnostics;
using ConsoleTableExt;

namespace CodingTracker.Program;

public class CRUDController
{
    private static readonly string dbFile = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    public static void NewSession()
    {
        string date = Helpers.GetFormattedDate();

        Console.Clear();
        Console.WriteLine("----------------------");
        Console.WriteLine("Creating A New Session");
        Console.WriteLine("----------------------");

        Helpers.PrintTimeSheet();

        Console.WriteLine("\nEnter Starting Time With 24-Hour Format HH:MM AM/PM");
        Console.Write("Your Entry: ");

        string? input = Console.ReadLine();
        string? startTime = UserValidation.IsValidTimeFormat(input);

        Console.WriteLine($"Start Date: {startTime}");
        Console.WriteLine("Press SPACEBAR To Stop Timer At Any Time");

        do
        {
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Console.WriteLine("Enter Ending Time With 24-Hour Format HH:MM");
                Console.Write("Your Entry: ");

                input = Console.ReadLine();
                string? endTime = UserValidation.IsValidTimeFormat(input);
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
                Console.WriteLine("Only The Spacebar Will Stop The Session.");
            }
        }
        while (!(Console.KeyAvailable));
    }

    public static void EditSession()
    {
        Console.Clear();

        List<CodeSession> allSessions = LogQueries.GetAllCodingSessions();
        var tableData = new List<List<object>>();

        foreach (CodeSession session in allSessions)
        {
            var item = new List<object>() { 
                session.Id, session.TodaysDate, session.StartTime, session.EndTime, session.Duration 
            };

            tableData.Add(item);
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Id", "Date Created", "Start Time", "End Time", "Duration")
            .ExportAndWrite();

        Console.WriteLine("\nTo Select An Entry To Edit, Enter It's ID Number");
        Console.Write("Your Selection: ");
        string? input = Console.ReadLine();
        int? selectedId = UserValidation.ValidateNumericInput(input);
        selectedId = UserValidation.ValidateIdSelection(selectedId, allSessions);

        CodeSession currentSession = LogQueries.GetCodeSession(selectedId);

        CodeSession updatedSession = StartSessionEdit(currentSession);
        
        LogQueries.UpdateData(updatedSession);
    }

    public static CodeSession StartSessionEdit(CodeSession currentSession)
    {
        Console.WriteLine("\nWhen Editing Information, If you want it left the same then press ENTER to skip.");
        Console.WriteLine($"\nCurrent Date Created: {currentSession.TodaysDate}");
        Console.Write("Enter Corrected Date: ");

        string? newDate = Console.ReadLine();
        newDate = UserValidation.VerifyEmptyOrChanged(currentSession.TodaysDate, newDate);

        Console.WriteLine($"\nCurrent Start Time: {currentSession.StartTime}");
        Console.Write("Enter Corrected Time: ");

        string? newStartTime = Console.ReadLine();
        newStartTime = UserValidation.VerifyEmptyOrChanged(currentSession.StartTime, newStartTime);

        Console.WriteLine($"\nCurrent End TIme: {currentSession.EndTime}");
        Console.Write("Enter Corrected Time: ");

        string? newEndTime = Console.ReadLine();
        newEndTime = UserValidation.VerifyEmptyOrChanged(currentSession.EndTime, newEndTime);

        string? newDuration = Helpers.CalculateDuration(newStartTime, newEndTime);

        CodeSession updatedSession = new()
        {
            Id = currentSession.Id,
            TodaysDate = newDate,
            StartTime = newStartTime,
            EndTime = newEndTime,
            Duration = newDuration
        };

        Console.WriteLine($"New Date: {newDate}");
        Console.WriteLine($"New Start Time: {newStartTime}");
        Console.WriteLine($"New End Time: {newEndTime}");
        Console.WriteLine($"New Duration: {newDuration}");
        Console.WriteLine("\n Are You Satisfied With These Changes? Y/N");
        Console.Write("Your Selection: ");

        string? satisfied = Console.ReadLine().ToLower();
        satisfied = UserValidation.ValidateYesNo(satisfied);

        if (satisfied == "y")
        {
            return updatedSession;
        }
        else
        {
            Console.WriteLine("Restarting Session Edit.");
            return StartSessionEdit(currentSession);
        }
    }

    public static void DeleteSession()
    {
        Console.Clear();

        List<CodeSession> allSessions = LogQueries.GetAllCodingSessions();
        var tableData = new List<List<object>>();

        foreach (CodeSession session in allSessions)
        {
            var item = new List<object>()
            {
                session.Id, session.TodaysDate, session.StartTime, session.EndTime, session.Duration
            };

            tableData.Add(item);
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithColumn("Id", "Date Created", "Start Time", "End Time", "Duration")
            .ExportAndWrite();

        Console.WriteLine("\nTo Select An Entry To Delete, Enter It's ID Number");
        Console.Write("Your Selection: ");

        string? input = Console.ReadLine();
        int? selectedId = UserValidation.ValidateNumericInput(input);
        selectedId = UserValidation.ValidateIdSelection(selectedId, allSessions);

        CodeSession currentSession = LogQueries.GetCodeSession(selectedId);



        Console.WriteLine("Are You Sure You Want To Delete This Entry? Y/N");
        Console.Write("Your Selection: ");

        string? selectedAction = UserValidation.ValidateYesNo(input);

        if (selectedAction == "y")
        {

            LogQueries.DeleteData(currentSession.Id);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Succesfully Deleted Saved Entry.");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press ENTER To Return To Main Menu");
            Console.ReadLine();
            Console.Clear();
            MainMenu.ShowMenu();
        }
        else
        {
            Console.WriteLine("You Entered No. Press ENTER To Go Back To Main Menu.");
            Console.ReadLine();
            Console.Clear();
            MainMenu.ShowMenu();
        }
    }
}
