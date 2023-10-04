using CodingTracker.Models;
using System.Text.RegularExpressions;

namespace CodingTracker.Services;

public class UserValidation
{
    public static string? ValidateMenuInput(string input)
    {
        string numbers = "0123456789";

        while (!numbers.Contains(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be Between 0 and 9");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your Selection: ");

            input = Console.ReadLine();

            return ValidateMenuInput(input);
        }

        return input;
    }

    public static string? IsValidTimeFormat(string input)
    {
        TimeSpan dummyOutput;
        while (!TimeSpan.TryParse(input, out dummyOutput))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Invalid DateTime Format.");
            Console.WriteLine("Use HH:MM Format");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your Entry: ");

            input = Console.ReadLine();
            return IsValidTimeFormat(input);
        }

        return dummyOutput.ToString();
    }

    public static string? IsValidDateFormat(string input)
    {
        DateTime dummyOutput;

        while(!DateTime.TryParse(input, out dummyOutput))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Invalid DateTime Format.");
            Console.WriteLine("Use HH:MM Format");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your Entry: ");

            input = Console.ReadLine();
            return IsValidTimeFormat(input);
        }

        return dummyOutput.ToString("MM/dd/yyyy");
    }

    public static int? ValidateIdSelection(int? selectedId, List<CodeSession> sessions)
    {
        while (!sessions.Any(x => x.Id == selectedId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Selected Number Does Not Exist");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Your Selection: ");

            string input = Console.ReadLine();
            selectedId = ValidateNumericInput(input);
            return ValidateIdSelection(selectedId, sessions);
        }

        return selectedId;
    }
    public static int ValidateNumericInput(string input)
    {
        int result;
        while (!int.TryParse(input, out result))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be An Integer.");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Your Selection: ");

            input = Console.ReadLine();
            return ValidateNumericInput(input);
        }

        return result;
    }

    public static string? VerifyEmptyOrChanged(string? oldEntry, string newEntry)
    {
        if (String.IsNullOrEmpty(newEntry) || newEntry == oldEntry)
        {
            return oldEntry;
        }
        else
        {
            return newEntry;
        }
    }

    public static string? ValidateYesNo(string? input)
    {
        List<string> options = new() { "y", "n" };

        while (!options.Contains(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be Y or N");
            Console.Write("Your Selection: ");

            input = Console.ReadLine();
            return ValidateYesNo(input);
        }

        return input;
    }
}
