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
                case "h":
                    break;
                case "a":
                    break;
                case "u":
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

    private static void ShowHeader(string title)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine("---------------------------------");
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
