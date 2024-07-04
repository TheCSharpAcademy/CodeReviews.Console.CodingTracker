using CodingTracker;





Console.WriteLine(Utilities.titleText);
var dataAccess = new DataAccess();

dataAccess.CreateDatabase();

if (dataAccess.IsDatabaseEmpty())
	SeedData.SeedRecords(20);

UserInterface.MainMenu();