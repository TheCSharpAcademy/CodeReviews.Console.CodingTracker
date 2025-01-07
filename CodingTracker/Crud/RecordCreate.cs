using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

internal class RecordCreate
{
    internal static void Create()
    {
        Console.Clear();
        var (exit, startTime, endTime, duration) = InputDataHandler.GetData();
        if (exit)
        {
            Console.Clear();
            return;
        }

        InsertNewRecordIntoDb(startTime, endTime, duration);
    }

    internal static void InsertNewRecordIntoDb(DateTime startTime, DateTime endTime, TimeSpan duration)
    {
        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@start_time", startTime);
            parameters.Add("@end_time", endTime);
            parameters.Add("@duration", duration);
            connection.Execute(@"
                INSERT INTO sessions (start_time, end_time, duration)
                VALUES (@start_time, @end_time, @duration)",
                parameters);

            AnsiConsole.MarkupLine($"[green]A new record created successfully![/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to add a record to database.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]An error occurred.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
    }
}
