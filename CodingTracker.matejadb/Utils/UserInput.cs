using CodingTracker.matejadb.Config;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.matejadb.Utils; 
public static class UserInput {
    public static string GetDateTimeFromUser(string message) {
        bool isValid = false;
        string dateTime;

        do {
            dateTime = AnsiConsole.Ask<string>($"Enter the [darkorange]{message}[/] of your session ({AppSettings.DateFormat}):");
            isValid = Validation.ValidateDateTime(dateTime);

            if (!isValid) {
                AnsiConsole.MarkupLine("[red]Invalid DateTime format. Please enter a valid DateTime.[/]");
            }

        } while (!isValid);

        return dateTime;
    }
}
