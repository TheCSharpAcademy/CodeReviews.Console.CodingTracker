using CodingTracker.DzemalKurtic.Controllers;
using CodingTracker.DzemalKurtic.Data;
using CodingTracker.DzemalKurtic.Views;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection");

Database.Initialize(connectionString);
var controller = new CodingSessionController(connectionString);
var ui = new UserInterface(controller);
ui.MainMenu();