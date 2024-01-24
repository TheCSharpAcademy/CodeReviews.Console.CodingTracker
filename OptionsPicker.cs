using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker;

public static class OptionsPicker
{
    public static int CurrentIndex { get; private set; }
    public static void Navigate(string[] menuOptions)
    {
        bool enterPressed = false;
        CurrentIndex = 0;

        do
        {

            Console.Clear();
            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (CurrentIndex == i)
                {
                    HighlightOption();
                }
                else
                {
                    Console.ResetColor();
                }

                Console.WriteLine(string.Format("  {0,-30}", menuOptions[i]));
                Console.ResetColor();
            }
            do
            {
                while (!Console.KeyAvailable) { };
                var keypress = Console.ReadKey().Key;

                if (keypress == ConsoleKey.DownArrow && CurrentIndex < menuOptions.Length - 1)
                {
                    CurrentIndex++;
                    break;
                }
                else if (keypress == ConsoleKey.UpArrow && CurrentIndex > 0)
                {
                    CurrentIndex--;
                    break;
                }
                else if (keypress == ConsoleKey.Enter)
                {
                    Console.SetCursorPosition(0, CurrentIndex);
                    EnterPressed(menuOptions);
                    enterPressed = true;
                    break;
                }
            } while (true);

        } while (!enterPressed);
    }
    private static void EnterPressed(string[] menuOptions)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(string.Format("  {0,-30}", menuOptions[CurrentIndex]));
        Console.ResetColor();
        Thread.Sleep(150);

    }
    private static void HighlightOption()
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
    }
}

