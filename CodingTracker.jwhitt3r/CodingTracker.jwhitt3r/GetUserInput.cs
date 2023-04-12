using System.Globalization;

namespace CodingTracker.jwhitt3r
{
    /// <summary>
    /// GetUserInput provides a menu interface for the user, as well as,
    /// providing the processing layer for the application before the data
    /// is interacted to the database
    /// </summary>
    internal class GetUserInput
    {
        CodingController codingController = new();

        /// <summary>
        /// Provides the main menu for the user to interact with
        /// </summary>
        internal void MainMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View records");
                Console.WriteLine("Type 2 to Add records");
                Console.WriteLine("Type 3 to Delete records");
                Console.WriteLine("Type 4 to Update records");


                string commandInput = Console.ReadLine();

                while (string.IsNullOrEmpty(commandInput))
                {
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    commandInput = Console.ReadLine();
                }

                switch (commandInput)
                {
                    case "0":
                        closeApp = true;
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
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }

        /// <summary>
        /// ProcessUpdate manages the users input for the ID as to ensure that the Id is correct and an integer
        /// </summary>
        private void ProcessUpdate()
        {
            codingController.Get();

            Console.WriteLine("Please add id of the category you want to update (or 0 to return to Main Menu).");
            string commandInput = Console.ReadLine();

            while (!Int32.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\nYou have to type an Id (or 0 to return to Main Menu).\n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) MainMenu();

            var coding = codingController.GetById(id);

            while (coding.Id == 0)
            {
                Console.WriteLine($"\nRecord with id {id} doesn't exist\n");
                ProcessUpdate();
            }

            var updateInput = "";

            bool updating = true;
            while (updating == true)
            {
                Console.WriteLine($"\nType 'd' for Date \n");
                Console.WriteLine($"\nType 't' for time session \n");
                Console.WriteLine($"\nType 's' to save update \n");
                Console.WriteLine($"\nType '0' to Go Back to Main Menu \n");

                updateInput = Console.ReadLine();

                switch (updateInput)
                {
                    case "d":
                        coding.Date = GetDateInput();
                        break;

                    case "t":
                        coding.StartTime = GetTimeInput();
                        coding.EndTime = GetTimeInput();
                        coding.Duration = GetDuration(coding.StartTime, coding.EndTime);
                        break;

                    case "0":
                        MainMenu();
                        updating = false;
                        break;

                    case "s":
                        updating = false;
                        break;

                    default:
                        Console.WriteLine($"\nType '0' to Go Back to Main Menu \n");
                        break;
                }
            }
            codingController.Update(coding);
            MainMenu();
        }

        private string GetDuration(string startTime, string endTime)
        {
            Console.WriteLine(startTime, endTime);
            int start = int.Parse(String.Join("", startTime.Split(':')));
            int end = int.Parse(String.Join("", endTime.Split(':')));
            string result = (end - start).ToString("##:##");
            return result;
        }

        /// <summary>
        /// ProcessDelete ensures that the user has entered a valid Id for the record
        /// </summary>
        private void ProcessDelete()
        {
            codingController.Get();
            Console.WriteLine("Please add id of the category you want to delete (or 0 to return to Main Menu).");

            string commandInput = Console.ReadLine();

            while (!Int32.TryParse(commandInput, out _) || string.IsNullOrEmpty(commandInput) || Int32.Parse(commandInput) < 0)
            {
                Console.WriteLine("\nYou have to type a valid Id (or 0 to return to Main Menu).\n");
                commandInput = Console.ReadLine();
            }

            var id = Int32.Parse(commandInput);

            if (id == 0) MainMenu();

            var coding = codingController.GetById(id);


            while (coding.Id == 0)
            {
                Console.WriteLine($"\nRecord with id {id} doesn't exist\n");
                ProcessDelete();
            }

            codingController.Delete(id);
            MainMenu();
        }

        /// <summary>
        /// ProcessAdd creates and populates a new record to be added to the database
        /// </summary>
        private void ProcessAdd()
        {
            CodingSession coding = new();

            coding.Date = GetDateInput();
            coding.StartTime = GetTimeInput();
            coding.EndTime = GetTimeInput();
            coding.Duration = GetDuration(coding.StartTime, coding.EndTime);

            codingController.Post(coding);
        }

        /// <summary>
        /// GetDurationInput is used to get a correctly formatted hour and minute representation of how long
        /// a coding session took
        /// </summary>
        /// <returns>The duration is then returned to the caller</returns>
        private string GetTimeInput()
        {
            Console.WriteLine("\n\nPlease insert a Time: (Format: hh:mm). Type 0 to return to main menu.\n\n");
            string timeInput = Console.ReadLine();

            if (timeInput == "0")
            {
                MainMenu();
            }

            while (!TimeSpan.TryParseExact(timeInput, "h\\:mm", CultureInfo.InvariantCulture, out _))
            {
                Console.WriteLine("\n\nDuration invalid. Please insert a Time: (Format: hh:mm) or type 0 to return to main menu\n\n");
                timeInput = Console.ReadLine();
                if(timeInput == "0")
                {
                    MainMenu();
                }

                var parsedDuration = TimeSpan.Parse(timeInput);

                long date = parsedDuration.Ticks;
                if (date < 0)
                {
                    Console.WriteLine("\n\nNegative time not allowed. \n\n");
                    GetTimeInput();
                }

            }

            return timeInput;

        }

        /// <summary>
        /// GetDateInput ensures that the user has input a valid format before submitting it to the database
        /// </summary>
        /// <returns>A correctly formatted date is returned to the caller</returns>
        private string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");

            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                MainMenu();
            }

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
    }
}