using System.Collections;
using System.Globalization;

namespace CodingTracker.Ledana
{
    public class GetUserInput
    {
        CodingController codingController = new();
        TimeSpan StartTime = TimeSpan.Zero;
        TimeSpan EndTime = TimeSpan.Zero;
        internal void MainMenu()
        {
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close the Application");
                Console.WriteLine("Type 1 to View records");
                Console.WriteLine("Type 2 to Add record");
                Console.WriteLine("Type 3 to Delete record");
                Console.WriteLine("Type 4 to Update record");

                string? commandInput = Console.ReadLine();

                while (string.IsNullOrEmpty(commandInput))
                {
                    Console.WriteLine("\nInvalid Command. Please type a number between 0 and 4\n");
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

        private void ProcessUpdate()
        {
            codingController.Get();
            Console.WriteLine("Please add Id of the category you want to update (or 0 to return to Main Menu).");

            string idInput = Console.ReadLine();
            if (idInput == "0") MainMenu();

            while (!int.TryParse(idInput, out _) || string.IsNullOrEmpty(idInput) || Int32.Parse(idInput) < 0)
            {
                Console.WriteLine("Please put an id number or 0 to return to main menu");
                idInput = Console.ReadLine();
            }

            var id = Int32.Parse(idInput);

            if (id == 0) MainMenu();

            var coding = codingController.GetById(id);

            while (coding == null)
            {
                Console.WriteLine($"Record with Id {id} doesn't exist\n");
                ProcessUpdate();
            }

            var updateInput = "";
            bool updating = true;
            while (updating)
            {
                Console.WriteLine($"\nType 'd' for Date \n");
                Console.WriteLine($"\nType 'b' for Start \n");
                Console.WriteLine($"\nType 'e' for End \n");
                Console.WriteLine($"\nType 's' to save update \n");
                Console.WriteLine($"\nType '0' to go back to Main Menu \n");

                updateInput = Console.ReadLine();

                switch (updateInput)
                {
                    case "d":
                        coding.Date = GetDateInput();
                        break;
                    case "b":
                        coding.Start = GetStartTime().ToString();
                        break;
                    case "e":
                        coding.End = GetEndTime().ToString();
                        break;
                    case "0":
                        MainMenu();
                        updating = false;
                        break;
                    case "s":
                        updating = false;
                        break;

                    default:
                        Console.WriteLine($"\nType '0' to go back toMain Menu");
                        break;
                }

            }
            
            coding.Duration = GetDuration(coding);
            codingController.Update(coding);
            MainMenu();
        }

        private void ProcessDelete()
        {
            codingController.Get();
            Console.WriteLine("Please add Id of the category you want to delete (or 0 to return to Main Menu).");

            string idInput = Console.ReadLine();
            if (idInput == "0") MainMenu();

            while (!int.TryParse(idInput, out _) || string.IsNullOrEmpty(idInput) || Int32.Parse(idInput) < 0)
            {
                Console.WriteLine("Please put an id number or 0 to return to main menu");
                idInput = Console.ReadLine();
            }

            var id = Int32.Parse(idInput);

            if (id == 0) MainMenu();

            var coding = codingController.GetById(id);

            while (coding == null)
            {
                Console.WriteLine($"Record with Id {id} doesn't exist\n");
                ProcessDelete();
            }
            codingController.Delete(id);
        }

        private void ProcessAdd()
        {
            var date = GetDateInput();
            var startTime = GetStartTime();
            var endTime = GetEndTime();
            

            Coding coding = new();
            
            coding.Start = startTime;
            coding.End = endTime;
            coding.Date = date;
            var duration = GetDuration(coding);
            coding.Duration = duration;

            codingController.Post(coding);
        }

        private string GetEndTime()
        {
            Console.WriteLine("\n\nPlease insert the time you ended coding: (Format: hh:mm) after the start time. Type 0 to return to main menu. \n\n");
            string endTime = Console.ReadLine();

            if (endTime == "0") MainMenu();

            TimeSpan end;
            while (!TimeSpan.TryParseExact(endTime, "h\\:mm", CultureInfo.InvariantCulture, out end) || end < StartTime)
            {
                Console.WriteLine("\n\nEnd time invalid. Please insert the end time: (Format: hh:mm) after the start time or type 0 to return to main menu");
                endTime = Console.ReadLine();
                if (endTime == "0") MainMenu();
            }
            EndTime = end;
            return end.ToString();
        }

        private string GetStartTime()
        {
            Console.WriteLine("\n\nPlease insert the time you started coding: (Format: hh:mm). Type 0 to return to main menu. \n\n");
            string startTime = Console.ReadLine();

            if (startTime == "0") MainMenu();

            TimeSpan start;
            while (!TimeSpan.TryParseExact(startTime, "h\\:mm", CultureInfo.InvariantCulture, out start))
            {
                Console.WriteLine("\n\nStart time invalid. Please insert the start time: (Format: hh:mm) or type 0 to return to main menu");
                startTime = Console.ReadLine();
                if (startTime == "0") MainMenu();
            }
            StartTime = start;
            return start.ToString();
        }

        public string GetDuration(Coding coding)
        {
            var start = TimeSpan.Parse(coding.Start);
            var end = TimeSpan.Parse(coding.End);
            return (end - start).ToString();
        }

        private string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");
            string dateInput = Console.ReadLine();

            if (dateInput == "0") MainMenu();
            
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-Us"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nNot a valid date. Please insert the date with the format: dd-mm-yy.\n\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
    }
}