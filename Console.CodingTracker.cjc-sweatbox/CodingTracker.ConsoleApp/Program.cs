using CodingTracker.ConsoleApp.Views;
using CodingTracker.Controllers;
using Spectre.Console;
using System.Configuration;
using CodingTracker.ConsoleApp.Extensions;

namespace CodingTracker.ConsoleApp;

internal class Program
{
    #region Methods

    /// <summary>
    /// Main insertion point of the program.
    /// Gets config settings and launches the main menu.
    /// </summary>
    /// <param name="args">Any arguments passed in.</param>
    private static void Main(string[] args)
    {
        try
        {
            // Read required configuration settings.
            string databaseConnectionString = ConfigurationManager.AppSettings.GetString("DatabaseConnectionString");
            bool seedDatabase = ConfigurationManager.AppSettings.GetBoolean("SeedDatabase");

            // Create the required services.
            var codingSessionController = new CodingSessionController(databaseConnectionString);
            var codingGoalController = new CodingGoalController(databaseConnectionString);

            // Generate seed data if required.
            if (seedDatabase)
            {
                // Could be a long(ish) process, so show a spinner while it works.
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Aesthetic)
                    .Start("Generating seed data. Please wait...", ctx =>
                    {
                        codingSessionController.SeedDatabase();
                    });
                AnsiConsole.WriteLine("Seed data generated.");
            }

            // Show the main menu.
            var mainMenu = new MainMenuPage(codingSessionController, codingGoalController);
            mainMenu.Show();
        }
        catch (Exception exception)
        {
            MessagePage.Show("Error", exception);
        }
        finally
        {
            Environment.Exit(0);
        }
    }

    #endregion
}
