using System.Globalization;

namespace CodingTracker
{
    internal static class UserInterface
    {
        public static void Start()
        {
            bool stop = false;
            while (!stop)
            {
                Menu();
                Console.Write("Option: ");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "0":
                        stop = true;
                        break;
                    case "1":
                        Console.Clear();
                        ConsoleTable.DisplayData(DbManager.AllDataToDisplay());
                        Console.WriteLine();
                        Console.WriteLine("Click enter to go back");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "2":
                        DbManager.InsertRow(false);
                        break;
                    case "3":
                        DbManager.InsertRow(true);
                        break;
                    case "4":
                        DbManager.UpdateRow();
                        break;
                    case "5":
                        DbManager.DeleteRow();
                        break;
                    default:
                        Console.Write("Unkown key pressed. Please click enter to try again.");
                        Console.ReadLine();
                        break;
                }

            }
        }
        internal static (string startDate, string endDate) GetInput()
        {
            bool validateDate = false;
            bool goBack = false;
            string? startDate = "";
            string? endDate = "";

            while (!validateDate)
            {
                startDate = GetDateTime("Insert START date and time of coding session.");
                if (string.IsNullOrEmpty(startDate))
                {
                    goBack = true;
                    break;
                }

                endDate = GetDateTime("Insert END date and time of coding session.");
                if (string.IsNullOrEmpty(endDate))
                {
                    goBack = true;
                    break;
                }

                validateDate = Validation.ValidateDates(startDate, endDate);
                if (!validateDate)
                {
                    Console.Clear();
                    Console.WriteLine("START date must be lower than END date, please click enter to try again.");
                    Console.ReadLine();
                }
            }

            if (goBack) return (null, null);
            return (startDate, endDate);
        }
        internal static (string startDate, string endDate) GetInputStopwatch()
        {
            string startDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            Console.Clear();
            Console.WriteLine("Counting time with stopwatch.");
            Console.WriteLine($"Start: {startDate}");
            Console.Write("Type 1 to stop time and add record (0 to go back): ");
            string numberText = Console.ReadLine();
            int number;
            int.TryParse(numberText, out number);

            if (!number.ToString().Equals(numberText)) number = -1;

            while (number != 0 && number != 1)
            {
                Console.Clear();
                Console.WriteLine("Counting time with stopwatch.");
                Console.WriteLine($"Start: {startDate}");
                Console.Write("Invalid input, please type 1 to stop time and add record (0 to go back): ");
                numberText = Console.ReadLine();
                int.TryParse(numberText, out number);
                if (!number.ToString().Equals(numberText)) number = -1;
            }

            if (number == 0) return (null, null);
            return (startDate, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
        }
        internal static (int id, string startTime, string endTime) GetUpdateInfo(bool delete)
        {
            Console.Clear();
            ConsoleTable.DisplayData(DbManager.AllDataToDisplay());

            List<CodingModel> data = DbManager.AllDataToList();
            List<int> ids = data.Select(x => x.Id).ToList();

            if (ids.Count == 0)
            {
                Console.Clear();
                Console.Write("No rows to update/delete, click enter to go back.");
                Console.ReadLine();
                return (0, null, null);
            }

            int id = GetInputNumber("Please insert row ID you want to update/delete (0 to go back): ");

            while (!ids.Contains(id))
            {
                if (id == 0)
                {
                    return (0, null, null);
                }
                Console.WriteLine("Row ID is not in the database, please click enter to try again.");
                Console.ReadLine();
                Console.Clear();
                ConsoleTable.DisplayData(DbManager.AllDataToDisplay());
                id = id = GetInputNumber("Please insert row ID you want to update/delete (0 to go back): ");
            }

            if (delete) return (id, null, null);

            (string startTime, string endTime) = GetInput();
            return (id, startTime, endTime);
        }
        private static int GetInputNumber(string text)
        {
            Console.Write(text);
            string numberText = Console.ReadLine();
            int number;
            int.TryParse(numberText, out number);

            while (number <= 0)
            {
                if (number == 0)
                {
                    return 0;
                }
                Console.Clear();
                Console.Write("Invalid input, " + text.ToLower());
                numberText = Console.ReadLine();
                int.TryParse(numberText, out number);
            }

            return number;
        }
        private static string GetDateTime(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
            Console.Write("Please use format dd-mm-yyyy hh:MM:ss (0 to go back): ");
            string? date = Console.ReadLine();

            if (date == "0")
            {
                return null;
            }

            while (!DateTime.TryParseExact(date, "dd-MM-yyyy HH:mm:ss",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.Clear();
                Console.WriteLine(text);
                Console.Write("Invalid input, use format dd-mm-yyyy hh:MM:ss (0 to go back): ");
                date = Console.ReadLine();

                if (date == "0")
                {
                    return null;
                }
            }

            return date;
        }
        private static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Coding Tracker Application");
            Console.WriteLine("\nMain menu");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("1 - View all records");
            Console.WriteLine("2 - Add new record");
            Console.WriteLine("3 - Add new record with stopwatch");
            Console.WriteLine("4 - Update record");
            Console.WriteLine("5 - Delete record");
            Console.WriteLine("0 - Exit");
            Console.WriteLine("-------------------------------------------");
        }
    }
}
