using System.Runtime.ConstrainedExecution;

public class Input
{
    public static void GetUserInput()
    {
        DateTime startTime;
        DateTime endTime;

        bool end = false;
        while (end == false)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the coding tracker!\n");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Press 0 to close the console.");
            Console.WriteLine("Press 1 to view Records.");
            Console.WriteLine("Press 2 to Insert a Record.");
            Console.WriteLine("Press 3 to Delete a Record.");
            Console.WriteLine("Press 4 to Update a Record.");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    end = true;
                    Console.WriteLine("Goodbye\nPress any key to quit.");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;

                case "1":
                    CodingController.ViewRecords();
                    break;

                case "2":
                    InsertInput();
                    break;

                case "3":
                    DeleteInput();
                    break;

                case "4":
                    UpdateInput();
                    break;

                default:
                    Console.WriteLine("Please enter an option between 1 and 4.");
                    break;
            }

        }
    }

    public static DateTime GeTimeInput(string message)
    {
        Console.WriteLine(message);
        string getTime = Console.ReadLine();

        if (getTime == "0") GetUserInput();

        return Validation.ValidateTime(getTime);
    }

    public static void InsertInput()
    {
        Console.Clear();

        DateTime startTime;
        DateTime endTime;

        startTime = GeTimeInput("Please enter the start Time Format: (HH:mm), Or enter 0 to exit.");
        endTime = GeTimeInput("Please enter the end Time Format: (HH:mm), Or enter 0 to exit.");

        CodingController.Insert(startTime, endTime);

    }

    public static void DeleteInput()
    {
        Console.Clear();
        CodingController.GetAllRecords();

        Console.WriteLine("Please enter a record ID to delete.\n\n");

        int id = Validation.ValidateId(Console.ReadLine());

        CodingController.Delete(id);

    }


    public static void UpdateInput()
    {
        Console.Clear();
        CodingController.GetAllRecords();

        Console.WriteLine("Please type Id of the record you want to update. Type 0 to return to main menu.\n\n");

        int ID = Validation.ValidateId(Console.ReadLine());
        
        Console.WriteLine("Please enter a Start Time ID to Update.");

        DateTime startTime = Validation.ValidateTime(Console.ReadLine());

        Console.WriteLine("Please enter a End Time ID to Update.");

        DateTime endTime = Validation.ValidateTime(Console.ReadLine());

        CodingController.Update(ID, startTime, endTime);

    }



}