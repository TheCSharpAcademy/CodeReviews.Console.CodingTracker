using CodingTracker.Furiax;

var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

Crud.CreateTable(connectionString);
UserInput.GetUserInput(connectionString);