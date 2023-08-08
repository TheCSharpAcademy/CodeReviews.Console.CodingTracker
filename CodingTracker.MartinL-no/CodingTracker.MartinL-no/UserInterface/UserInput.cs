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
                    AddCodingTime();
                    break;
                case "u":
                    UpdateCodingTime();
                    break;
                case "d":
                    break;
                case "v":
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
            a - Add coding time
            u - Update coding time
            d - Delete coding time
            v - View records
            0 - Exit program

            """);

        Console.WriteLine("---------------------------------");
    }

    private void AddCodingTime()
    {
        while (true)
        {
            ShowHeader("Add coding time");

            var startTime = Ask("When did you start coding (input must be in format - 2023-01-30 21:34)");
            var endTime = Ask("When did you end coding (input must be in format - 2023-01-30 21:34)");

            var isAdded = _controller.InsertCodingSession(startTime, endTime);

            if (isAdded)
            {
                ShowMessage("Coding time added!");
                break;
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void UpdateCodingTime()
    {
        ShowHeader("Update coding time");
        ShowAllSessions();

        while (true)
        {
            var id = Ask("Enter the id of the sessions you would like to update: ");
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
