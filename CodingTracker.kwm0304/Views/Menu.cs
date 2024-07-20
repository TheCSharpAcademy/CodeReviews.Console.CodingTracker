using Spectre.Console;

public class Menu<T>(string title, List<T> choices) where T : notnull
{
  public string Title { get; set; } = title;
  public List<T> Choices { get; set; } = choices;

  public T Show()
  {
    return AnsiConsole.Prompt(
        new SelectionPrompt<T>()
            .Title(Title)
            .AddChoices(Choices)
    );
  }
}