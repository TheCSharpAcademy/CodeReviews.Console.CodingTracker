using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class UserInput
{
    public SessionController SessionController { get; set; }

    public UserInput(SessionController sessionController)
    {
        SessionController = sessionController;
        sessionController.UserInput = this;
    }

    static InputValidation validation = new();
    internal void MainMenu()
    {
        bool endApplication = false;
        do
        {
            var crudActions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select the operation with the arrow keys")
            .PageSize(10)
            .AddChoices(new[]
            {
                "View Sessions", "Enter New Sessions",
                "Update Session", "Delete Sessions", "Exit Application"
            }));

            switch (crudActions)
            {
                case "View Sessions":
                    AnsiConsole.Write(new Markup("[bold red]Not Implemented yet.[/]"));
                    AnsiConsole.WriteLine("\n\nPress any key to return to the Main Menu.");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case "Enter New Sessions":
                    SessionController.AddNewEntry();
                    Console.ReadLine();
                    break;
                case "Update Session":
                    AnsiConsole.Write(new Markup("[bold red]Not Implemented yet.[/]"));
                    AnsiConsole.WriteLine("\n\nPress any key to return to the Main Menu.");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case "Delete Sessions":
                    AnsiConsole.Write(new Markup("[bold red]Not Implemented yet.[/]"));
                    AnsiConsole.WriteLine("\n\nPress any key to return to the Main Menu.");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    break;
                case "Exit Application":
                    endApplication = true;
                    Environment.Exit(0);
                    break;
            }
        } while (!endApplication);
    }

    internal string[] GetSessionInput()
    {
        List<string> type = new() { "Start", "End"};
        string[] dateTime = new string[3];

        string dateInput = "";
        string timeInput = "";
        bool validDateTime = false;

        for (int i = 0; i < type.Count; i++)
        { 
            while(!validDateTime)
            {
                dateInput = AnsiConsole.Ask<string>($"Please enter the {type[i]} Date of your session [green]DD-MM-YY[/]");
                validDateTime = validation.DateValidation(dateInput, type[i]);           
            }

            validDateTime = false;
            AnsiConsole.Clear();

            while (!validDateTime)
            {
                timeInput = AnsiConsole.Ask<string>($"Please enter the Time {type[i]} of your session using a 24 hour format [green]hh:mm[/]");
                validDateTime = validation.TimeValidation(timeInput, type[i]);
            }
            validDateTime = false;
            dateTime[i] = $"{dateInput} {timeInput}";
        }
        dateTime[2] = Duration(dateTime[0], dateTime[1]);
        return dateTime;

    }

    internal string Duration(string sessionStart, string sessionEnd)
    {
        DateTime startTime = DateTime.ParseExact(sessionStart, "dd-MM-yy HH:mm", new CultureInfo("en-UK"));
        DateTime endTime = DateTime.ParseExact(sessionEnd, "dd-MM-yy HH:mm", new CultureInfo("en-UK"));

        TimeSpan duration = endTime.Subtract(startTime);
        return duration.TotalMinutes.ToString();
    }
}
