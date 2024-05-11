using CodingTracker.kalsson.Configuration;
using CodingTracker.kalsson.Data;
using CodingTracker.kalsson.Startup;

var configuration = ConfigurationHelper.BuildConfiguration();
var dbInitializer = new DatabaseInitializerWrapper(configuration);
dbInitializer.Initialize();

var appStarter = new ApplicationStarter(configuration);
appStarter.Start();