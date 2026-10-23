using CodingTracker.Enums;
using Spectre.Console;
using System.Globalization;
namespace CodingTracker.UI
{
    internal class UserInput
    {
        private static string[] MainMenuOptions = { "Start Coding Session", "Manage Coding Sessions", "Exit" };
        private static string[] ManageSessionsMenuOptions = { "View All Sessions" ,"Delete Session" ,"Update Session" ,"Exit to Main Menu" };

        internal static string GetMainMenuInput()
        {
            return AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[white bold]What would you like to do?[/]")
                    .AddChoices(MainMenuOptions));
        }

        internal static DateTime GetDateTime(string startOrEnd)
        {
            const string requiredFormat = "yyyy-MM-dd HH:mm";
            ClearDisplay();
            string input = AnsiConsole.Prompt(
                new TextPrompt<string>($"[white bold]Enter the {startOrEnd} time[/] [blue bold](yyyy-MM-dd HH:mm)[/]: ")
                    .Validate(time =>
                    {
                        bool validDate = DateTime.TryParseExact(
                            time,
                            requiredFormat,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out _);

                        return validDate
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red italic]Invalid date format![/]");
                    }))
                .Trim();

            return DateTime.ParseExact(input, requiredFormat, CultureInfo.InvariantCulture);
        }
        internal static string GetManageSessionMenuOption()
        {
            ClearDisplay();

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[White bold]Select an option to manage coding sessions.[/]")
                .AddChoices(ManageSessionsMenuOptions));
        }

        internal static int GetCodingSessionIdInput(List<int> sessionIds)
        {
            ClearDisplay();

            return AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .Title("[white bold]Select the session ID[/]:")
                .AddChoices(sessionIds));

        }

        internal static SortOrderOptions GetOrderOption()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<SortOrderOptions>()
                .Title("[white bold]Select the order you would like to view the coding sessions in[/]:")
                .AddChoices(SortOrderOptions.Ascending, SortOrderOptions.Descending));
        }

        internal static TrackingType GetTrackingType()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<TrackingType>()
                .Title("Select the tracking type:")
                .AddChoices(TrackingType.AutoTimer, TrackingType.Manual));
        }

        internal static void DisplayMessage(string message)
        {
            AnsiConsole.MarkupLine($"[White bold]{message}[/]");
        }

        internal static void AwaitUserInput()
        {
            Console.ReadLine();
        }

        internal static bool Confirm(string message)
        {
            return AnsiConsole.Confirm($"[White bold]{message}[/]");
        }
        
        internal static void ConfirmationMessage(string message)
        {
            AnsiConsole.MarkupLine($"[white bold]{message}[/]");
        }

        internal static void Continue()
        {
            AnsiConsole.MarkupLine("[grey]Press 'Enter' to continue.[/]");
            Console.ReadKey();
        }

        internal static void Error(string message)
        {
            AnsiConsole.MarkupLine($"[red italic]{message}[/]");
        }

        internal static void Goodbye()
        {
            AnsiConsole.MarkupLine("[white bold]Thank you for using the Coding tracker app![/]");
            Continue();
        }

        internal static void DisplayMainMenuFiglet()
        {
            var figlet = new FigletText("Coding Tracker")
                .Color(Color.LightGreen);

            AnsiConsole.Write(figlet);
        }

        internal static void ClearDisplay()
        {
            AnsiConsole.Clear();
        }

    }
}
