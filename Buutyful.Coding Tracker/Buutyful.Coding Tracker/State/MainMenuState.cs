using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.State;

public class MainMenuState(StateManager stateManager) : IState
{
    private readonly StateManager _manager = stateManager;

    public ICommand GetCommand()
    {
        var command = Console.ReadLine()?.ToLower();
        return MenuSelector(command);
    }

    public void Render()
    {
        AnsiConsole.MarkupLine("Track your habits. " +
            "Select [yellow][[info]][/] for navigation help");
    }
    private ICommand MenuSelector(string? command)
    {
        return command switch
        {
            "info" => new InfoCommand(),
            "view" => new SwitchStateCommand(_manager, new ViewState(_manager)),
            "create" => new SwitchStateCommand(_manager, new CreateState(_manager)),
            "update" => new SwitchStateCommand(_manager, new UpdateState(_manager)),
            "delete" => new SwitchStateCommand(_manager, new DeleteState(_manager)),
            "back" => new SwitchStateCommand(_manager, _manager.PastState()),
            "forward" => new SwitchStateCommand(_manager, _manager.FutureState()),
            "clear" => new ClearCommand(),
            "quit" => new QuitCommand(),
            _ => new InvalidCommand(command),
        };
    }
}
