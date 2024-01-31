using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        HandleInputError(userInput, "\n  Enter a valid time (HH:mm):");
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
                        HandleInputError(userInput, "\n  Enter a valid date (YYYY-MM-DD):");
                    }
                }
            } while (true);
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
                    if(!escapeOption) continue;

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
        private static void HandleInputError(string userInput, string errorMessage)
        {
            if (_priorError)
            {
                Console.SetCursorPosition(userInput.Length, Console.GetCursorPosition().Top - 1);

                foreach (char character in userInput)
                {
                    Console.Write("\b \b");
                }
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 2);
            }
            _priorError = true;
            Console.WriteLine(errorMessage);
        }

        public static void DisplayError(string message, string actionMessage = "continue")
        {
            Console.WriteLine($"{message} Press any key to {actionMessage}...");
            Console.ReadKey();
        }
    }
}
