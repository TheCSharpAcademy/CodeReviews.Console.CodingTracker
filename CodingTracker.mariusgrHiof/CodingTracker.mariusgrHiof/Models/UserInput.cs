namespace CodingTracker.Models;

public class UserInput
{
    public string? GetMenuChoice()
    {
        Console.Write("Enter a number: ");
        string? choice = Console.ReadLine();

        return choice;
    }

    public string? GetDateInput()
    {
        Console.Write("Enter a date(format: dd/mm/yyyy HH:MM i.e 20/10/2023 14:54): ");
        string? stringDate = Console.ReadLine();

        return stringDate;
    }
}

