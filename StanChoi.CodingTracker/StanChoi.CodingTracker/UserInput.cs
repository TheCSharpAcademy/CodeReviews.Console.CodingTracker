using System.Globalization;

namespace StanChoi.CodingTracker
{
    internal class UserInput
    {
        CodingController codingController = new();
        internal void MainMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View record");
                Console.WriteLine("Type 2 to Add record");
                Console.WriteLine("Type 3 to Delete record");
                Console.WriteLine("Type 4 to Update record");

                string commandInput = Console.ReadLine();

                while (string.IsNullOrEmpty(commandInput))
                {
                    Console.WriteLine("\nInvalid Command.  Please type a number from 0 to 4.\n");
                    commandInput = Console.ReadLine();
                }

                switch (commandInput)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        codingController.Get();
                        break;
                    case "2":
                        ProcessAdd();
                        break;
                    case "3":
                        ProcessDelete();
                        break;
                    case "4":
                        ProcessUpdate();
                        break;

                    default:
                        Console.WriteLine("\nInvalid Command.  Please type a numbe from 0 to 4.\n");
                        break;
                }

            }
        }

        private void ProcessAdd()
        {
            Console.WriteLine("\nStart Time");
            var startTimeInfo = GetDateTimeInput();
            Console.WriteLine("End Time");
            var endTimeInfo = GetDateTimeInput();

            while (endTimeInfo.dateTime < startTimeInfo.dateTime)
            {
                Console.WriteLine("End Time");
                endTimeInfo = GetDateTimeInput();
            }

            CodingSession codingSession = new();

            codingSession.StartTime = startTimeInfo.dateTimeString;
            codingSession.EndTime = endTimeInfo.dateTimeString;

            CalculateDuration(endTimeInfo.dateTime, startTimeInfo.dateTime, codingSession);

            codingController.Post(codingSession);
        }

        private void ProcessDelete()
        {
            codingController.Get();
            Console.WriteLine("Please add id of the code session you want to delete (or 0 to return to Main Menu).");

            string commandInput = Console.ReadLine();

            // I guess I can DRY validation logic into Validation file as suggested but idk
            while (!Int32.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\nYou have to type a valid Id (or 0 to return to Main Menu).\n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) MainMenu();

            var codingSession = codingController.GetById(id);

            while (codingSession.Id == 0)
            {
                Console.WriteLine($"\nRecord with id {id} doesn't exist\n");
                ProcessDelete();
            }

            codingController.Delete(id);
            MainMenu();
        }

        private void ProcessUpdate()
        {
            codingController.Get();

            Console.WriteLine("Please add id of the code session you want to update (or 0 to return to Main Menu).");
            string commandInput = Console.ReadLine();

            while (!Int32.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\nYou have to type a valid Id (or 0 to return to Main Menu).\n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) MainMenu();

            var codingSession = codingController.GetById(id);

            while (codingSession.Id == 0)
            {
                Console.WriteLine($"\nRecord with id {id} doesn't exist\n");
                ProcessUpdate();
            }

            var updateInput = "";

            bool updating = true;
            while (updating == true)
            {
                Console.WriteLine($"\nType 'start' for Date \n");
                Console.WriteLine($"\nType 'end' for Duration \n");
                Console.WriteLine($"\nType 'save' to save update \n");
                Console.WriteLine($"\nType '0' to Go Back to Main Menu \n");

                updateInput = Console.ReadLine();

                switch (updateInput)
                {
                    case "start":
                        var startTimeInfo = GetDateTimeInput();
                        DateTime endDateTime;
                        DateTime.TryParseExact(codingSession.EndTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out endDateTime);
                        while (startTimeInfo.dateTime > endDateTime)
                        {
                            Console.WriteLine("Start Time");
                            startTimeInfo = GetDateTimeInput();
                        }
                        CalculateDuration(endDateTime, startTimeInfo.dateTime, codingSession);
                        codingSession.StartTime = startTimeInfo.dateTimeString;
                        break;

                    case "end":
                        var endTimeInfo = GetDateTimeInput();
                        DateTime startDateTime;
                        DateTime.TryParseExact(codingSession.StartTime, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out startDateTime);
                        while (endTimeInfo.dateTime < startDateTime)
                        {
                            Console.WriteLine("End Time");
                            endTimeInfo = GetDateTimeInput();
                        }
                        CalculateDuration(endTimeInfo.dateTime, startDateTime, codingSession);
                        codingSession.EndTime = endTimeInfo.dateTimeString;
                        break;

                    case "0":
                        updating = false;
                        MainMenu();
                        break;

                    case "save":
                        updating = false;
                        break;

                    default:
                        Console.WriteLine($"\nType '0' to Go Back to Main Menu \n");
                        break;
                }
            }
            codingController.Update(codingSession);
            MainMenu();
        }

        internal (string dateTimeString, DateTime dateTime) GetDateTimeInput()
        {
            Console.Write("Enter a date and time (e.g., 'yyyy-MM-dd HH:mm').  Type 0 to return to main menu.\n\n");

            string dateTimeInput = Console.ReadLine();

            if (dateTimeInput == "0") MainMenu();

            DateTime dateTime;
            while (!DateTime.TryParseExact(dateTimeInput, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
            {
                Console.WriteLine("\n\nNot a valid datetime.  Please insert the datetime with the format: yyyy-MM-dd HH:mm.\n\n");
                dateTimeInput = Console.ReadLine();
            }

            Console.WriteLine();

            return (dateTimeInput, dateTime);
        }

        internal void CalculateDuration(DateTime endDateTime, DateTime startDateTime, CodingSession codingSession)
        {
            TimeSpan duration = endDateTime - startDateTime;

            codingSession.Duration = $"{duration.Hours}:{duration.Minutes}";
        }
    }
}
