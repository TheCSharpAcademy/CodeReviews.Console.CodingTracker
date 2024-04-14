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
                var startDate = UserInput.GetDateInput();
            }
            catch (Validation.InputZero)
            {
                AnsiConsole.MarkupLine("[bold]Returning to main menu...[/]");
            }
        }
    }
}