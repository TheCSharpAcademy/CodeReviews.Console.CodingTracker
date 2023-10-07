using System.Configuration;
using CodingTracker.wkktoria;

var pathToSaveDb = Path.GetDirectoryName(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent
    .FullName);
var dbPath = Path.Combine(pathToSaveDb, ConfigurationManager.AppSettings.Get("DbPath"));
var connectionString = ConfigurationManager.AppSettings.Get("ConnectionString") + dbPath;

var dbManager = new DbManager(connectionString);

dbManager.Initialize();