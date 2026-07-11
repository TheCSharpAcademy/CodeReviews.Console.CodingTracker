using CodingTracker.Controller;
using CodingTracker.Database;
using CodingTracker.Service;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
string connectionString = configuration.GetConnectionString("DefaultConnection")!;
var db = new TrackerDB(connectionString);
var service = new CodingTrackerService(db);
var controller = new CodingController(service);
controller.Start();