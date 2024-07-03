using CodingTracker.Controllers;
using CodingTracker.Models;
using Services;
using Spectre.Console;

public class Program
{
    private static readonly CodingController codingController = new CodingController("Data Source=CodingSessionDB.db");
    public static void Main(string[] args)
    {
        var keepRunning = true;
        while (keepRunning)
        {
            // Display a menu using Spectre.Console
            Services.DisplayInfo.WelcomeMessage();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Enter new record (CodingSession)",
                        "View previous records",
                        "Edit a record",
                        "Delete a record",
                        "About",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Enter new record (CodingSession)":
                    // Call method to enter a new coding session
                    EnterNewCodingSession();
                    break;
                case "View previous records":
                    // Call method to view previous records
                    ViewPreviousRecords();
                    break;
                case "Edit a record":
                    // Call method to edit a record
                    break;
                case "Delete a record":
                    // Call method to delete a record
                    break;
                case "About":
                    Services.DisplayInfo.About();
                    break;
                case "Exit":
                    ExitProgram();
                    break;
            }
            Console.Clear();
        }

    }

    private static void EnterNewCodingSession()
    {
        var startTime = UserInput.GetDateTimeFromUser("Enter the start time (yyyy-MM-dd HH:mm):");
        var endTime = UserInput.GetDateTimeFromUser("Enter the end time (yyyy-MM-dd HH:mm):");
        // check if the end time is after the start time
        if (endTime <= startTime)
        {
            AnsiConsole.MarkupLine("[red]End time must be after the start time.[/]");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(true);
            return;
        }
        codingController.AddSession(new CodingSession { StartTime = startTime, EndTime = endTime });
        AnsiConsole.MarkupLine("[green]Session added successfully.[/]");
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey(true);
    }
    private static void ExitProgram()
    {
        var exit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Are you sure you want to exit?")
                .PageSize(10)
                .AddChoices(new[] {
                    "Yes",
                    "No"
                }));

        if (exit == "Yes")
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }

    private static void ViewPreviousRecords()
    {
        var records = codingController.GetSessions(); // Retrieve the previous coding sessions from the controller
        if (records.Count == 0)
        {
            Console.WriteLine("No previous records found.");
        }
        else
        {
            Console.WriteLine("Previous Records:");
            foreach (var record in records)
            {
                Console.WriteLine($"Start Time: {record.StartTime}, End Time: {record.EndTime}, Duration: {record.Duration}");
            }
        }
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey(true);
    }

    private static void EditRecord()
    {
        // Implementation depends on how you want to identify and edit records
        // This could involve selecting a session by ID, then updating its details
    }

    private static void DeleteRecord()
    {
        // Similar to EditRecord, you would typically prompt the user to select a record by ID, then delete it
    }
}
