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

    public static bool ConfirmationPrompt(string message)
    {
        return AnsiConsole.Confirm(string.Format(ConfirmationBaseMessage, message));
    }

    public static DateTime AskDate(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<DateTime>(string.Format(DatePromptMessage, message))
                .DefaultValue(DateTime.Now)
                .WithConverter(Validation.DateToString)
                .ValidationErrorMessage(Validation.DateErrorMessage));
    }
}