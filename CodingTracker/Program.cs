// See https://aka.ms/new-console-template for more information
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using CodingTracker;

var sAll = ConfigurationManager.AppSettings;
var connectionString = sAll.Get("ConnectionString");

Database.Migrate();

// might be the main loop
var title = new FigletText("Coding Tracker");
AnsiConsole.Write(title);

var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("what do?")
        .PageSize(10)
        .MoreChoicesText("move up or down to choose")
        .AddChoices([
            "Create", "Read", "Update", "Delete"
            ])
        );

AnsiConsole.WriteLine($"{choice}");

switch (choice)
{
    case "Create":
        // input start end
        CodingController.Create();
        break;
    case "Read":
        // make table
        // col: id start end duration
        CodingController.Read();
        break;
    case "Update":
        // selection with record
        CodingController.Update();
        break;
    case "Delete":
        CodingController.Delete();
        break;
    default:
        AnsiConsole.WriteLine("invalid");
        break;
}

