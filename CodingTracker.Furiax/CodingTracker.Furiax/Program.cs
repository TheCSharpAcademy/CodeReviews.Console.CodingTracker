using CodingTracker.Furiax;
using System.Configuration;

var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

Crud.CreateTable(connectionString);
UserInput.GetUserInput(connectionString);