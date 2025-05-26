using CodingTracker.Golvi1124.Data;
using CodingTracker.Golvi1124.UI;

var dataAccess = new DataAccess();

dataAccess.CreateDatabase();

//SeedData.SeedRecords(20); // comment out when done with testing

UserInterface.MainMenu();