using System.Configuration;
using System.Globalization;
using codingTracker.Fennikko.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace codingTracker.Fennikko;

public class CodingController
{
    private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("connectionString") +
                                                      ConfigurationManager.AppSettings.Get("databasePath");

    public static void DatabaseCreation()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Execute(
            """
            CREATE TABLE IF NOT EXISTS coding_tracker (
                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                 StartTime TEXT,
                 EndTime TEXT,
                 Duration TEXT
                 )
            """);
    }

    public static void ManualSession()
    {
        AnsiConsole.Clear();
        var startTime = Validation.GetDateInput("Enter your start time:",
            "(Format dd-mm-yy HH:mm:ss)", "Or type 0 to return to the main menu");
        var endTime = Validation.GetDateInput("Enter your end time:",
            "(Format dd-mm-yy HH:mm:ss)", "Or type 0 to return to the main menu");
        var startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        var endDateTime = DateTime.ParseExact(endTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        while (startDateTime > endDateTime)
        {
            startTime = Validation.GetDateInput("Start time is after end time, please enter valid start time:",
                "(Format dd-mm-yy HH:mm)", "Or type 0 to return to main menu");
            startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        }
        using var connection = new SqliteConnection(ConnectionString);
        var command = "INSERT INTO coding_tracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
        var session = new CodingSession { StartTime = startTime, EndTime = endTime };
        var sessionCreation = connection.Execute(command, session);
        AnsiConsole.Write(new Markup($"[green]{sessionCreation}[/] session added"));
        Thread.Sleep(3000);
        AnsiConsole.Clear();
    }

    public static void AutoSession()
    {
        var startTime = DateTime.Now.ToString("dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        AnsiConsole.Clear();
        string autoSession;
        do
        {
            autoSession = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Session in progress, select End Session to stop session...[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "End Session"
                    }));
        } while (autoSession != "End Session");

        var endTime = DateTime.Now.ToString("dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        using var connection = new SqliteConnection(ConnectionString);
        var command = "INSERT INTO coding_tracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
        var session = new CodingSession { StartTime = startTime, EndTime = endTime };
        var sessionCreation = connection.Execute(command, session);
        AnsiConsole.Write(new Markup($"[green]{sessionCreation}[/] session added"));
        Thread.Sleep(3000);
        AnsiConsole.Clear();

    }

    public static void GetAllSessions()
    {
        AnsiConsole.Clear();
        using var connection = new SqliteConnection(ConnectionString);
        var command = "SELECT * FROM coding_tracker";
        var codingSessions = connection.Query<CodingSession>(command);

        TableCreation(codingSessions);
    }

    public static void GetSessionByFilterType(string period,string stringFilterAmount)
    {
        AnsiConsole.Clear();
        var timeNow = DateTime.Now;
        string? timeType = null;
        var filterAmount = Convert.ToInt32(stringFilterAmount);
        var filteredCodingSessions = new List<CodingSession>();
        using var connection = new SqliteConnection(ConnectionString);
        var command = "SELECT * FROM coding_tracker";
        var codingSessions = connection.Query<CodingSession>(command);

        if (period.Contains("Days"))
            timeType = "Days";
        else if (period.Contains("Weeks"))
            timeType = "Weeks";
        else if (period.Contains("Months"))
            timeType = "Months";
        else if (period.Contains("Years"))
            timeType = "Years";

        foreach (var session in codingSessions)
        {
            switch (timeType)
            {
                case "Days":

                    if (DateTime.ParseExact(session.StartTime!, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US")) >= timeNow.AddDays(-filterAmount))
                    {
                        filteredCodingSessions.Add(session);
                    }
                    break;
                case "Weeks":
                    if (DateTime.ParseExact(session.StartTime!, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US")) >= timeNow.AddDays(-filterAmount * 7))
                    {
                        filteredCodingSessions.Add(session);
                    }
                    break;
                case "Months":
                    if (DateTime.ParseExact(session.StartTime!, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US")) >= timeNow.AddMonths(-filterAmount))
                    {
                        filteredCodingSessions.Add(session);
                    }
                    break;
                case "Years":
                    if (DateTime.ParseExact(session.StartTime!, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US")) >= timeNow.AddYears(-filterAmount))
                    {
                        filteredCodingSessions.Add(session);
                    }
                    break;
            }
            
        }
        TableCreation(filteredCodingSessions);
    }

    public static void DeleteSession()
    {
        AnsiConsole.Clear();
        var idSelection = Convert.ToInt32(GetSessionId("Please type the", "Id",
            "Of the record you want to delete. Or type 0 to go back to the main menu"));
        using var connection = new SqliteConnection(ConnectionString);
        var command = $"DELETE from coding_tracker WHERE Id = {idSelection}";
        var sesionDeletion = connection.Execute(command);
        if (sesionDeletion == 0)
        {
            AnsiConsole.Write(new Markup($"Record with Id [red]{idSelection}[/] doesn't exist. Please try again"));
            Thread.Sleep(4000);
            DeleteSession();
        }
        AnsiConsole.Write(new Markup($"[red]{sesionDeletion}[/] session deleted"));
        Thread.Sleep(3000);
        AnsiConsole.Clear();
    }

    public static void UpdateSession()
    {
        AnsiConsole.Clear();
        var idSelection = Convert.ToInt32(GetSessionId("Please type the", "Id",
            "Of the record you want to update. Or type 0 to go back to the main menu"));
        using var connection = new SqliteConnection(ConnectionString);
        var checkCommand = $"SELECT * FROM coding_tracker WHERE Id = '{idSelection}'";
        var sessionValidation = connection.Query<CodingSession>(checkCommand);
        if (!sessionValidation.Any())
        {
            AnsiConsole.Write(new Markup($"Record with Id [red]{idSelection}[/] doesn't exist. Please try again"));
            Thread.Sleep(3000);
            UpdateSession();
        }

        var startTime = Validation.GetDateInput("Enter your start time:",
            "(Format dd-mm-yy HH:mm:ss)", "Or type 0 to return to the main menu");
        var endTime = Validation.GetDateInput("Enter your end time:",
            "(Format dd-mm-yy HH:mm:ss)", "Or type 0 to return to the main menu");
        var startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        var endDateTime = DateTime.ParseExact(endTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        while (startDateTime > endDateTime)
        {
            startTime = Validation.GetDateInput("Start time is after end time, please enter valid start time:",
                "(Format dd-mm-yy HH:mm)", "Or type 0 to return to main menu");
            startDateTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm:ss", new CultureInfo("en-US"));
        }
        var session = new CodingSession { StartTime = startTime, EndTime = endTime, Id = idSelection};
        var updateCommand = "UPDATE coding_tracker SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
        var sessionCreation = connection.Execute(updateCommand,session);
        AnsiConsole.Write(new Markup($"[green]{sessionCreation}[/] session updated"));
        Thread.Sleep(3000);
        AnsiConsole.Clear();

    }

    public static string GetSessionId(string? message1, string? message2, string message3)
    {
        AnsiConsole.Clear();
        GetAllSessions();
        var idSelect = AnsiConsole.Prompt(
            new TextPrompt<string>($"{message1} [green]{message2}[/] {message3}: ")
                .PromptStyle("blue")
                .AllowEmpty());
        if(idSelect == "0") UserInput.GetUserInput();

        while (!int.TryParse(idSelect, out _))
        {
            idSelect = AnsiConsole.Prompt(
                new TextPrompt<string>(
                        "[red]Invalid entry. Please enter a[/] [green]number[/] or type 0 to go back to the main menu")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if (idSelect == "0") UserInput.GetUserInput();
        }

        return idSelect;
    }

    public static void GetSessionFilterType()
    {
        AnsiConsole.Clear();
        var filterRunning = true;
        do
        {
            var filter = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a [blue]coding session[/] filter")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Days", "Weeks", "Months", 
                        "Years", "Exit"
                    }));
            string filterTime;
            switch (filter)
            {
                case "Days":
                    filterTime = FilterTime(filter);
                    GetSessionByFilterType(filter,filterTime);
                    break;
                case "Weeks":
                    filterTime = FilterTime(filter);
                    GetSessionByFilterType(filter,filterTime);
                    break;
                case "Months":
                    filterTime = FilterTime(filter);
                    GetSessionByFilterType(filter,filterTime);
                    break;
                case "Years":
                    filterTime = FilterTime(filter);
                    GetSessionByFilterType(filter,filterTime);
                    break;
                case "Exit":
                    filterRunning = false;
                    break;
            }
        } while (filterRunning);
    }

    public static string FilterTime(string filterType)
    {
        var timePeriod = AnsiConsole.Prompt(
            new TextPrompt<string>($"How many [green]{filterType} [/]would you like to see? Or type 0 to return to main menu")
                .PromptStyle("blue")
                .AllowEmpty());
        if(timePeriod == "0") UserInput.GetUserInput();
        while (string.IsNullOrWhiteSpace(timePeriod))
        {
            timePeriod = AnsiConsole.Prompt(
                new TextPrompt<string>($"[red]Invalid entry.[/] How many [green]{filterType} [/]would you like to see? Or type 0 to return to main menu")
                    .PromptStyle("blue")
                    .AllowEmpty());
            if(timePeriod == "0") UserInput.GetUserInput();
        }
        return timePeriod;
    }

    public static void TableCreation(IEnumerable<CodingSession> sessions)
    {
        AnsiConsole.Clear();
        var table = new Table();

        table.Title(new TableTitle("[blue]Coding Sessions[/]"));

        table.AddColumn(new TableColumn("[#FFA500]Id[/]").Centered());
        table.AddColumn(new TableColumn("[#104E1D]Start Time[/]").Centered());
        table.AddColumn(new TableColumn("[red]End Time[/]").Centered());
        table.AddColumn(new TableColumn("[#8F00FF]Duration[/]").Centered());

        var averageListBuilder = new List<double>();

        foreach (var session in sessions)
        {

            table.AddRow($"[#3EB489]{session.Id}[/]", $"[#3EB489]{session.StartTime}[/]", $"[#3EB489]{session.EndTime}[/]", $"[#3EB489]{session.Duration}[/]");
            averageListBuilder.Add(session.AverageMinutes);
        }

        var averageArray = averageListBuilder.ToArray();
        var average = Math.Round(averageArray.AsQueryable().Average(), 2);
        var averageTime = "";

        switch (average)
        {
            case <= 59 and < 60:
                averageTime = Convert.ToString(average, new CultureInfo("en-US")) + " minutes";
                break;
            case >= 60:
                averageTime = Convert.ToString(average, new CultureInfo("en-US")) + " hours";
                break;
        }

        table.Caption(new TableTitle($"[#87CEEB]Average coding session: {averageTime}[/]"));

        AnsiConsole.Write(table);
    }

}