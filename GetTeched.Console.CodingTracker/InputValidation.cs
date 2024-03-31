using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class InputValidation
{
    internal bool DateValidation(string userDateInput, string inputType)
    {
        while (!DateTime.TryParseExact(userDateInput, "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Markup("\n[Red]Invalid date entered. Please try again.[/]\n\n"));
            return false;
        }
        return true;
    }

    internal bool TimeValidation(string userTimeInput, string inputType)
    {
        while (!DateTime.TryParseExact(userTimeInput, "HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Markup("\n[Red]Invalid time entered. Please try again.[/]\n\n"));
            return false;
        }
        return true;
    }

    internal bool DateTimeValidation(string userTimeInput)
    {
        List<string> userInput = userTimeInput.Split(' ').ToList();
        while (!DateTime.TryParseExact(userTimeInput, "dd-MM-yy HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
        {
            AnsiConsole.Clear();
            if (userInput.Count > 1)
            {
                if (!DateTime.TryParseExact(userInput[0], "dd-MM-yy", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
                {
                    AnsiConsole.Write(new Markup("\n[Red]Invalid date entered. Please try again.[/]\n\n"));
                    return false;
                }
                else if (!DateTime.TryParseExact(userInput[1], "HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out _))
                {
                    AnsiConsole.Write(new Markup("\n[Red]Invalid time entered. Please try again.[/]\n\n"));
                    return false;
                }
            }
            else
            {
                AnsiConsole.Write(new Markup("\n[red]Invalid entry made. Please try again.[/]\n\n"));
                return false;
            }
            
        }
        return true;
    }

    internal bool SessionIdInRange(int[] sessionIds, int idSelection)
    {
        if (sessionIds.Contains(idSelection))
        {
            AnsiConsole.Write(new Markup($"\n[red]ID: {idSelection} was found.[/]\n"));
            return true;
        }
        AnsiConsole.Write(new Markup($"[red]ID:{idSelection} was not found. Press any key to try again.[/]\n\n"));
        Console.ReadLine();
        return false;
        
    }
}
