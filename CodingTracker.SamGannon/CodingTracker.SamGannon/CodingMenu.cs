using CodingTracker.SamGannon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.SamGannon
{
    internal class CodingMenu
    { 
        internal void ShowCodingMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.Clear();
                Console.WriteLine("-----Coding Menu-----");
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
                        CodingController codingController = new();
                        codingController.GetCodingData();
                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadLine();
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

        private void ProcessAdd()
        {
            Validation validation = new();
            var startTime = validation.GetStartTime();
            var endTime = validation.GetEndTime();
            var duration = validation.CalculateDuration(startTime, endTime, ProcessAdd);

            Coding coding = new();

            coding.Date = validation.GetDateInput();
            coding.Duration = duration;

            CodingController codingController = new();
            codingController.Post(coding);
        }

        private void DeleteRecord()
        {
            CodingController codingController = new();
            codingController.GetCodingData();
            Console.WriteLine("Please add id of the record you want to delete (or press 0 to return to Main Menu).");

            string commandInput = Console.ReadLine();

            Validation validation = new();
            var id = validation.ValidateIdInput(commandInput);

            var coding = codingController.GetById(id);

            while (coding.Id == 0)
            {
                Console.WriteLine($"\nRecord with id {id} doesn't exist. Press a key to continue.\n");
                Console.ReadLine();
                DeleteRecord();
            }

            codingController.Delete(id);
        }

        private void UpdateRecord()
        {
            CodingController codingController = new();
            GetUserInput getUserInput = new();
            codingController.GetCodingData();

            Console.WriteLine("Please add id of the record you want to update (or 0 to return to the Main Menu).");
            string commandInput = Console.ReadLine();

            Validation validation = new();
            var id = validation.ValidateIdInput(commandInput);

            if (id == 0) ShowCodingMenu();

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
                Console.WriteLine("What do you want to update?");
                Console.WriteLine($"Type 'd' for Date");
                Console.WriteLine($"Type 't' for Time");
                Console.WriteLine($"Type '0' to go back to the Main Menu");

                updateInput = Console.ReadLine();

                switch (updateInput)
                {
                    case "d":
                        coding.Date = validation.GetDateInput();
                        updating = false;
                        break;
                    case "t":
                        var startTime = validation.GetStartTime();
                        var endTime = validation.GetEndTime();
                        coding.Duration = validation.CalculateDuration(startTime, endTime, UpdateRecord);
                        updating = false;
                        break;
                    case "0":
                        ShowCodingMenu();
                        break;
                    default:
                        Console.WriteLine($"\nType '0' to go back to the Main Menu");
                        break;

                }
            }
            codingController.Update(coding);
            Console.WriteLine("record updated Press a key to continue");
            Console.ReadLine();
            getUserInput.MainMenu();
        }
    }
}
