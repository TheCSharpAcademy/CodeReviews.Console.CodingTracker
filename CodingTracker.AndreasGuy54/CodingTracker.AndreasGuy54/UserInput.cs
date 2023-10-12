using System.Globalization;

namespace CodingTracker.AndreasGuy54
{
    internal static class UserInput
    {
        internal static void GetUserInput()
        {
            Console.Clear();

            bool closeApp = false;

            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine("\n\nMain Menu");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("------------------------------------------\n");

                int userInput;
                bool validInput = int.TryParse(Console.ReadLine().ToLower().Trim(), out userInput);

                while (!validInput || userInput < 0)
                {
                    Console.WriteLine("Enter a valid number");
                    validInput = int.TryParse(Console.ReadLine().ToLower().Trim(), out userInput);
                }

                switch (userInput)
                {
                    case 0:
                        Console.WriteLine("\nGoodbye:\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case 1:
                        CodingController.ShowRecords();
                        break;
                    case 2:
                        CodingController.InsertRecord();
                        break;
                    case 3:
                        CodingController.DeleteRecord();
                        break;
                    case 4:
                        CodingController.UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Please type a number from 0 to 4:\n");
                        break;
                }
            }
        }

        internal static string GetDateInput()
        {
            Console.Write("Please insert the date: ");
            string dateInput = Console.ReadLine();

            if (dateInput.Length == 0)
                GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("\n\nInvalid data. Format = dd-MM-yy HH:mm.\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine().ToLower().Trim();

            while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid Input. Enter a valid positive number");
                numberInput = Console.ReadLine().ToLower().Trim();
            }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}
