using CodingTracker.SamGannon.Models;

namespace CodingTracker.SamGannon;

internal class GetUserInput
{
    CodingController codingController = new();
    CodingMenu codingMenu = new();
    SleepMenu sleepMenu = new();

    internal void MainMenu()
    {
        bool runningMainMenu = true;
        while (runningMainMenu)
        {
            Console.Clear();
            Console.WriteLine("-----Main Menu-----");
            Console.WriteLine("Which habit would you like to track?");
            Console.WriteLine("Press 0 to exit the application");
            Console.WriteLine("Press 1 to see the coding menu");
            Console.WriteLine("Press 2 to see the sleep menu");

            string commandInput = Console.ReadLine();

            while (string.IsNullOrEmpty(commandInput))
            {
                Console.WriteLine("Invalid Command type a number 0 to 2");
            }

            switch (commandInput)
            {
                case "0":
                    runningMainMenu = false;
                    break;
                case "1":
                    runningMainMenu = false;
                    codingMenu.ShowCodingMenu();
                    break;
                case "2":
                    runningMainMenu = false;
                    sleepMenu.ShowSleepMenu();
                    break;
                default:
                    Console.WriteLine("Inalid command press any key and enter to continue.");
                    Console.ReadLine();
                    break;
            }
        }
    }
}