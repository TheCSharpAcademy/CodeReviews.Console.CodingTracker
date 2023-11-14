using CodingTracker.SamGannon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.SamGannon
{
    internal class SleepMenu
    {
        internal void ShowSleepMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("-----Sleep Menu-----");
                Console.WriteLine("What would you like to do? Press the corresponding number key:");
                Console.WriteLine("1 - View All Records");
                Console.WriteLine("2 - Add A Record");
                Console.WriteLine("3 - Delete A Record");
                Console.WriteLine("4 - Update A Record");
                Console.WriteLine("0 - Close Application");

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
                        DeleteSleepRecord();
                        break;
                    case "4":
                        UpdateSleepRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Commmand. Press any key and enter to continue");
                        Console.ReadLine();
                        break;
                }
            }
        }


        private void ProcessSleepAdd()
        {
            var duration = GetDurationInput();
            var sleepType = CalculateSleepType(duration);

            Sleep sleep = new();

            sleep.Duration = duration;
            sleep.SleepType = sleepType;

            codingController.PostSleep(sleep);
        }

        private void DeleteSleepRecord()
        {
            codingController.GetSleepData();
            Console.WriteLine("Please add id of the record you want to delete (or press 0 to reutrn to Main Menu).");

            string commandInput = Console.ReadLine();

            while (!int.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\n You have to type a valid Id (or 0 to return to the Main Menu). \n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) MainMenu();

            var sleep = codingController.GetBySleepId(id);

            while (sleep.Id == 0)
            {
                Console.WriteLine($"\nRecord with id {id} doesn't exist\n");
                DeleteSleepRecord();
            }

            codingController.DeleteSleep(id);
        }

        private void UpdateSleepRecord()
        {
            codingController.GetSleepData();

            Console.WriteLine("Please add id of the record you want to update (or 0 to return to the Main Menu).");
            string commandInput = Console.ReadLine();

            while (!Int32.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\nYou have to type an Id (or 0 to return to the Main Menu).\n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) MainMenu();

            var sleep = codingController.GetBySleepId(id);

            while (sleep.Id == 0)
            {
                Console.WriteLine($"\nRecord with Id {id} doesn't exist\n");
                UpdateRecord();
            }

            var updateInput = "";

            bool updating = true;
            while (updating == true)
            {
                Console.Clear();
                Console.WriteLine("What do you want to do?");
                Console.WriteLine($"Type 'u' to update record");
                Console.WriteLine($"Type '0' to go back to the Main Menu");

                updateInput = Console.ReadLine();

                switch (updateInput)
                {
                    case "u":
                        sleep.Duration = GetDurationInput();
                        sleep.SleepType = CalculateSleepType(sleep.Duration);
                        updating = false;
                        break;
                    case "0":
                        MainMenu();
                        break;
                    default:
                        Console.WriteLine($"\nType '0' to go back to the Main Menu");
                        break;

                }
            }
            codingController.UpdateSleep(sleep);
            Console.WriteLine("Record has been updated. Press a key to continue");
            Console.ReadLine();
            MainMenu();
        }
    }
}
