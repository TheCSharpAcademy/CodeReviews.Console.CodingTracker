﻿using Spectre.Console;
using System.Globalization;
namespace TrackingProgram;

// This class handles all user interface elements and input validation.
class Program
{
    // Configuration manager, as required. Just for connection string at the moment.
    static void Main(string[] args)
    {
        TrackingData.StartupDatabase();
        // Pretend like it takes a long time to connect to a local database, lol.
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
        // Initialisation stuff for datetime parsing.
        string format = "dd/MM/yyyy HH:mm tt";
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-AU");

        // Initialise these variables to have them in the scope of the method, not the loops below... made that mistake.
        DateTime startDateTime;
        DateTime endDateTime;

        // Avoiding nulls, probably pointless but not sure.
        string enteredStartTime = DateTime.Now.ToString(format);
        string enteredEndTime = DateTime.Now.ToString(format);
        string entryLabel = "Default Label";

        // Get time entry label from user.
        Console.WriteLine("Label for the entry:");
        entryLabel = Console.ReadLine();

        // Get and parse start time.
        Console.WriteLine($"\nWhat time did the coding start? {format}");
        enteredStartTime = Console.ReadLine();
        while (!DateTime.TryParse(enteredStartTime, culture, DateTimeStyles.None, out startDateTime))
        {
            Console.WriteLine("Invalid date/time format. Please try again.");
            enteredStartTime = Console.ReadLine();
        }
        Console.WriteLine($"Start time confirmed: {startDateTime.ToString(format)}.");


        // Get and parse end time.
        Console.WriteLine($"What time did this end? {format}");
        enteredEndTime = Console.ReadLine();
        while (!DateTime.TryParse(enteredEndTime, culture, DateTimeStyles.None, out endDateTime))
        {
            Console.WriteLine("Invalid date/time format. Please try again.");
            enteredEndTime = Console.ReadLine();
        }
        Console.WriteLine($"End time confirmed: {endDateTime.ToString(format)}.");

        TrackingData.CodeEntry manualEntry = new TrackingData.CodeEntry
        {
            Id = 0, // I think this could be null or undefined but not sure how to make it accept that. It's not used.
            Label = entryLabel,
            StartDate = startDateTime,
            EndDate = endDateTime,
        };

        TrackingData.InsertCodeEntry(manualEntry);
    }

    static void Stopwatch()
    {
        // Initialisation stuff for datetime parsing.
        string format = "dd/MM/yyyy HH:mm tt";
        string entryLabel = "Default Label";
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-AU");

        // Get the label.
        Console.WriteLine("Label for the entry:");
        entryLabel = Console.ReadLine();
        Console.WriteLine();

        // Initialise these variables to have them in the scope of the method, not the loops below.
        DateTime startDateTime = DateTime.Now;
        DateTime endDateTime;

        Console.WriteLine($"Stopwatch started at: {startDateTime.ToString(format)}. Type 'stop' to stop.");
        while (Console.ReadLine() != "stop") { }

        endDateTime = DateTime.Now;

        Console.WriteLine("-----------------------------");
        Console.WriteLine($"Timer stopped at {endDateTime}");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Recording time...");

        // Make a CodeEntry object.
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
        // Past Entries Presentation
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

        // Initialisation stuff for datetime parsing.
        string format = "dd/MM/yyyy HH:mm tt";
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-AU");

        // Initialise these variables to have them in the scope of the method, not the loops below... made that mistake.
        DateTime startDateTime;
        DateTime endDateTime;

        // Avoiding nulls, probably pointless but not sure.
        string enteredStartTime = DateTime.Now.ToString(format);
        string enteredEndTime = DateTime.Now.ToString(format);
        string entryLabel = "Default Label";

        // Get time entry label from user.
        Console.WriteLine("New Label:");
        entryLabel = Console.ReadLine();

        // Get and parse start time.
        Console.WriteLine($"\nNew Start Time ({format}):");
        enteredStartTime = Console.ReadLine();
        while (!DateTime.TryParse(enteredStartTime, culture, DateTimeStyles.None, out startDateTime))
        {
            Console.WriteLine("Invalid date/time format. Please try again.");
            enteredStartTime = Console.ReadLine();
        }
        Console.WriteLine($"Start time confirmed: {startDateTime.ToString(format)}.");


        // Get and parse end time.
        Console.WriteLine($"New End Time ({format}):");
        enteredEndTime = Console.ReadLine();
        while (!DateTime.TryParse(enteredEndTime, culture, DateTimeStyles.None, out endDateTime))
        {
            Console.WriteLine("Invalid date/time format. Please try again.");
            enteredEndTime = Console.ReadLine();
        }
        Console.WriteLine($"End time confirmed: {endDateTime.ToString(format)}.");

        TrackingData.CodeEntry updatedEntry = new TrackingData.CodeEntry
        {
            Id = int.Parse(id), // I think this is okay
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
        } else { }

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
