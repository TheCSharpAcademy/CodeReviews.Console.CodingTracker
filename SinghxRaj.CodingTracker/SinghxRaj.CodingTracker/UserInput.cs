using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SinghxRaj.CodingTracker
{
    internal class UserInput
    {
        internal static void MainMenu()
        {
            Introduction();
            bool isDone = false;
            while (!isDone)
            {
                int option = GetOption();
                HandleOption(option, ref isDone);
            }
        }

        private static void Introduction()
        {
            Console.WriteLine("This is the Coding Tracker Application.");
            Console.WriteLine("This is for tracking each of your coding sessions.");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();
        }

        private static int GetOption()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("Type an option: ");
            Console.WriteLine("Type 1 - Exit Application");
            Console.WriteLine("Type 2 - Add Coding Session");
            Console.WriteLine("Type 3 - Delete Coding Session");
            Console.WriteLine("Type 4 - Update Coding Session");

            Console.Write("Enter option:");
            string? input = Console.ReadLine();
            int option;
            while (!int.TryParse(input, out option))
            {
                Console.WriteLine("Invalid input. Must be a number between 1-4.");
                Console.Write("Re-enter an option:");
                input = Console.ReadLine();
            }
            return option;
        }

        private static void HandleOption(int option, ref bool isDone)
        {
            switch (option)
            {
                case 1:
                    isDone = true;
                    Environment.Exit(0);
                    break;
                case 2:
                    AddSession();
                    break;
                case 3:
                    DeleteSession();
                    break;
                case 4:
                    UpdateSession();
                    break;
                default:
                    break;
            }
        }

        private static void UpdateSession()
        {
            // TODO: Get Coding Session Info to update
            //       Call CodingController to update the session
            throw new NotImplementedException();
        }

        private static void DeleteSession()
        {
            // TODO: Get Coding Session Info to delete
            //       Call CodingController to delete the session
            throw new NotImplementedException();
        }

        private static void AddSession()
        {
            // TODO: Get Coding session Info to add
            //       Call CodingController to add the session
            throw new NotImplementedException();
        }
    }
}
