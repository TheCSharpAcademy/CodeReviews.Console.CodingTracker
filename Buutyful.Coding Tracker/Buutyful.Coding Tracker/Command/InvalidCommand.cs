using Buutyful.Coding_Tracker.Abstraction;

namespace Buutyful.Coding_Tracker.Command;

public class InvalidCommand(string? command) : ICommand
{
    private readonly string _command = command ??= string.Empty;
    public void Execute()
    {
        Console.WriteLine($"Invalid Command : {_command}");            
    }
}