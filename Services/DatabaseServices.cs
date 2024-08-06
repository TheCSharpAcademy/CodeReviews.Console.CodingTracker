using Coding_Tracking_Application.DataModels;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Coding_Tracking_Application.Services;

public class DatabaseServices
{
    //creating db file and table
    public static void CreateDatabaseAndTable()
    {
        var connectionString = DatabaseConfig.dbFilePath;
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS tracker (
                id INTEGER PRIMARY KEY, 
                starttime TEXT,     
                endtime TEXT,
                codingtime TEXT
            );  
        ";
        command.ExecuteNonQuery();
    }

    //save & update to database
    public static void CreateEntry(CodingSession session)
    {
        var sql = "INSERT INTO tracker (starttime, endtime, codingtime) VALUES (@starttime, @endtime, @codingtime)";
        object[] parameters = { new { starttime = session.StartTime, endtime = session.EndTime, codingtime = session.CodingTime } };
        var connectionString = DatabaseConfig.dbFilePath;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Execute(sql, parameters);
        }
    }

    //get list from database
    public void GetSessionList()
    {
        var sql = "SELECT * FROM tracker";
        var products = new List<CodingSession>();
        var connectionString = DatabaseConfig.dbFilePath;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            products = connection.Query<CodingSession>(sql).ToList();
        }

        var table = new Table();
        table.AddColumns("[green]Start Time[/]", "[red]End Time[/]", "[blue]Time Coding[/]");
        table.Border = TableBorder.Rounded;

        foreach (var product in products)
        {
            table.AddRow(product.StartTime.ToString(), product.EndTime.ToString(), product.CodingTime);
        }
        AnsiConsole.Write(table);
        AnsiConsole.Markup("\n\n\n\n");
        UserInput.MainMenuOptions();
    }

    //delete from database
    public void DeleteEntry()
    {
        var sql = "DELETE FROM tracker";
        var connectionString = DatabaseConfig.dbFilePath;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            connection.Execute(sql);
        }

        AnsiConsole.Markup("[red]The database has been cleared.[/]\n\n\n");
        UserInput.MainMenuOptions();
    }
}
