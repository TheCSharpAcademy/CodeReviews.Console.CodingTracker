using System.Configuration;
using CodingTracker;
using DB;

var dbConnString = ConfigurationManager.AppSettings
    .Get("sqliteConnString") ?? throw new ArgumentNullException("missing 'sqliteConnString' in App.config");

var dbContext = new CodingTimeDBContext(dbConnString);
var app = new App(dbContext);

app.Run();