using Spectre.Console;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner
{
    internal static class UserInterfaceHelpers
    {
        internal static void WaitForAnyKey()
        {
            AnsiConsole.MarkupLine("[green]Press any key to continue...[/]");
            Console.ReadKey();
        }

        internal static void WaitForSpecificKey(ConsoleKey key, string message)
        {
            AnsiConsole.MarkupLine(message);

            while(Console.ReadKey(true).Key != key) { }
        }

        internal static bool WaitForEnterOrEscape_CheckForEnter(string? initialMessage = null)
        {
            if(initialMessage != null)
            {
                AnsiConsole.MarkupLine(initialMessage);
            } 

            AnsiConsole.MarkupLine("Press [green]ENTER[/] to continue or [yellow]ESC[/] to go back to the main menu");

            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;
            }
            while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);

            return key == ConsoleKey.Enter;
        }
    }
}
