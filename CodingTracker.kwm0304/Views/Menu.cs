using Spectre.Console;

public class Menu<T> where T : notnull
{
  public string Title { get; set; }
  public List<T> Choices { get; set; }

  public Menu(string title, List<T> choices)
  {
    Title = title;
    Choices = choices;
  }

  public T Show()
  {
    AnsiConsole.WriteLine("[blue]Press Escape to return[/]");
    return AnsiConsole.Prompt(
        new SelectionPrompt<T>()
            .Title(Title)
            .AddChoices(Choices)
    );
  }
}
