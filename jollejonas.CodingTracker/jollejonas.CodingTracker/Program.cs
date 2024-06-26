using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;
using jollejonas.CodingTracker.Models;
using Spectre.Console;
using jollejonas.CodingTracker.Data;
using jollejonas.CodingTracker.Controllers;


string currentDirectory = Directory.GetCurrentDirectory();

string projectDirectory = Path.Combine(currentDirectory, @"..\..\..");
string appSettingsPath = Path.Combine(projectDirectory, "Properties");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"{appSettingsPath}\\appsettings.json", optional: true, reloadOnChange: true)
    .Build();

string? connectionString = configuration.GetConnectionString("DefaultConnection");
string goalStatus = "";
var menuOptions = new List<MenuOption>
    {
        new MenuOption { Id = 1, Description = "Get all coding sessions" },
        new MenuOption { Id = 2, Description = "Start new session" },
        new MenuOption { Id = 3, Description = "Start new live session" },
        new MenuOption { Id = 4, Description = "Update session" },
        new MenuOption { Id = 5, Description = "Delete session" },
        new MenuOption { Id = 6, Description = "Show coding sessions by(year/month/week)" },
        new MenuOption { Id = 7, Description = "Set goal" },
        new MenuOption { Id = 0, Description = "Quit" }
};

using (IDbConnection db = DatabaseManager.Connection(connectionString))
{
    DatabaseManager.EnsureSessionDatabaseCreated(db);
    var codingSessionController = new CodingSessionController(db);
    codingSessionController.SeedData(db);
    db.Close();
}

using (IDbConnection db = DatabaseManager.Connection(connectionString))
{
    DatabaseManager.EnsureGoalDatabaseCreated(db);
    db.Close();
}

while (true) {
    Console.Clear();
    var goalController = new GoalController(DatabaseManager.Connection(connectionString));
    goalStatus = goalController.CalculateGoalActualDifference();
    Console.WriteLine(goalStatus);


    var menuSelection = AnsiConsole.Prompt(
        new SelectionPrompt<MenuOption>()
        .Title("Select an option")
        .PageSize(10)
        .AddChoices(menuOptions)
        .UseConverter(option => option.Description));

    if(menuSelection.Id == 0)
    {
        break;
    }

    using (IDbConnection db = DatabaseManager.Connection(connectionString))
    {
        var codingSessionController = new CodingSessionController(db);
        codingSessionController.SelectOperation(menuSelection.Id);
    }
    Console.WriteLine("Press any key");
    Console.ReadKey();
}