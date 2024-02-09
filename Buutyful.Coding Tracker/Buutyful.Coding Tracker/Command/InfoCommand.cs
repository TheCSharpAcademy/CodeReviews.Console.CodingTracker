﻿using Buutyful.Coding_Tracker.Abstraction;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.Command;

public class InfoCommand : ICommand
{
    private readonly Dictionary<Commands, string> _commands = new()
        {
            {Commands.Info, "Gets you all the infos u need" },
            {Commands.Menu, "Return to the main menu"},
            {Commands.View, "Display database records"},
            {Commands.Create, "Create new entry" },
            {Commands.Update, "Update record" },
            {Commands.Delete, "Delete record" },
            {Commands.Back, "Goes back to past state" },
            {Commands.Forward, "Goes to forward state" },
            {Commands.Clear, "Clear console"},
            {Commands.Break, "Breaks out of input loop"},
            {Commands.Quit, "Quit the application" }
        };

    public void Execute()
    {
        Console.WriteLine("============================");
        Console.WriteLine("Use the words marked by the squere brakets [] to navigate");
        Console.WriteLine("Here's a list of general commands:");
        foreach (var info in _commands)
        {
            Console.WriteLine($"[{info.Key}]: {info.Value}");
        }
        Console.WriteLine("Press Any Key to continue");
        Console.WriteLine("============================");
        Console.WriteLine(Test());
    }
    private static string Test() =>
        AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Select command:")
        .PageSize(10)
        .MoreChoicesText("[grey]===================[/]")
        .AddChoices(Enum.GetValues(typeof(Commands))
        .Cast<Commands>()
        .Select(o => o.ToString())));

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