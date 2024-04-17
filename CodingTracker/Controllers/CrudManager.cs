using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.Controllers;

public class CrudManager
{
    public static void InsertRecord()
    {
        using (var connection = DbBuilder.GetConnection())
        {
            try
            {
                var startDateTime = HelpersValidation.GetDateTimeInput();
                var endDateTime = HelpersValidation.GetDateTimeInput();
                
                
            }
            catch (HelpersValidation.InputZero)
            {
                AnsiConsole.MarkupLine("[bold]Returning to main menu...[/]");
            }
        }
    }
}