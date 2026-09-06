using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker
{
    internal static class UserInput
    {
        
        public static void WaitForUser(string message = "Press any key to continue...")
        {
            AnsiConsole.MarkupLine($"\n[grey]{message}[/]");
            Console.ReadKey(true);
            Console.Clear();
        }

        public static string AnySelection(string msg, string[] choices)
        {
            string choice = " ";
            choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title($"[green]{msg}[/]")
                    .AddChoices(choices));

            return choice;
        }

        public static int GetIntFromUser(List<int> validIDs)
        {
            int value;
            bool incorrectInt = true;
            do
            {
                AnsiConsole.Markup("Enter valid positive Integer here: ");
                incorrectInt = !Validation.CheckStringToInt(Console.ReadLine(), out value);
                if (incorrectInt || !(validIDs.Contains(value)))
                {
                    AnsiConsole.MarkupLine($"[bold red]INVALID[/] Input. Try again.");
                    incorrectInt = true;
                }
            } while (incorrectInt);
            return value;
        }

        public static string GetString(string promptMessage = "Enter here: ")
        {
            while (true)
            {
                AnsiConsole.Markup(promptMessage);
                string? input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                AnsiConsole.MarkupLine("[bold red]Input cannot be empty. Please try again.[/]");
            }
        }
        public static void StopSessionOnKeyPress()
        {
            Console.ReadKey(true);
        }

        public static void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
