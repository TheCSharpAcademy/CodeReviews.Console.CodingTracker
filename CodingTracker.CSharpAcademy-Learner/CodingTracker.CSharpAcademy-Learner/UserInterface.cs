using CodingTracker.CSharpAcademy_Learner.Controllers;
using CodingTracker.CSharpAcademy_Learner.Models;
using Spectre.Console;
using static CodingTracker.CSharpAcademy_Learner.Enums;
using static System.Collections.Specialized.BitVector32;

namespace CodingTracker.CSharpAcademy_Learner
{
    internal class UserInterface
    {
        private readonly CodingController _codingController;
        private readonly StopwatchService _stopwatchService;

        public UserInterface(CodingController codingController)
        {
            _codingController = codingController;
            _stopwatchService = new StopwatchService(codingController);
        }

        internal void MainMenu()
        {
            bool closeApp = false;

            while (!closeApp)
            {
                Console.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MenuAction>()
                    .Title("[bold cyan]CODING TRACKER MENU[/]")
                    .UseConverter(choice => choice.GetDisplayName())
                    .AddChoices(Enum.GetValues<MenuAction>())
                    );

                switch (choice)
                {
                    case MenuAction.ViewRecords:
                        ViewAllSessions();
                        break;
                    case MenuAction.AddRecord:
                        AddRecord();
                        break;
                    case MenuAction.UpdateRecord:
                        UpdateRecord();
                        break;
                    case MenuAction.DeleteRecord:
                        DeleteRecord();
                        break;
                    case MenuAction.StopwatchService:
                        _stopwatchService.TrackSession();
                        break;
                    case MenuAction.Exit:
                        AnsiConsole.MarkupLine("[cyan]Goodbye :)[/]");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private void ViewAllSessions()
        {
            var sessions = _codingController.GetAllSessions();

            TableVisualisationEngine.DisplaySessions(sessions);

            UserInterfaceHelpers.WaitForAnyKey();
        }

        private void AddRecord()
        {
            if(!UserInterfaceHelpers.WaitForEnterOrEscape_CheckForEnter("You are about to add a new record.")) { return; }

            var sessionInfo = GetSessionInfoFromUserInput();

            _codingController.InsertSession(sessionInfo.startDate.ToString(Validation.DatabaseDateFormat), sessionInfo.endDate.ToString(Validation.DatabaseDateFormat), sessionInfo.duration);

            UserInterfaceHelpers.WaitForAnyKey();
        }

        private void UpdateRecord()
        {
            if (!UserInterfaceHelpers.WaitForEnterOrEscape_CheckForEnter("You are about to edit a record.")) { return; }

            var selectedSession = SessionSelector("Choose a session to edit");

            var sessionInfo = GetSessionInfoFromUserInput();

            _codingController.UpdateSession(selectedSession.Id, sessionInfo.startDate.ToString(Validation.DatabaseDateFormat), sessionInfo.endDate.ToString(Validation.DatabaseDateFormat), sessionInfo.duration);

            UserInterfaceHelpers.WaitForAnyKey();
        }

        private void DeleteRecord()
        {
            if (!UserInterfaceHelpers.WaitForEnterOrEscape_CheckForEnter("You are about to delete a record.")) { return; }

            var selectedSession = SessionSelector("Choose a session to delete:");

            AnsiConsole.MarkupLine(DisplaySessionInfo(selectedSession));

            if (AnsiConsole.Confirm("Are you sure you want to delete this session?"))
            {
                _codingController.DeleteSession(selectedSession.Id);
                AnsiConsole.MarkupLine("\nSucessfully deleted!");
            }
            else
            {
                AnsiConsole.MarkupLine("\nNOT DELETED");
            }

            UserInterfaceHelpers.WaitForAnyKey();
        }

        private DateTime GetDateInput(string prompt, DateTime? minDate = null)
        {
            while (true)
            {
                var dateInput = AnsiConsole.Ask<string>($"[yellow]{prompt}[/]");

                if (Validation.IsValidDate(dateInput, out DateTime parsedDate))
                {
                    if (minDate.HasValue && !Validation.IsValidDateRange(minDate.Value, parsedDate))
                    {
                        AnsiConsole.MarkupLine("[red]Start date cannot be equal to or after the end date[/]");
                        continue;
                    }

                    return parsedDate;
                }

                AnsiConsole.MarkupLine("[red]Invalid format. Please use exactly dd-MM-yyyy HH:mm (e.g., 25-10-2026 14:30).[/]");
            }
        }

        private CodingSession SessionSelector(string prompt)
        {
            var sessions = _codingController.GetAllSessions();

            var selectedSession = AnsiConsole.Prompt(
                new SelectionPrompt<CodingSession>()
                .Title(prompt)
                .UseConverter(s => DisplaySessionInfo(s))
                .AddChoices(sessions)
                );

            return selectedSession;
        }

        private (DateTime startDate, DateTime endDate, int duration) GetSessionInfoFromUserInput()
        {
            var startDate = GetDateInput("Enter the start date (use format dd/MM/yyyy HH:mm): ");
            var endDate = GetDateInput("Enter the end date (use format dd/MM/yyyy HH:mm): ", startDate);

            var duration = Validation.CalculateDurationInSeconds(startDate, endDate);

            return (startDate, endDate, duration);
        }

        private string DisplaySessionInfo(CodingSession session)
        {
            return $"Start Time: {session.StartTime} - End Time: {session.EndTime} - Duration: {Validation.ShowDurationInFriendlyFormat(session.Duration)}";
        }
    }
}
