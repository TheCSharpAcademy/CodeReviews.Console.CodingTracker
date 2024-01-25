using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public static class UserInput
    {
        public static string TimeInput()
        {
            do
            {
                string? userInput = InputWithSpecialKeys();

                if (userInput == "_escape_") return userInput;

                if (userInput != null)
                {
                    try
                    {
                        return DateTime.Parse(userInput).ToString("HH:mm");
                    }
                    catch
                    {
                        Console.WriteLine("  \nEnter a valid time (HH:mm):");
                    }
                }
            } while (true);
        }

        public static string InputWithSpecialKeys()
        {
            StringBuilder userInput = new StringBuilder();
            ConsoleKeyInfo keyPress;
            Console.SetCursorPosition(2,Console.GetCursorPosition().Top);

            do
            {
                keyPress = Console.ReadKey(true);

                if (keyPress.Key == ConsoleKey.Escape)
                    break;

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

            } while (keyPress.Key != ConsoleKey.Escape);

            return "_escape_";
        }
    }
}
