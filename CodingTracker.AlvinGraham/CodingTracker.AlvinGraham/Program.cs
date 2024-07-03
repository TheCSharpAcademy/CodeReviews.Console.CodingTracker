using CodingTracker;




var dataAccess = new DataAccess();

dataAccess.CreateDatabase();

SeedData.SeedRecords(20);

UserInterface.MainMenu();