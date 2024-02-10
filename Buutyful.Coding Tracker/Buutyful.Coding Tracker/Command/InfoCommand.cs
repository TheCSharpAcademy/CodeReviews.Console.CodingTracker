using Buutyful.Coding_Tracker.Abstraction;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.Command;

public class InfoCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("============================");
        Console.WriteLine("Use the words marked by the squere brakets [] to navigate");
        Console.WriteLine("Here's a list of general commands:");
        foreach (var info in Constants.MapCommands)
        {
            Console.WriteLine($"[{info.Key}]: {info.Value}");
        }
        Console.WriteLine("Press Any Key to continue");
        Console.WriteLine("============================");
       
    }
}
public enum Commands
{
    Info,
    Menu,
    View,
    Create,
    Update,
    Delete,
    Back,
    Forward,
    Clear,
    Break,
    Quit
}