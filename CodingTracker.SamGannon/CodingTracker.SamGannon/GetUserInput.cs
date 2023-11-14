using System.Globalization;
using CodingTracker.SamGannon.Models;

namespace CodingTracker.SamGannon
{
    internal class GetUserInput
    {
        CodingController codingController = new();

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
                        CodingMenu();
                        break;
                    case "2":
                        runningMainMenu = false;
                        SleepMenu();
                        break;
                    default:
                        Console.WriteLine("Inalid command press any key and enter to continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private string CalculateSleepType(string duration)
        {
            TimeSpan sleepDuration = TimeSpan.ParseExact(duration, "h\\:mm", CultureInfo.InvariantCulture);

            if (sleepDuration.TotalHours > 4)
            {
                return "long";
            }
            else
            {
                return "Short";
            }
        }



        private string GetDurationInput()
        {
            Console.WriteLine("PLease enter the duration of your session in the following format: (hh:mm). Type 0 to return to main menu\n\n");

            string userDuration = Console.ReadLine();

            if (userDuration == "0") MainMenu();

            while (!TimeSpan.TryParseExact(userDuration, "h\\:mm", CultureInfo.InvariantCulture, out _))
            {
                Console.WriteLine("\n\nDuration invalid. Please insert the duration: (Format: hh:mm) or type 0 to return to the main menu\n\n");
                userDuration = Console.ReadLine();
                if (userDuration == "0") MainMenu();
            }

            return userDuration;
        }

        private string GetDateInput()
        {
            Console.WriteLine("Please enter the date in the following format: (dd-mm-yy). Type 0 to return to the main menu.");

            string userDateInput = Console.ReadLine();

            if (userDateInput == "0") MainMenu();

            while (!DateTime.TryParseExact(userDateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
                userDateInput = Console.ReadLine();
            }

            return userDateInput;
        }
    }
}