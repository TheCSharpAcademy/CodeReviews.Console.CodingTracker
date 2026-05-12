using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodingTracker.urasylmaz1.Helpers
{
    public class UserInput
    {
        public static DateTime GetDateTime(string message)
        {
            while (true)
            {
                string input = AnsiConsole.Ask<string>(message);

                if (Validation.IsValidDateTime(input))
                {
                    return DateTime.ParseExact(
                        input,
                        "yyyy-MM-dd HH:mm",
                        null
                    );
                }

                AnsiConsole.MarkupLine(
                    "[red]Invalid format![/]"
                );

                AnsiConsole.MarkupLine(
                    "[yellow]Use format: yyyy-MM-dd HH:mm[/]"
                );
            }
        }
    }
}
