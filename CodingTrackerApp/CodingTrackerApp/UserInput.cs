using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CodingTrackerApp
{
    public class UserInput
    {
        public static BigInteger userInput;

        public static BigInteger ReadUserInput()
        {
            Console.Write("Enter Command Input: ");
            userInput = NumericInputOnly();
            Console.WriteLine();
            Console.WriteLine();

            return userInput;
        }
        public static BigInteger NumericInputOnly()
        {
            string msg = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    if (char.IsDigit(key.KeyChar) || key.KeyChar == '.')
                    {
                        if (key.KeyChar == '.' && msg.Contains(".")) continue;
                        else
                        {
                            msg += key.KeyChar;
                            Console.Write(key.KeyChar);
                        }
                    }
                }
                else if (key.Key == ConsoleKey.Backspace && msg.Length > 0)
                {
                    msg = msg.Substring(0, (msg.Length - 1));
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter || string.IsNullOrEmpty(msg));

            if (BigInteger.TryParse(msg, out BigInteger val))
            {
                return val;
            }
            return userInput;
        }
    }
}
