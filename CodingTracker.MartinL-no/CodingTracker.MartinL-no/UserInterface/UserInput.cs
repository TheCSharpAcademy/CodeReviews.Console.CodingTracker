using CodingTracker.MartinL_no.Controllers;
using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly CodingController _controller;

    public UserInput(CodingController controller)
	{
        _controller = controller;
    }

    public void Execute()
    {
        while (true)
        {
            ShowMainMenuOptions();
            var op = Ask("Your choice: ");

            switch (op.ToUpper())
            {
                case "S":
                    StartCodingSession();
                    break;
                case "A":
                    AddCodingSession();
                    break;
                case "U":
                    UpdateCodingSession();
                    break;
                case "D":
                    DeleteCodingSession();
                    break;
                case "V":
                    RecordsReports();
                    break;
                case "0":
                    ShowMessage("Program ended");
                    return;
                default:
                    ShowMessage("Invalid option, please try again");
                    break;
            }
        }
    }

    private void ShowMainMenuOptions()
    {
        ShowHeader("Welcome to the Coding Tracker app!");

        Console.WriteLine("""
            Select an option:
                S - Start coding session
                A - Add coding session
                U - Update coding session
                D - Delete coding session
                V - View records/reports
                0 - Exit program

            """);
        
        Console.WriteLine("---------------------------------");
    }

    private void StartCodingSession()
    {
        var startTime = _controller.StartSession();
        var currentTime = startTime;

        while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
        {
            currentTime = currentTime.AddSeconds(1);

            ShowHeader("Recording Coding Session");
            Console.WriteLine($"Start time: {startTime.ToString("HH:mm:ss").PadLeft(12)}\n");
            Console.Write($"Current time: ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{currentTime.ToString("HH:mm:ss").PadLeft(10)}\n");
            Console.ResetColor();

            Console.WriteLine("Press ESC to end session");
            Thread.Sleep(1000);
        }

        _controller.InsertCodingSession(startTime.ToString(), currentTime.ToString());

        ShowMessage("Coding session added!");
    }

    private void AddCodingSession()
    {
        while (true)
        {
            ShowHeader("Add coding session");

            var startTime = Ask("When did you start coding (input must be in format - 2023-01-30 21:34)");
            var endTime = Ask("When did you end coding (input must be in format - 2023-01-30 21:34)");

            var isAdded = _controller.InsertCodingSession(startTime, endTime);

            if (isAdded)
            {
                ShowMessage("Coding session added!");
                break;
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void UpdateCodingSession()
    {
        while (true)
        {
            ShowHeader("Update coding session");
            ShowAllSessions();

            var id = Ask("Enter the id of the session you would like to update: ");
            var startTime = Ask("Enter the new start time (input must be in format - 2023-01-30 21:34): ");
            var endTime = Ask("Enter the new start time (input must be in format - 2023-01-30 21:34): ");
            var intId = 0;

            if (Int32.TryParse(id, out intId) && _controller.UpdateCodingSession(intId, startTime, endTime))
            {
                ShowMessage("Session updated!");
                break;
            }

            else ShowMessage("Invalid input please try again");
        }
    }

    private void DeleteCodingSession()
    {
        while (true)
        {
            ShowHeader("Delete coding session");
            ShowAllSessions();

            var id = Ask("Enter the Id of the session you would like to delete: ");
            var intId = 0;

            if (Int32.TryParse(id, out intId) && _controller.DeleteCodingSession(intId))
            {
                ShowMessage("Session deleted!");
                break;
            }

            else ShowMessage("Incorrect Id please try again");
        }
    }

    private void RecordsReports()
    {
        while (true)
        {
            ShowRecordsReportsMenu();

            var op = Ask("Your choice: ");

            switch (op.ToUpper())
            {
                case "A":
                    ShowAllSessions();
                    return;
                case "D":
                    PreviousDaysMenu();
                    break;
                case "M":
                    PreviousMonthsMenu();
                    break;
                case "Y":
                    PreviousYearsMenu();
                    break;
            }
        }
    }

    private void ShowRecordsReportsMenu()
    {
        ShowHeader("View records/reports");

        Console.WriteLine("""
            Select an option:
                A - View all sessions
                D - View by previous amount of days
                M - View by previous amount of months
                Y - View by previous amount of years
            
            """);

        Console.WriteLine("---------------------------------");
    }

    private void ShowAllSessions()
    {
    }

    private void PreviousDaysMenu()
    {
        while (true)
        {
            ShowHeader("View records/reports");

            var daysString = Ask("How many previous days would you like to see reports for: ");
            var order = Ask("View starting from newest date (y/n)? ");
            var days = 0;

            if (Int32.TryParse(daysString, out days))
            {
                var sessions = _controller.GetCodingSessionsByDays(days);

                if (order == "y") sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                else sessions = sessions.OrderBy(s => s.StartTime).ToList();

                ShowSessions(sessions);
                break;
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void PreviousMonthsMenu()
    {
        while (true)
        {
            ShowHeader("View records/reports");
            var monthsString = Ask("How many previous months would you like to see reports for: ");
            var order = Ask("View starting from newest date (y/n)? ");

            var months = 0;

            if (Int32.TryParse(monthsString, out months))
            {
                var sessions = _controller.GetCodingSessionsByYears(months);

                if (order == "y") sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                else sessions = sessions.OrderBy(s => s.StartTime).ToList();

                ShowSessions(sessions);
                break;
            }
            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void PreviousYearsMenu()
    {
        while (true)
        {
            ShowHeader("View records/reports");
            var yearsString = Ask("How many previous years would you like to see reports for: ");
            var order = Ask("View starting from newest date (y/n)? ");

            var years = 0;

            if (Int32.TryParse(yearsString, out years))
            {
                var sessions = _controller.GetCodingSessionsByYears(years);

                if (order == "y") sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                else sessions = sessions.OrderBy(s => s.StartTime).ToList();

                ShowSessions(sessions);
                break;
            }
            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void ShowSessions(List<CodingSession> sessions)
    {
        throw new NotImplementedException();
    }

    private static void ShowHeader(string title)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine("---------------------------------\n");
    }

    private void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Thread.Sleep(2500);
    }

    private string Ask(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}
