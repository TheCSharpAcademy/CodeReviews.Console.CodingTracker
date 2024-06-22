using CodingTrackerProgram.Database;
using CodingTrackerProgram.Model;
using Spectre.Console;


namespace CodingTrackerProgram.Services;

public class CodingSessionService
{
    public static CodingSession? PromptCodingSessionId()
    {
        CodingSession? existingSession = null;

        var editCodingSessionId = (Int64)Input.Parser.TryReadInput("Coding session ID", Input.Parser.ParseDecimal);
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

        AnsiConsole.MarkupLine("[grey]Note: Date times must be YYYY-mm-dd hh:mm with 24hr time e.g. [/][blue]2024-12-31 14:15[/]");

        string validStartDateTimeString = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Start[/] date time: ")
                .PromptStyle("blue")
                .ValidationErrorMessage("[red]That's not a valid date time[/]")
                .Validate(startDateTimeInput =>
                    {
                        Tuple<bool, DateTime> parseResult = Input.Parser.ParseDateTimeExactFormat(
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
                        Tuple<bool, DateTime> parseResult = Input.Parser.ParseDateTimeExactFormat(endDateTimeInput, expectedDateTimeFormat);

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

    public static void ViewSessions()
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