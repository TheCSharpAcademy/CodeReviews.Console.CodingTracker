using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data;

internal class RecordUpdate
{
    internal static void Update()
    {
        Console.Clear();
        var dictionaryOfSessions = RecordRead.MakeRecordsMap();
        if (RecordRead.CheckIfNoRecordsAvailable(dictionaryOfSessions.Keys)) return;

        var choice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Choose a record to update", dictionaryOfSessions.Keys);
        if (choice == DisplayInfoHelpers.Back) return;

        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();

            if (dictionaryOfSessions.TryGetValue(choice, out CodingSession? recordToUpdate))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", recordToUpdate.Id, DbType.Int64);

                var exists = connection.ExecuteScalar<bool>(
                    "SELECT EXISTS(SELECT 1 FROM sessions WHERE id = @id)", parameters);

                if (exists)
                {
                    AnsiConsole.MarkupLine($"[yellow]Updating record:[/]\n{choice}\n");

                    var (exit, startTime, endTime, duration) = InputDataHandler.GetData();
                    if (exit)
                    {
                        Console.Clear();
                        return;
                    }

                    var updParameters = new DynamicParameters();
                    updParameters.Add("@id", recordToUpdate.Id);
                    updParameters.Add("@start_time", startTime);
                    updParameters.Add("@end_time", endTime);
                    updParameters.Add("@duration", duration);
                    connection.Execute($@"
                        UPDATE sessions
                        SET start_time = @start_time, end_time = @end_time, duration = @duration
                        WHERE id = @id",
                        updParameters);

                    AnsiConsole.MarkupLine("[green]Record updated successfully![/]");
                    DisplayInfoHelpers.PressAnyKeyToContinue();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Record not found in the database.[/]");
                    DisplayInfoHelpers.PressAnyKeyToContinue();
                }
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to update record.[/]");
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
