using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data;

internal class RecordDelete
{
    internal static void Delete()
    {
        Console.Clear();
        var dictionaryOfSessions = RecordRead.MakeRecordsMap();
        if (RecordRead.CheckIfNoRecordsAvailable(dictionaryOfSessions.Keys)) return;

        var choice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Choose a record to delete", dictionaryOfSessions.Keys);
        if (choice == DisplayInfoHelpers.Back) return;

        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();

            if (dictionaryOfSessions.TryGetValue(choice, out CodingSession? recordToDelete))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", recordToDelete.Id, DbType.Int64);

                var exists = connection.ExecuteScalar<bool>(
                    "SELECT EXISTS(SELECT 1 FROM sessions WHERE id = @id)", parameters);

                if (exists)
                {
                    AnsiConsole.MarkupLine($"[red]WARNING!\nYou want to delete that record permanently![/]");
                    AnsiConsole.MarkupLine($"{choice}\n");
                    if (!DisplayInfoHelpers.ConfirmDeletion())
                    {
                        Console.Clear();
                        return;
                    }

                    connection.Execute("DELETE FROM sessions WHERE id = @id", parameters);

                    AnsiConsole.MarkupLine("Record deleted successfully.");
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
            AnsiConsole.MarkupLine("[red]Failed to delete record from DB.[/]");
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

    internal static void DeleteAllRecords()
    {
        AnsiConsole.MarkupLine("[red]WARNING!\n\nYou want to delete ALL records permanently![/]");
        AnsiConsole.MarkupLine("[yellow]That operation can not be undone![/]");
        if (!DisplayInfoHelpers.ConfirmDeletion())
        {
            Console.Clear();
            return;
        }

        try
        {
            using var connection = new SqliteConnection(Config.ConnectionString);
            connection.Open();
            connection.Execute(@"
                DELETE FROM sessions;
                UPDATE sqlite_sequence SET seq = 0 WHERE name = 'sessions';");
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to delete records from DB.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]An error occurred.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            DisplayInfoHelpers.PressAnyKeyToContinue();
        }

        AnsiConsole.MarkupLine("\nAll records deleted successfully.");
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }
}
