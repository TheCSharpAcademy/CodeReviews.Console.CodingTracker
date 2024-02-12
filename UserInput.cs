using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CodingTracker
{
    public static class UserInput
    {
        private static bool _priorError = false;

        public static string TimeInput(MenuManager menuManager, bool blankOption)
        {
            _priorError = false;

            do
            {
                string? userInput = InputWithSpecialKeys(menuManager, true);

                if (userInput == "_escape_") return userInput;
                else if (userInput == "" && blankOption) return "_noInput_";

                if (userInput != null)
                {
                    try
                    {
                        return DateTime.Parse(userInput).ToString("HH:mm");
                    }
                    catch
                    {
                        HandleInputError("Enter a valid time (HH:mm):");
                    }
                }
            } while (true);
        }

        public static string DateInput(MenuManager menuManager, bool blankOption)
        {
            _priorError = false;

            do
            {
                string? userInput = InputWithSpecialKeys(menuManager, true);

                if (userInput == "_escape_") return userInput;
                else if (userInput == "" && !blankOption) return DateTime.Now.ToString("yyyy-MM-dd");
                else if (userInput == "" && blankOption) return "_noInput_";

                if (userInput != null)
                {
                    try
                    {
                        return DateTime.Parse(userInput).ToString("yyyy-MM-dd");
                    }
                    catch
                    {
                        HandleInputError("Enter a valid date (YYYY-MM-DD):");
                    }
                }
            } while (true);
        }

        public static CodingSession IdInput(MenuManager menuManager, Database database, List<CodingSession> codingSessionList)
        {
            _priorError = false;
            bool idFound = false;
            do
            {
                string userInput = InputWithSpecialKeys(menuManager, true);

                if (int.TryParse(userInput, out int resultId))
                {
                    foreach (var session in codingSessionList)
                    {
                        if (session.Id == resultId)
                        {
                            idFound = true;
                            return database.GetByIndex(resultId);
                        }
                    }
                    if (!idFound)
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
