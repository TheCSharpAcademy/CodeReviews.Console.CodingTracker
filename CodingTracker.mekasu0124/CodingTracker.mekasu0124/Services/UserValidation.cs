using CodingTracker.Models;
using System.Globalization;

namespace CodingTracker.Services;

public class UserValidation
{
    public static string? ValidateMenuInput(string? input)
    {
        string numbers = "0123456789";

        while (!numbers.Contains(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be Between 0 and 9");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your Selection: ");

            input = Console.ReadLine();
        }

        return input;
    }
    public static int? ValidateIdSelection(int? selectedId, List<CodeSession>? sessions, List<Goal>? goals)
    {
        if (sessions == null)
        {
            while (!goals.Any(x => x.Id == selectedId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Selected Number Does Not Exist");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Your Selection: ");

                string? input = Console.ReadLine();
                selectedId = ValidateNumericInput(input);
            }

            return selectedId;
        }
        else
        {
            while (!sessions.Any(x => x.Id == selectedId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Selected Number Does Not Exist");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Your Selection: ");

                string? input = Console.ReadLine();
                selectedId = ValidateNumericInput(input);
            }

            return selectedId;
        }
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
        }

        return result;
    }
    public static string? ValidateAlphaInput(string? input, string? sentence)
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string? strippedInput = input.Replace(" ", "").ToLower();

        foreach (char letter in strippedInput)
        {
            while (!alphabet.Contains(letter))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Use Only The 26-Letter English Alphabet");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{sentence}: ");

                input = Console.ReadLine();
            }
        }

        return input;
    }
    public static bool VerifyEmptyOrChanged(string? oldEntry, string? newEntry)
    {
        // true = changed ; false = not changed
        return (String.IsNullOrEmpty(newEntry) || newEntry == oldEntry) ? false : true;
    }
    public static string? VerifyDateInput(string? input)
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        string format = "MM/dd/yyyy";
        DateTimeStyles style = DateTimeStyles.None;
        DateTimeOffset result;

        while (!DateTimeOffset.TryParseExact(input, format, provider, style, out result) && !String.IsNullOrEmpty(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be In Format: MM/DD/YYYY");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter Corrected Date: ");

            input = Console.ReadLine();
        }

        return result.ToString(format);
    }
    public static string? VerifyTimeInput(string? input)
    {
        CultureInfo? provider = CultureInfo.InvariantCulture;
        string format = @"hh\:mm";
        TimeSpan result;

        while (!TimeSpan.TryParseExact(input, format, provider, out result) && !String.IsNullOrEmpty(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be In Format: HH:MM");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter Corrected Time: ");

            input = Console.ReadLine();
        }

        return result.ToString(format);
    }
    public static bool ValidateYesNo(string? sentence)
    {
        while (true)
        {
            Console.WriteLine(sentence);
            Console.Write("Your Selection: ");

            string? input = Console.ReadLine();

            switch (input.ToLowerInvariant())
            {
                case "y" or "yes" or "true":
                    return true;
                case "n" or "no" or "false":
                    return false;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error] Input Must Be 'Y', 'Yes', 'N', or 'No'");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Your Selection: ");

                    input = Console.ReadLine();
                    break;
            }
        }
    }
    public static string? ValidateSecondaryMenu(string? input)
    {
        List<string?>? options = new() { "1", "2" };

        while (!options.Contains(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be 1 or 2");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Your Selection: ");

            input = Console.ReadLine();
        }

        return input;
    }
}
