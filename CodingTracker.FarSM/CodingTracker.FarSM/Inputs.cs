using System.Globalization;

namespace CodingTracker.FarSM
{
    internal class Inputs
    {
        public static void MainMenu()
        {
            Console.Clear();
            bool doLoop = true;
            while (doLoop)
            {
                Console.WriteLine(@"    
    MAIN MENU
    ---------
    What do you want to do?
    Type 0 to exit app
    Type 1 to view the data
    Type 2 to insert data
    Type 3 to delete data
    Type 4 to update data
    Type 5 to retrieve specific data");
                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        doLoop = false;
                        Environment.Exit(0);
                        break;
                    case "1":
                        CodingController.ViewAllEntries();
                        break;
                    case "2":
                        CodingController.InsertEntry();
                        break;
                    case "3":
                        CodingController.DeleteEntry();
                        break;
                    case "4":
                        CodingController.UpdateEntry();
                        break;
                    case "5":
                        RetrieveSpecificData();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter the correct input");
                        break;
                }
            }
        }

        private static void RetrieveSpecificData()
        {
            Console.Clear();
            bool doLoop = true;
            while (doLoop)
            {
                Console.WriteLine(@"    
    
    What do you want to do?

    Type 0 to exit app
    Type 1 to record present coding time
    Type 2 to view the details about last month
    Type 3 to set coding goals
    Type 4 to return to the main menu");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        doLoop = false;
                        Environment.Exit(0);
                        break;
                    case "1":
                        CodingController.RecordCodingTime();
                        break;
                    case "2":
                        CodingController.LastFourWeeksData();
                        break;
                    case "3":
                        CodingController.SetCodingGoals();
                        break;
                        case "4":
                        MainMenu();
                        break;  
                    default:
                        Console.WriteLine("Invalid input. Please enter the correct input");
                        break;
                }
            }
        }

        public static DateTime GetTimeInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            DateTime clearInput = new DateTime();

            while (!DateTime.TryParseExact(input, "hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None, out clearInput))
            {
                Console.Write("\nInvalid input. Please enter time in the format hh:mm am/pm : ");
                input = Console.ReadLine();

            }
            return clearInput;
        }

        public static DateTime GetDateInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            DateTime clearInput = new DateTime();
            DateTime finalInput = new DateTime();

            while (!DateTime.TryParseExact(input, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out clearInput))
            {
                Console.Write("Invalid input. Enter the date in the format dd-mm-yyyy : ");
                input = Console.ReadLine();
            }
            return clearInput;
        }

        public static int GetNumberInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();

            while (!int.TryParse(input, out _) || (Convert.ToInt32(input) < 0))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                input = Console.ReadLine();

            }
            int clearInput = Convert.ToInt32(input);
            return clearInput;
        }

    }
}
