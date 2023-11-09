using System.Globalization;

namespace CodingTracker.SamGannon
{
    internal class GetUserInput
    {
        internal void MainMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("-----Main Menu-----");
                Console.WriteLine("What would you like to do? Press the corresponding number key:");
                Console.WriteLine("1. View All Records");
                Console.WriteLine("2 Add A Record");
                Console.WriteLine("3 Delete A Record");
                Console.WriteLine("4 Update A Record");
                Console.WriteLine("0 Close Application");

                var userCommand = Console.ReadLine();

                while (string.IsNullOrEmpty(userCommand))
                {
                    Console.WriteLine("\nInvalid Command type a number 0 to 4.\n");
                    userCommand = Console.ReadLine();
                }

                switch (userCommand)
                {
                    case "0":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        ViewAllRecords();
                        break;
                    case "2":
                        AddRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Commmand. Press any key and enter to continue");
                        Console.ReadLine();
                        break;
                }

            }
        }

        private void UpdateRecord()
        {
            throw new NotImplementedException();
        }

        private void DeleteRecord()
        {
            throw new NotImplementedException();
        }

        private void ViewAllRecords()
        {
            throw new NotImplementedException();
        }

        private void ProcessAdd()
        {
            GetDateInput();
        }

        private void AddRecord()
        {
            GetDateInput();
        }

        private string GetDateInput()
        {
            Console.WriteLine("Please enter the date in the following format: {mm-dd-yyyy}. Type 0 to return to the main menu.");

            string userDateInput = Console.ReadLine();

            if (userDateInput == "0") MainMenu();

            while (!DateTime.TryParse(userDateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.Writeline("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
                userDateInput = Console.ReadLine();
            }

            return userDateInput;
        }
    }
}