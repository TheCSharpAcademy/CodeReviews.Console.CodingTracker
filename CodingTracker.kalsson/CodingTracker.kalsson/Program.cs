using CodingTracker.kalsson.Configuration;
using CodingTracker.kalsson.Data;
using CodingTracker.kalsson.UI;
using Microsoft.Extensions.Configuration;

var configuration = ConfigurationHelper.BuildConfiguration();
InitializeDatabase(configuration);
StartApplication(configuration);

static void InitializeDatabase(IConfiguration configuration)
{
    // Initialize the database using the connection string from the configuration
    var databaseInitializer = new DatabaseInitializer(configuration.GetConnectionString("DefaultConnection"));
    databaseInitializer.InitializeDatabase();
}

static void StartApplication(IConfiguration configuration)
{
    // Start the main application menu
    var consoleMenu = new ConsoleMenu(new CodingSessionRepository(configuration.GetConnectionString("DefaultConnection")));
    consoleMenu.ShowMenu();
}