using CodingTracker.Controllers;
using CodingTracker.Models;
using Services;
using Spectre.Console;

namespace CodingTracker
{
    public class Program
    {
        private static readonly CodingController codingController = new("Data Source=CodingSessionDB.db");
        public static void Main(string[] args)
        {
            var keepRunning = true;
            while (keepRunning)
            {
                // Display a menu using Spectre.Console
                DisplayInfo.WelcomeMessage();
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What would you like to do?")
                        .PageSize(10)
                        .AddChoices([
                        "Enter new record (CodingSession)",
                        "View previous records",
                        "Edit a record",
                        "Delete a record",
                        "About",
                        "Exit"
                        ]));

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
                        EditRecord();
                        break;
                    case "Delete a record":
                        // Call method to delete a record
                        DeleteRecord();
                        break;
                    case "About":
                        DisplayInfo.About();
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
            var startTime = UserInput.GetDateTimeFromUser("Enter the start time:");
            var endTime = UserInput.GetDateTimeFromUser("Enter the end time:");
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
                    .AddChoices([
                    "Yes",
                    "No"
                    ]));

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
                    Console.WriteLine($"Record no.{record.Id}| Start Time: {record.StartTime}, End Time: {record.EndTime}, Duration: {record.Duration}");
                }
            }
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(true);
        }

        private static void EditRecord()
        {
            var sessions = codingController.GetSessions(); // Retrieve the previous coding sessions from the controller
            if (sessions.Count == 0)
            {
                Console.WriteLine("No previous records found.");
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Select a session to edit:");
            for (int i = 0; i < sessions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Start Time: {sessions[i].StartTime}, End Time: {sessions[i].EndTime}, Duration: {sessions[i].Duration}");
            }

            int selection;
            while (true)
            {
                Console.Write("Enter the session id: ");
                if (int.TryParse(Console.ReadLine(), out selection) && selection >= 1 && selection <= sessions.Count)
                {
                    break;
                }
                Console.WriteLine("You don't have that session in the database!");
            }

            var selectedSession = sessions[selection - 1];

            Console.WriteLine($"Selected session: Start Time: {selectedSession.StartTime}, End Time: {selectedSession.EndTime}, Duration: {selectedSession.Duration}");

            var newStartTime = UserInput.GetDateTimeFromUser("Enter the new start time:");
            var newEndTime = UserInput.GetDateTimeFromUser("Enter the new end time:");

            if (newEndTime <= newStartTime)
            {
                AnsiConsole.MarkupLine("[red]End time must be after the start time.[/]");
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey(true);
                return;
            }

            selectedSession.StartTime = newStartTime;
            selectedSession.EndTime = newEndTime;

            codingController.UpdateSession(selectedSession);

            AnsiConsole.MarkupLine("[green]Session updated successfully.[/]");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(true);
        }

        private static void DeleteRecord()
        {
            // Similar to EditRecord, you would typically prompt the user to select a record by ID, then delete it
            var sessions = codingController.GetSessions(); // Retrieve the previous coding sessions from the controller
            if (sessions.Count == 0)
            {
                Console.WriteLine("No previous records found.");
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey(true);
                return;
            }
            foreach (var session in sessions)
            {
                Console.WriteLine($"ID: {session.Id}, Start Time: {session.StartTime}, End Time: {session.EndTime}, Duration: {session.Duration}");
            }
            int id;
            while (true)
            {
                Console.Write("Enter the ID of the session to delete: ");
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please try again.");
            }
            codingController.DeleteSession(id);
            AnsiConsole.MarkupLine("[green]Session deleted successfully.[/]");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(true);
        }
    }
}