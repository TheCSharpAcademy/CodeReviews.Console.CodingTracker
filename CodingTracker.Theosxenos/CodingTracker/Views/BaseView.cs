namespace CodingTracker.Views;

public class BaseView
{
    public void ShowSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green]{message}[/]");
        Console.ReadKey();
    }

    public void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
        Console.ReadKey();
    }

    public bool AskConfirm(string message)
    {
        return AnsiConsole.Confirm(message);
    }

    public T AskInput<T>(string prompt, Func<T, bool>? validator = null, string? errorMessage = null,
        T? defaultValue = default)
    {
        var textPrompt = new TextPrompt<T>(prompt);

        if (defaultValue != null)
            textPrompt.DefaultValue(defaultValue);
        if (validator != null)
            textPrompt.Validate(validator,
                string.IsNullOrEmpty(errorMessage) ? textPrompt.ValidationErrorMessage : errorMessage);

        return AnsiConsole.Prompt(textPrompt);
    }

    public T ShowMenu<T>(IEnumerable<T> menuOptions, string title = "Select a menu option:", int pageSize = 10,
        Func<T, string>? converter = null)
        where T : notnull
    {
        AnsiConsole.Clear();

        var prompt = new SelectionPrompt<T>
        {
            Title = title,
            PageSize = pageSize
        };
        prompt.AddChoices(menuOptions);

        if (converter != null) prompt.UseConverter(converter);

        return AnsiConsole.Prompt(prompt);
    }
}