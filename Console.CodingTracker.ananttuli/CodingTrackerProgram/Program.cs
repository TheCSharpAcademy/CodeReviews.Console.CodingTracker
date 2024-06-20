namespace CodingTrackerProgram;

using Spectre.Console;

using Input;
using CodingTrackerProgram.Model;
using CodingTrackerProgram.Database;

public class Program
{
    public static void Main()
    {
        bool keepAppRunning = true;

        Connection.Init();
        ShowWelcomeScreen();

        do
        {
            keepAppRunning = StartApp();
        } while (keepAppRunning == true);

        ShowExitMessage();
    }

    public static void ShowExitMessage()
    {
        AnsiConsole.MarkupLine("\n\t[green]Thanks[/] for using [red]Coding[/][blue]Tracker[/]");
    }

    public static void ShowWelcomeScreen()
    {
        AnsiConsole.MarkupLine("\n\n[green]Welcome[/] to [red]Coding[/][blue]Tracker[/]");
    }

    public static bool StartApp()
    {
        bool isAppRunning = true;

        string option = Menu.ShowMenu();

        Console.Clear();

        switch (option)
        {
            case Menu.CREATE_SESSION:
                CreateOrUpdateSession();
                break;
            case Menu.VIEW_SESSIONS:
                ViewSessions();
                break;
            case Menu.UPDATE_SESSION:
                CreateOrUpdateSession(updateMode: true);
                break;
            case Menu.DELETE_SESSION:
                DeleteSession();
                break;
            case Menu.EXIT_PROGRAM:
                isAppRunning = false;
                break;
            default:
                AnsiConsole.WriteLine("Please select a valid menu option.");
                break;
        }

        AnsiConsole.MarkupLine("[green]Press any key to go back to the menu[/]");
        Console.ReadKey();
        Console.Clear();

        return isAppRunning;
    }

    public static CodingSession? PromptCodingSessionId()
    {
        CodingSession? existingSession = null;

        var editCodingSessionId = (Int64)Input.TryReadInput("Coding session ID", Input.ParseDecimal);
        existingSession = CodingSessionRepository.FindById(editCodingSessionId);
        if (existingSession == null)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Could not find session with ID {editCodingSessionId}[/]");
            return null;
        }

        return existingSession;
    }

    public static void DeleteSession()
    {
        CodingSession? existingSession = PromptCodingSessionId();

        if (existingSession == null)
        {
            return;
        }

        CodingSessionRepository.Delete(existingSession.Id);
    }

    public static void CreateOrUpdateSession(bool updateMode = false)
    {
        CodingSession? existingSession = null;

        if (updateMode == true)
        {
            existingSession = PromptCodingSessionId();

            if (existingSession == null)
            {
                return;
            }
        }

        const string expectedDateTimeFormat = "yyyy-MM-dd HH:mm";

        string heading = updateMode == false ? "Create session log" : "Update session log";

        AnsiConsole.MarkupLineInterpolated($"\n[b][blue]{heading}[/][/]\n");

        AnsiConsole.MarkupLine("[grey]Note: Date times should be 24hr format e.g. [/][blue]2024-01-01 12:15[/]");

        string validStartDateTimeString = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Start[/] date time: ")
                .PromptStyle("blue")
                .ValidationErrorMessage("[red]That's not a valid date time[/]")
                .Validate(startDateTimeInput =>
                    {
                        Tuple<bool, DateTime> parseResult = Input.ParseDateTimeExactFormat(
                            startDateTimeInput,
                            expectedDateTimeFormat
                        );

                        if (parseResult.Item1 == false)
                        {
                            return ValidationResult.Error("\t[red]Please enter valid date format[/]");
                        }

                        return ValidationResult.Success();
                    }
                )
        );

        DateTime startDateTime = DateTime.Parse(validStartDateTimeString);

        string validEndDateTimeString = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]End[/] date time: ")
                .PromptStyle("blue")
                .ValidationErrorMessage("[red]That's not a valid date time[/]")
                .Validate(endDateTimeInput =>
                    {
                        Tuple<bool, DateTime> parseResult = Input.ParseDateTimeExactFormat(endDateTimeInput, expectedDateTimeFormat);

                        if (parseResult.Item1 == false)
                        {
                            return ValidationResult.Error("\t[red]Please enter valid date format[/]");
                        }

                        if (parseResult.Item2 < startDateTime)
                        {
                            return ValidationResult.Error("\t[red]End date time must be later than start date time[/]");
                        }

                        return ValidationResult.Success();
                    }
                )
        );

        DateTime endDateTime = DateTime.Parse(validEndDateTimeString);

        if (existingSession != null)
        {
            CodingSessionRepository.Update(existingSession.Id, startDateTime, endDateTime);
            return;
        }

        CodingSessionRepository.Create(
            startDateTime,
            DateTime.Parse(validEndDateTimeString)
        );
    }

    static void ViewSessions()
    {
        List<CodingSession> sessions = CodingSessionRepository.FindAll();

        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("\n\t[red]No records found[/]");
            return;
        }

        var table = new Table();

        table.AddColumn(new TableColumn("Id").Centered());
        table.AddColumn(new TableColumn("Start time").Centered());
        table.AddColumn(new TableColumn("End time").Centered());
        table.AddColumn(new TableColumn("Duration").Centered());

        foreach (CodingSession session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StartDateTime.ToString(),
                session.EndDateTime.ToString(),
                string.Format("{0:%d} d {0:%h} h {0:%m} m {0:%s} s", session.Duration)
            );
        }

        AnsiConsole.Write(table);
    }
}