using Spectre.Console;

namespace CodingTracker;

public static class UserInterface
{
    private const string TitleStyle = "[green]";
    private const string TitleString = "Coding Session";
    private const string MainMenuTitle = $"{TitleStyle}What do you want to do ?[/]";
    private const string ViewMenuTitle = $"{TitleStyle}Navigate through the months[/]";

    private static readonly string ConfirmationBaseMessage = $"Are you sure you want to {{0}} ?";

    private static readonly string DatePromptMessage =
        $"Enter the date ({Validation.DateFormat}) of the {{0}}, nothing for right now";

    private static readonly string DatePromptLiveMessage =
        $"You started the live session at {{0}}, press enter to end the session";

    private static readonly string DatePromptErrorEndBeforeStart =
        "[red]End date cannot be earlier than start date. Try again.[/]";

    public static void Title()
    {
        var title = new Text(TitleString, new Style(foreground: Color.Green1));
        var alignedTitle = Align.Center(title);
        AnsiConsole.Write(alignedTitle);
    }
    public static UserOption.MainMenu MainMenuPrompt()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<UserOption.MainMenu>()
                .Title(MainMenuTitle)
                .PageSize(Enum.GetNames<UserOption.MainMenu>().Length)
                .AddChoices(Enum.GetValues<UserOption.MainMenu>())
                .UseConverter(UserOption.MainMenuToString));
    }

    public static UserOption.ViewMenu ViewMenuPrompt()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<UserOption.ViewMenu>()
                .Title(ViewMenuTitle)
                .PageSize(Enum.GetNames<UserOption.ViewMenu>().Length)
                .AddChoices(Enum.GetValues<UserOption.ViewMenu>())
                .UseConverter(UserOption.ViewMenuToString));
    }

    public static (DateTime, DateTime) AskDateRange()
    {
        
        var startDate = AskDate("start date");
        var endDate = AskDate("end date");

        while (endDate <= startDate)
        {
            AnsiConsole.MarkupLine(DatePromptErrorEndBeforeStart);
            endDate = AskDate("end date");
        }
        
        return (startDate, endDate);
    }

    public static (DateTime, DateTime) AskDateRangeLive()
    {
        var startDate = DateTime.Now;
        AnsiConsole.MarkupLine(string.Format(DatePromptLiveMessage, startDate));
        
        // Wait user input without showing it in console
        Console.ReadKey(intercept: true);
        
        var endDate = DateTime.Now;
        return (startDate, endDate);
    }
    
    public static bool ConfirmationPrompt(string message)
    {
        return AnsiConsole.Confirm(string.Format(ConfirmationBaseMessage, message));
    }

    private static DateTime AskDate(string message)
    {
        string formattedMessage = string.Format(DatePromptMessage, message);
        string defaultValue = Validation.DateToString(DateTime.Now);
        Validation.DateTryParser(defaultValue, out DateTime parsedDate);
        
        AnsiConsole.Prompt(
            new TextPrompt<string>(formattedMessage)
                .DefaultValue(defaultValue)
                .Validate(input => Validation.DateTryParser(input, out parsedDate))
                .ValidationErrorMessage(Validation.DateErrorMessage));
        return parsedDate;
    }
}