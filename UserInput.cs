using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CodingTracker
{
    public static class UserInput
    {
        private static bool _priorError = false;

        public static string TimeInput(MenuManager menuManager)
        {
            _priorError = false;

            do
            {
                string? userInput = InputWithSpecialKeys(menuManager, true);

                if (userInput == "_escape_") return userInput;

                if (userInput != null)
                {
                    try
                    {
                        return DateTime.Parse(userInput).ToString("HH:mm");
                    }
                    catch
                    {
                        HandleInputError("\n  Enter a valid time (HH:mm):");
                    }
                }
            } while (true);
        }

        public static string DateInput(MenuManager menuManager, bool startDate)
        {
            _priorError = false;

            do
            {
                string? userInput = InputWithSpecialKeys(menuManager, true);

                if (userInput == "_escape_") return userInput;
                else if (userInput == "" && startDate) return DateTime.Now.ToString("yyyy-MM-dd");
                else if (userInput == "" && !startDate) return "_sameAsStart_";

                if (userInput != null)
                {
                    try
                    {
                        return DateTime.Parse(userInput).ToString("yyyy-MM-dd");
                    }
                    catch
                    {
                        HandleInputError("\n  Enter a valid date (YYYY-MM-DD):");
                    }
                }
            } while (true);
        }

        public static List<CodingSession> IdInput(MenuManager menuManager, Database database)
        {
            _priorError = false;
            do
            {
                string userInput = InputWithSpecialKeys(menuManager, true);

                if (int.TryParse(userInput, out int resultId))
                {
                    var codingSession = database.GetByIndex(resultId);

                    if (codingSession.Any())
                        return codingSession;
                    else
                        HandleInputError($"ID '{resultId}' not found, enter a valid ID number:");
                }
                else
                    HandleInputError("Enter a valid ID number:");
            }
            while (true);
        }

        public static string InputWithSpecialKeys(MenuManager menuManager, bool escapeOption)
        {
            StringBuilder userInput = new StringBuilder();
            ConsoleKeyInfo keyPress;

            do
            {
                keyPress = Console.ReadKey(true);


                if (keyPress.Key == ConsoleKey.Escape)
                {
                    if (!escapeOption) continue;

                    menuManager.GoBack();
                }

                else if (keyPress.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    return userInput.ToString();
                }

                else if (keyPress.Key == ConsoleKey.Backspace && userInput.Length > 0)
                {
                    Console.Write("\b \b");
                    userInput.Length--;
                }

                else if (!char.IsControl(keyPress.KeyChar))
                {
                    Console.Write(keyPress.KeyChar);
                    userInput.Append(keyPress.KeyChar);
                }
            } while (true);

        }
        private static void HandleInputError(string errorMessage)
        {
            if (_priorError)
                UserInterface.ConsoleClearLastLines(2);

            _priorError = true;
            Console.WriteLine(errorMessage);
        }

        public static void DisplayMessage(string message = "", string actionMessage = "continue")
        {
            if (message == "")
                Console.WriteLine($"\nPress any key to {actionMessage}...");
            else
                Console.WriteLine($"\n{message} Press any key to {actionMessage}...");

            Console.ReadKey();
        }
    }
}
