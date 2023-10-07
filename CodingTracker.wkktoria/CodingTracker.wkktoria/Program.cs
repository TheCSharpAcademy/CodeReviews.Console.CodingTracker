using System.Configuration;
using System.Globalization;
using CodingTracker.wkktoria;
using CodingTracker.wkktoria.Controllers;
using CodingTracker.wkktoria.Services;
using CodingTracker.wkktoria.Ui;

var customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
customCulture.NumberFormat.NumberDecimalSeparator = ".";

Thread.CurrentThread.CurrentCulture = customCulture;

var pathToSaveDb = Path.GetDirectoryName(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent
    .FullName);
var dbPath = Path.Combine(pathToSaveDb, ConfigurationManager.AppSettings.Get("DbPath"));
var connectionString = ConfigurationManager.AppSettings.Get("ConnectionString") + dbPath;

var dbManager = new DbManager(connectionString);

dbManager.Initialize();

var service = new CodingService(connectionString);
var ui = new UserInterface(new CodingController(service));

ui.Run();