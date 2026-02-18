using CodingTracker;
using CodingTracker.Data;

// Initialize the database (creates the table if it doesn't exist)
var db = new Database();
db.Initialize();

// Start the user interface
UserInterface ui = new();
ui.MainMenu();
