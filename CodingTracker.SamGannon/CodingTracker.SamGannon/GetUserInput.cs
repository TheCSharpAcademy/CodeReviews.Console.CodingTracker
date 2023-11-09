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
                        break;
                }
            }
        }

        private void SleepMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("-----Sleep Menu-----");
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
                        codingController.GetSleepData();
                        break;
                    case "2":
                        ProcessSleepAdd();
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

        internal void CodingMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("-----Coding Menu-----");
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
                        codingController.GetCodingData();
                        break;
                    case "2":
                        ProcessAdd();
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
            codingController.GetCodingData();

            Console.WriteLine("Please add id of the record you want to update (or 0 to return to the Main Menu).");
            string commandInput = Console.ReadLine();

            while (!Int32.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\nYou have to type an Id (or 0 to return to the Main Menu).\n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) CodingMenu();

            var coding = codingController.GetById(id);

            while (coding.Id == 0)
            {
                Console.WriteLine($"\nRecord with Id {id} doesn't exist\n");
                UpdateRecord();
            }

            var updateInput = "";

            bool updating = true;
            while (updating == true)
            {
                Console.WriteLine($"\nType 'd' for Date \n");
                Console.WriteLine($"\nType 't' for Time \n");
                Console.WriteLine($"\nType 's' to save update \n");
                Console.WriteLine($"\n Type '0' to go back to the Main Menu");

                updateInput = Console.ReadLine();

                switch (updateInput)
                {
                    case "d":
                        coding.Date = GetDateInput();
                        break;
                    case "t":
                        coding.Duration = GetDurationInput();
                        break;
                    case "0":
                        CodingMenu();
                        break;
                    case "s":
                        updating = false;
                        break;
                    default:
                        Console.WriteLine($"\nType '0' to go back to the Main Menu");
                        break;

                }
            }
            codingController.Update(coding);
            CodingMenu();
        }

        private void DeleteRecord()
        {
            codingController.GetCodingData();
            Console.WriteLine("Please add id of the category you want to delete (or press 0 to reutrn to Main Menu).");

            string commandInput = Console.ReadLine();

            while (!int.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\n You have to type a valid Id (or 0 to return to the Main Menu). \n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) CodingMenu();

            var coding = codingController.GetById(id);

            while (coding.Id == 0)
            {
                Console.WriteLine($"\nRecord with id {id} doesn't exist\n");
                DeleteRecord();
            }

            codingController.Delete(id);

        }

        private void ProcessSleepAdd()
        {
            var duration = GetDurationInput();
            var sleepType = CalculateSleepType(duration);

            Sleep sleep = new();

            sleep.Duration = duration;
            sleep.SleepType = sleepType;
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


        private void ProcessAdd()
        {
            var date = GetDateInput();
            var duration = GetDurationInput();

            Coding coding = new();

            coding.Date = date;
            coding.Duration = duration;

            codingController.Post(coding);
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

        private void AddRecord()
        {
            GetDateInput();
        }

        private string GetDateInput()
        {
            Console.WriteLine("Please enter the date in the following format: {mm-dd-yyyy}. Type 0 to return to the main menu.");

            string userDateInput = Console.ReadLine();

            if (userDateInput == "0") CodingMenu();

            while (!DateTime.TryParseExact(userDateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
                userDateInput = Console.ReadLine();
            }

            return userDateInput;
        }
    }
}