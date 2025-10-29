using System.Configuration;
using CodingTracker;

System.Collections.Specialized.NameValueCollection sAll = ConfigurationManager.AppSettings;
// TODO: what?
string? connectionString = sAll.Get("ConnectionString") ?? throw new Exception("database connection string cannot be empty");

Database db = new(connectionString);
db.Migrate();

CodingController codingController = new(db);

UserInterface.MainMenu(codingController);
