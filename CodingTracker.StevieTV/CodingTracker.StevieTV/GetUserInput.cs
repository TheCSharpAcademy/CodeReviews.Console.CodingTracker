using System.Globalization;

namespace CodingTracker.StevieTV;

internal class GetUserInput
{
    readonly CodingController codingController = new();
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
        var startTime = GetStartTimeInput();
        var endTime = GetEndTimeInput(startTime);
        var duration = CalculateDuration(startTime, endTime);

        var codingSession = new CodingSession() {Date = date, StartTime = startTime, EndTime = endTime, Duration = duration};

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
                var updating = true;
                while (updating)
                {
                    Console.WriteLine($"Type 'd' for Date");
                    Console.WriteLine($"Type 'b' for Start Time");
                    Console.WriteLine($"Type 'e' for End Time");
                    Console.WriteLine($"Type 's' to save update");
                    Console.WriteLine($"Type '0' to cancel update");

                    action = Console.ReadLine();

                    switch (action)
                    {
                        case "d":
                            codingSession.Date = GetDateInput();
                            break;
                        case "b":
                            codingSession.StartTime = GetStartTimeInput();
                            break;
                        case "e":
                            codingSession.EndTime = GetEndTimeInput(codingSession.StartTime);
                            break;
                        case "0":
                            updating = false;
                            MainMenu();
                            break;
                        case "s":
                            while (!Validations.IsValidEndTime(codingSession.EndTime, codingSession.StartTime))
                            {
                                Console.WriteLine("\nThe end time is before the start time, please enter them both again\n");
                                codingSession.StartTime = GetStartTimeInput();
                                codingSession.EndTime = GetEndTimeInput(codingSession.StartTime);
                            }
                            codingSession.Duration = CalculateDuration(codingSession.StartTime, codingSession.EndTime);
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

        while (String.IsNullOrEmpty(dateInput) || !Validations.IsValidDateFormat(dateInput))
        {
            Console.WriteLine("\nNot a valid date, please enter the date with the format dd-mm-yy or enter 0 to return to the main menu.\n");
            dateInput = Console.ReadLine();
            if (dateInput == "0") MainMenu();
        }

        return dateInput;
    }
    
    internal string GetStartTimeInput()
    {
        Console.WriteLine($"\nPlease enter the start time of your activity (hh:mm), or enter 0 to return to the main menu:\n");

        var timeInput = Console.ReadLine();

        if (timeInput == "0") MainMenu();

        while (string.IsNullOrEmpty(timeInput) || !Validations.IsValidTimeFormat(timeInput))
        {
            Console.WriteLine("\nNot a valid time, please enter the date with the format hh:mm, or enter 0 to return to the main menu.\n");
            timeInput = Console.ReadLine();
            if (timeInput == "0") MainMenu();
        }

        return timeInput;
    }
    
    internal string GetEndTimeInput(string startTime)
    {
        Console.WriteLine($"\nPlease enter the end time of your activity (hh:mm) that is also later that {startTime}, or enter 0 to return to the main menu:\n");

        var timeInput = Console.ReadLine();

        if (timeInput == "0") MainMenu();

        while (string.IsNullOrEmpty(timeInput) || !Validations.IsValidTimeFormat(timeInput) || !Validations.IsValidEndTime(timeInput, startTime))
        {
            Console.WriteLine($"\nNot a valid time, please enter the date with the format hh:mm that is also later than {startTime}, or enter 0 to return to the main menu.\n");
            timeInput = Console.ReadLine();
            if (timeInput == "0") MainMenu();
        }

        return timeInput;
    }

    internal string CalculateDuration(string startTime, string endTime)
    {
        TimeSpan.TryParseExact(startTime, "h\\:mm", CultureInfo.InvariantCulture, out TimeSpan start);
        TimeSpan.TryParseExact(endTime, "h\\:mm", CultureInfo.InvariantCulture, out TimeSpan end);
        TimeSpan duration = new TimeSpan(end.Ticks - start.Ticks);

        return duration.ToString("h\\:mm");
    }
}