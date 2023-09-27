internal class GetUserInput
{
    CodingController codingController = new CodingController();
    Coding coding = new Coding();

    internal void MainMenu()
    {
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application");
            Console.WriteLine("Type 1 to View Record");
            Console.WriteLine("Type 2 to Add Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");

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

    private void ProcessUpdate()
    {
        codingController.Get();

        Console.WriteLine("Please select id of the category you want to update (or 0 to return to Main Menu)");
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
            ProcessUpdate();
        }

        var updateInput = "";
        bool updating = true;
        while (updating == true)
        {
            Console.WriteLine($"\nType 'd' for Date \n");
            Console.WriteLine($"\nType 't' for Duration \n");
            Console.WriteLine($"\nType 's' to save update \n");
            Console.WriteLine($"\nType '0' to go back to Main Menu \n");

            updateInput = Console.ReadLine();

            switch (updateInput)
            {
                case "d":
                    coding.StartDate = Helpers.GetDateInput("Start Date");
                    coding.EndDate = Helpers.GetDateInput("End Date");
                    break;
                case "t":
                    coding.TotalDuration = Helpers.CalculateDuration(coding.StartDate, coding.EndDate);
                    break;
                case "0":
                    MainMenu();
                    updating = false;
                    break;
                case "s":
                    updating = false;
                    break;
                default:
                    Console.WriteLine($"\nType '0' to go back to Main Menu \n");
                    break;
            }
        }

        codingController.Update(coding);
        MainMenu();
    }
    private void ProcessDelete()
    {
        codingController.Get();
        Console.WriteLine("Please select id of the category you want to delete (or 0 to return to Main Menu).");

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
    }
    private void ProcessAdd()
    {
        Session session = Helpers.GetNewSession();

        coding.StartDate = session.StartDate;
        coding.EndDate = session.EndDate;
        coding.TotalDuration = session.TotalDuration;

        codingController.Post(coding);
    }
}