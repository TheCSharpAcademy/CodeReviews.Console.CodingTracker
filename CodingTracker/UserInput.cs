using Spectre.Console;
using CodingTracker.Controllers;
using CodingTracker;

namespace CodingTracker;

internal class UserInterface
{
  private readonly CodingController _controller = new CodingController();

  public void MainMenu()
  {
    bool closeApp = false;
    while (closeApp == false)
    {
      Console.Clear();

      var actionChoice = AnsiConsole.Prompt(
          new SelectionPrompt<MenuAction>()
          .Title("What do you want to do next?")
          .AddChoices(Enum.GetValues<MenuAction>()));

      switch (actionChoice)
      {
        case MenuAction.ViewSession:
          _controller.ViewSessions();
          break;
        case MenuAction.AddSession:
          _controller.AddSession();
          break;
        case MenuAction.DeleteSession:
          _controller.DeleteSession();
          break;
        case MenuAction.Exit:
          closeApp = true;
          break;

      }
    }
  }
}