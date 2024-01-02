using System.Globalization;

namespace CodingTracker.StevieTV;

internal class GetUserInput
{
    CodingController codingController = new();
    internal void MainMenu()
    {
        bool closeApp = false;

        while (closeApp == false)
        {
            Console.WriteLine(
                @"MAIN MENU

What would you like to do?
1 - View record
2 - Add record
3 - Delete record
4 - Update record
0 - QUIT");

            var commandInput = Console.ReadLine();

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
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4\n");
                    break;
            }
        }
    }

    private void ProcessAdd()
    {
        var date = GetDateInput();
        var duration = GetDurationInput();

        var codingSession = new CodingSession() {Date = date, Duration = duration};

        codingController.Post(codingSession);
    }

    private void ProcessDelete()
    {
        codingController.Get();

        var codingSession = new CodingSession();

        while (codingSession.Id == 0)
        {
            Console.WriteLine("Please enter the id of the entry you want to delete, or enter 0 to return to the main menu:\n");
            var inputId = Console.ReadLine();
            
        
            while (!Int32.TryParse(inputId, out _) || string.IsNullOrEmpty(inputId) || Int32.Parse(inputId) < 0)
            {
                Console.WriteLine("Please enter a valid integer id, or enter 0 to return to the main menu:");
                inputId = Console.ReadLine();
            }
            
            var id = Int32.Parse(inputId);
            if (id == 0) MainMenu();
            codingSession = codingController.GetById(id);

            if (codingSession.Id != 0)
            {
                codingController.Delete(id);
            }
            Console.WriteLine($"Record with id {id} doesn't exist\n");
        }
    }
    
    private void ProcessUpdate()
    {
        codingController.Get();

        var codingSession = new CodingSession();

        while (codingSession.Id == 0)
        {
            Console.WriteLine("Please enter the id of the entry you want to update, or enter 0 to return to the main menu:\n");
            var inputId = Console.ReadLine();
            
        
            while (!Int32.TryParse(inputId, out _) || string.IsNullOrEmpty(inputId) || Int32.Parse(inputId) < 0)
            {
                Console.WriteLine("Please enter a valid integer id, or enter 0 to return to the main menu:");
                inputId = Console.ReadLine();
            }
            
            var id = Int32.Parse(inputId);
            if (id == 0) MainMenu();
            codingSession = codingController.GetById(id);

            if (codingSession.Id != 0)
            {
                var action = "";
                bool updating = true;
                while (updating == true)
                {
                    Console.WriteLine($"Type 'd' for Date");
                    Console.WriteLine($"Type 't' for Duration");
                    Console.WriteLine($"Type 's' to save update");
                    Console.WriteLine($"Type '0' to cancel update");

                    action = Console.ReadLine();

                    switch (action)
                    {
                        case "d":
                            codingSession.Date = GetDateInput();
                            break;
                        case "t":
                            codingSession.Duration = GetDurationInput();
                            break;
                        case "0":
                            updating = false;
                            MainMenu();
                            break;
                        case "s":
                            updating = false;
                            break;
                        default:
                            Console.WriteLine("Enter 0 to cancel update");
                            break;
                    }
                }

                codingController.Update(codingSession);
                MainMenu();
            }
            Console.WriteLine($"Record with id {id} doesn't exist\n");
        }
    }

    internal string GetDateInput()
    {
        Console.WriteLine("\nPlease enter the date (dd-mm-yy), or enter 0 to return to the main menu:\n");

        var dateInput = Console.ReadLine();

        if (dateInput == "0") MainMenu();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nNot a valid date, please enter the date with the format dd-mm-yy or enter 0 to return to the main menu.\n");
            dateInput = Console.ReadLine();
            if (dateInput == "0") MainMenu();
        }

        return dateInput;
    }

    internal string GetDurationInput()
    {
        Console.WriteLine("\nPlease enter the duration (hh:mm), or enter 0 to return to the main menu:\n");

        var durationInput = Console.ReadLine();

        if (durationInput == "0") MainMenu();

        while (!TimeSpan.TryParseExact(durationInput, "h\\:mm", CultureInfo.InvariantCulture, out _))
        {
            Console.WriteLine("\nNot a valid duration, please enter the date with the format hh:mm, or enter 0 to return to the main menu.\n");
            durationInput = Console.ReadLine();
            if (durationInput == "0") MainMenu();
        }

        return durationInput;
    }
}