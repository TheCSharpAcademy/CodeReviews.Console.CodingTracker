using CodingTracker.Furiax;
using Microsoft.Data.Sqlite;
using System.Configuration;

var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

Crud.CreateTable(connectionString);
UserInput.GetUserInput(connectionString);