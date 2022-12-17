using System.Globalization;
using Spectre.Console;

namespace TrackingProgram;

public class Menus
{
    public static void Begin()
    {
        TrackingData.StartupDatabase();
        Console.WriteLine("-------------------------------\n");
        Console.WriteLine("Connecting to " + TrackingData.connectionString);
        Console.WriteLine("\n-----------------------------\n");
        Console.WriteLine("Welcome to Code Tracker! \n");
        MainMenu();
    }

    static void MainMenu()
    {
        bool apprun = true;
        while (apprun)
        {
            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("\t1. Add time entry manually");
            Console.WriteLine("\t2. Start stopwatch");
            Console.WriteLine("\t3. View/Edit past time entries");
            Console.WriteLine("(type the number you want to select, then press enter)");

            string mainMenuSelection = Console.ReadLine();
            switch (mainMenuSelection)
            {
                case ("0"):
                    apprun = false;
                    break;

                case ("1"):
                    ManualEntry();
                    break;

                case ("2"):
                    Stopwatch();
                    break;

                case ("3"):
                    PastEntries();
                    break;

                default:
                    break;
            }
        }
    }

    static void ManualEntry()
    {
        DateTime startDateTime;
        DateTime endDateTime;
        string entryLabel;

        Console.WriteLine("Label for the entry:");
        entryLabel = Console.ReadLine();
        Console.WriteLine($"\nWhat time did the coding start? {UserInput.format}");
        startDateTime = UserInput.GetDateInput();
        Console.WriteLine($"Start time confirmed: {startDateTime.ToString(UserInput.format)}.");
        Console.WriteLine($"What time did this end? {UserInput.format}");
        endDateTime = UserInput.GetDateInput();
        while (!Validation.DateTimeIsValid(startDateTime, endDateTime))
        {
            Console.WriteLine($"End time is not before start time. The end time you entered was: {endDateTime.ToString(UserInput.format)} and the start time you entered was: {startDateTime.ToString(UserInput.format)}.");
        }
        Console.WriteLine($"End time confirmed: {endDateTime.ToString(UserInput.format)}.");

        TrackingData.CodeEntry manualEntry = new TrackingData.CodeEntry
        {
            Id = 0,
            Label = entryLabel,
            StartDate = startDateTime,
            EndDate = endDateTime,
        };
        TrackingData.InsertCodeEntry(manualEntry);
    }

    static void Stopwatch()
    {
        string format = "dd/MM/yyyy HH:mm tt";
        string entryLabel;
        Console.WriteLine("Label for the entry:");
        entryLabel = Console.ReadLine();
        Console.WriteLine();

        DateTime startDateTime = DateTime.Now;
        DateTime endDateTime;

        Console.WriteLine($"Stopwatch started at: {startDateTime.ToString(format)}. Type 'stop' to stop.");
        while (Console.ReadLine() != "stop") { }
        endDateTime = DateTime.Now;
        Console.WriteLine("-----------------------------");
        Console.WriteLine($"Timer stopped at {endDateTime}");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Recording time...");

        TrackingData.CodeEntry stopwatchEntry = new TrackingData.CodeEntry
        {
            Id = 0,
            Label = entryLabel,
            StartDate = startDateTime,
            EndDate = endDateTime,
        };
        TrackingData.InsertCodeEntry(stopwatchEntry);
        Console.WriteLine($"Time recorded. You coded for {TrackingData.getDurationOfCodeEntry(stopwatchEntry).ToString("hh\\:mm\\:ss")}");
        Console.WriteLine("-------------------------------------------------------------------\n\n\n");
    }

    static void PastEntries()
    {
        Table table = ReportBuilder.AllEntryTable();
        AnsiConsole.Write(table);
        PastEntriesMenu();
    }

    static void PastEntriesMenu()
    {
        Console.WriteLine("\nWhat would you like to do next?");
        Console.WriteLine("\t0. Back to Main Menu");
        Console.WriteLine("\t1. Advanced Reports");
        Console.WriteLine("\t2. Edit an Entry");
        Console.WriteLine("\t3. Delete an Entry\n");
        string pastEntriesMenuSelection = Console.ReadLine();
        switch (pastEntriesMenuSelection)
        {
            case ("0"):
                break;
            case ("1"):
                ReportMenu();
                break;
            case ("2"):
                UpdateMenu();
                break;
            case ("3"):
                DeleteMenu();
                break;
            default:
                Console.WriteLine("Invalid Input. Try again.");
                PastEntriesMenu();
                break;
        }
    }

    static void UpdateMenu()
    {
        Console.WriteLine("\n\n Please enter the ID of the entry you want to edit.");
        string id = Console.ReadLine();
        Table table = ReportBuilder.SingleEntryTable(id);
        while (table == null)
        {
            Console.WriteLine("Invalid ID. Try again.");
            id = Console.ReadLine();
            table = ReportBuilder.SingleEntryTable(id);
        }
        Console.Clear();
        AnsiConsole.Write(table);

        DateTime startDateTime;
        DateTime endDateTime;
        string entryLabel;

        Console.WriteLine("New Label:");
        entryLabel = Console.ReadLine();

        Console.WriteLine($"\nNew Start Time ({UserInput.format}):");
        startDateTime = UserInput.GetDateInput();
        Console.WriteLine($"Start time confirmed: {startDateTime.ToString(UserInput.format)}.");

        Console.WriteLine($"New End Time ({UserInput.format}):");
        endDateTime = UserInput.GetDateInput();
        Console.WriteLine($"End time confirmed: {endDateTime.ToString(UserInput.format)}.");

        TrackingData.CodeEntry updatedEntry = new TrackingData.CodeEntry
        {
            Id = int.Parse(id),
            Label = entryLabel,
            StartDate = startDateTime,
            EndDate = endDateTime,
        };
        TrackingData.UpdateCodeEntry(updatedEntry);
        table = ReportBuilder.SingleEntryTable(id);
        AnsiConsole.Write(table);
    }

    static void DeleteMenu()
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("Look out! You're about to permanently delete a record!");
        Console.WriteLine("Enter the record ID you'd like to delete:");
        string id = Console.ReadLine();
        Table table = ReportBuilder.SingleEntryTable(id);
        while (table == null)
        {
            Console.WriteLine("Invalid ID. Try again.");
            id = Console.ReadLine();
            table = ReportBuilder.SingleEntryTable(id);
        }
        Console.Clear();
        AnsiConsole.Write(table);
        Console.WriteLine("Are you sure you want to delete this record? (y/n)");
        if (Console.ReadLine() == "y")
        {
            TrackingData.DeleteCodeEntry(id);
            Console.WriteLine("Record Deleted\n-----------------------------------------\n\n\n\n");
        }
        else { }

    }

    static void ReportMenu()
    {
        Console.WriteLine("\nWhat report do you want to see?");
        Console.WriteLine("\t0. Back to Main Menu");
        Console.WriteLine("\t1. Fun Statistics");
        Console.WriteLine("\t2. This week's stats\n");

        string pastEntriesMenuSelection = Console.ReadLine();
        switch (pastEntriesMenuSelection)
        {
            case ("0"):
                break;
            case ("1"):
                AnsiConsole.Write(ReportBuilder.FunStats());
                ReportMenu();
                break;
            case ("2"):
                AnsiConsole.Write(ReportBuilder.ThisWeek());
                ReportMenu();
                break;
            default:
                Console.WriteLine("Invalid Input. Try again.");
                ReportMenu();
                break;
        }
    }
}