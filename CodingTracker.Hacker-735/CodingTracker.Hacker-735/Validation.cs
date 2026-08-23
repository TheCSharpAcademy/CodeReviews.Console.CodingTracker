using Microsoft.Data.Sqlite;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Timers;

namespace CodingTracker.Hacker_735
{
    internal class Validation
    {
        internal static bool IsValidTime(string input, out DateTime output)
        {
            if (DateTime.TryParseExact(input, "HH:mm", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out output))
            {
                return true;
            }
            return false;
        }

        internal static bool IsValidDate(string input, out string output)
        {
            if (DateTime.TryParseExact(input, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                output = input;
                return true;
            }
            output = null;
            return false;
        }

        static internal void SafeExecute(string errorContext, System.Action action)
        {
            try
            {
                action();
            }
            catch (SqliteException ex)
            {
                AnsiConsole.MarkupLine($"[red]Database error while {errorContext}: {ex.Message}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Unexpected error while {errorContext}: {ex.Message}[/]");
            }
        }

        static internal T SafeExecute<T>(string errorContext, Func<T> action, T fallback)
        {
            try
            {
                return action();
            }
            catch (SqliteException ex)
            {
                AnsiConsole.MarkupLine($"[red]Database error while {errorContext}: {ex.Message}[/]");
                return fallback;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Unexpected error while {errorContext}: {ex.Message}[/]");
                return fallback;
            }
        }
    }
}
