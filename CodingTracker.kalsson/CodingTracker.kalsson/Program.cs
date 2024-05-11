using CodingTracker.kalsson.Data;
using CodingTracker.kalsson.UI;
using Microsoft.Extensions.Configuration;

// Configure and build the configuration
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Build();

// Initialize the database using the connection string from the configuration
var databaseInitializer = new DatabaseInitializer(configuration.GetConnectionString("DefaultConnection"));
databaseInitializer.InitializeDatabase();

var ConsoleMenu = new ConsoleMenu(new CodingSessionRepository(configuration.GetConnectionString("DefaultConnection")));
ConsoleMenu.ShowMenu();

Console.ReadLine();