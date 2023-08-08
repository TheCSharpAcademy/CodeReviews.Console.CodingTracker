using CodingTracker.MartinL_no.Controllers;

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

            switch (op.ToLower())
            {
                case "a":
                    AddCodingSession();
                    break;
                case "u":
                    UpdateCodingSession();
                    break;
                case "d":
                    DeleteCodingSession();
                    break;
                case "v":
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
            a - Add coding session
            u - Update coding session
            d - Delete coding session
            v - View records/reports
            0 - Exit program

            """);

        Console.WriteLine("---------------------------------");
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

            switch (op.ToLower())
            {
                case "a":
                    ShowAllSessions();
                    return;
            }
        }
    }

    private void ShowRecordsReportsMenu()
    {
        ShowHeader("View records/reports");

        Console.WriteLine("""
            Select an option:
            a - View all sessions

            """);

        Console.WriteLine("---------------------------------");
    }

    private void ShowAllSessions()
    {
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
